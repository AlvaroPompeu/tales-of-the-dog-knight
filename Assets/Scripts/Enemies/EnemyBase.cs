using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    protected GameObject player;
    protected SpawnManager spawnManager;
    protected Animator enemyAnimator;
    protected Rigidbody enemyRigidbody;
    protected Slider healthBar;
    protected Vector3 playerDir;
    protected float currentHealth;

    protected float attackRange;
    protected float moveSpeed;
    protected float attackSpeed;
    protected float maxHealth;    
    protected float minDamage;
    protected float maxDamage;
    protected float staggerTime;

    protected bool isStaggered;
    protected bool isAttacking;

    public bool CanDealDamage { get; protected set; }

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
    void Update()
    {
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

    private void ChasePlayer()
    {
        // Rotate the enemy towards the player
        FacePlayer();

        // Move the enemy
        enemyRigidbody.MovePosition(transform.position + (playerDir * moveSpeed * Time.deltaTime));

        // Play the moving animation
        enemyAnimator.SetBool("bMoving", true);
    }

    protected void FacePlayer()
    {
        // Get the player direction vector
        playerDir = (player.transform.position - transform.position).normalized;

        //playerDir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(playerDir.z, playerDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -angle + 90f, 0);
    }

    protected virtual void Attack()
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

    protected virtual IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        isAttacking = false;
        CanDealDamage = false;
    }

    public virtual void OnHit(int damage, bool critical)
    {
        // The enemy can only be hit if he is not staggered and alive
        if (!isStaggered && currentHealth > 0)
        {
            HUDManager.Instance.CreateFloatingDamageText(transform.position, damage, critical);
            enemyAnimator.SetTrigger("tHit");
            isStaggered = true;

            //Disable the enemy to deal damage, in case he is hit in the middle of an attack
            CanDealDamage = false;

            // Apply the damage and check if the enemy dies
            currentHealth -= damage;
            healthBar.value = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                Die();
            }

            StartCoroutine(RemoveStagger());
        }
    }

    IEnumerator RemoveStagger()
    {
        yield return new WaitForSeconds(staggerTime);
        isStaggered = false;
    }

    protected virtual void Die()
    {
        enemyAnimator.SetBool("bDead", true);

        // Destroy the enemy after some seconds
        spawnManager.enemyCount--;
        Destroy(gameObject, 3f);

        // Disable the minimap icon
        SpriteRenderer minimapIcon = GetComponentInChildren<SpriteRenderer>();
        minimapIcon.enabled = false;

        // Disable the enemy rigid body and this script
        enemyRigidbody.isKinematic = true;
        this.enabled = false;
    }

    public virtual int Damage()
    {
        float damage = Random.Range(minDamage, maxDamage);

        return Mathf.RoundToInt(damage);
    }
}
