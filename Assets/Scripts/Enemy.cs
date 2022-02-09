using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private Animator enemyAnimator;
    private SpawnManager spawnManager;
    private Rigidbody enemyRigidbody;
    private Slider healthBar;
    private Vector3 playerDir;

    protected float attackRange = 2f;
    protected float moveSpeed = 8f;
    protected float attackSpeed = 1f;
    protected float maxHealth = 100f;
    protected float currentHealth;
    protected float attackDamage = 10f;
    protected float staggerTime = 0.3f;

    private bool isStaggered;
    private bool isAttacking;

    public bool CanDealDamage { get; protected set; }
    public float AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the player Game Object
        player = GameObject.Find("Player");

        // Get the spawn manager component
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        // Get the animator component
        enemyAnimator = GetComponent<Animator>();

        // Get the rigid body component
        enemyRigidbody = GetComponent<Rigidbody>();

        // Get the slider component used for the health bar
        healthBar = GetComponentInChildren<Slider>();

        // Set initial health
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get the player direction vector
        playerDir = (player.transform.position - transform.position).normalized;

        // The enemy should only attack if the attack is ready and isn't staggered
        if (!isAttacking && !isStaggered)
        {
            // Attack the player if he is within the attack range
            float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distFromPlayer <= attackRange)
            {
                Attack();
            }
        }
    }

    private void FixedUpdate()
    {
        // The enemy should only move if he isn't attacking and isn't staggered
        if (!isAttacking && !isStaggered)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Rotate the enemy towards the player
        FacePlayer();

        // Move the enemy
        enemyRigidbody.MovePosition(transform.position + (playerDir * moveSpeed * Time.deltaTime));

        // Play the moving animation
        enemyAnimator.SetBool("bMoving", true);
    }

    void FacePlayer()
    {
        float angle = Mathf.Atan2(playerDir.z, playerDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -angle + 90f, 0);
    }

    void Attack()
    {
        // Rotate the enemy towards the player
        FacePlayer();

        // Play the attack animation
        enemyAnimator.SetTrigger("tAttack");

        // Enable the enemy to damage the player
        CanDealDamage = true;

        // Limits the attack speed
        isAttacking = true;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        isAttacking = false;
        CanDealDamage = false;
    }

    public void OnHit(float damage)
    {
        // The enemy can only be hit if he is not staggered
        if (!isStaggered)
        {
            enemyAnimator.SetTrigger("tHit");
            isStaggered = true;

            //Disable the enemy to deal damage, in case he is hit in the middle of an attack
            CanDealDamage = false;

            // Apply the damage and check if the enemy dies
            currentHealth -= damage;
            healthBar.value = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                enemyAnimator.SetBool("bDead", true);

                // Destroy the enemy after some seconds
                spawnManager.enemyCount--;
                Destroy(gameObject, 5f);

                // Disable this script
                enemyRigidbody.isKinematic = true;
                this.enabled = false;
            }

            StartCoroutine(RemoveStagger());
        }
    }

    IEnumerator RemoveStagger()
    {
        yield return new WaitForSeconds(staggerTime);
        isStaggered = false;
    }
}
