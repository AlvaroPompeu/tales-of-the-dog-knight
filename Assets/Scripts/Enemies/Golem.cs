using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyBase
{
    [SerializeField] Collider rightHand;
    [SerializeField] Collider leftHand;
    [SerializeField] AudioClip attackSound;

    private float damageReduction = 0.5f;

    protected string bossName;

    private void Awake()
    {
        attackRange = 5f;
        moveSpeed = 5f;
        maxHealth = 500f;
        staggerTime = 0.3f;
        bossName = "Ancient Golem";

        // Activate boss fight mode
        HUDManager.Instance.SetupBossFight(true, bossName);
        HUDManager.Instance.SetBossHealth(maxHealth, maxHealth);
    }

    protected override void Attack()
    {
        audioSource.PlayOneShot(attackSound);

        // Rotate the enemy towards the player
        FacePlayer();

        int randomIndex = Random.Range(0, 2);
        // Attack 1
        if (randomIndex == 0)
        {
            rightHand.enabled = true;
            enemyAnimator.SetTrigger("tAttack1");

            // The attack speed must match the animation length
            attackSpeed = 2.667f;
        }
        // Attack 2
        else
        {
            leftHand.enabled = true;
            enemyAnimator.SetTrigger("tAttack2");

            // The attack speed must match the animation length
            attackSpeed = 2f;
        }

        // Enable the enemy to damage the player
        CanDealDamage = true;

        // Limits the attack speed
        isAttacking = true;
        StartCoroutine(AttackCooldown());
    }

    protected override IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        isAttacking = false;
        CanDealDamage = false;
        rightHand.enabled = false;
        leftHand.enabled = false;
    }

    public override void OnHit(int damage, bool critical)
    {
        damage = MitigateDamage(damage);

        // The enemy can only be hit if he is not staggered and alive
        if (!isStaggered && currentHealth > 0)
        {
            HUDManager.Instance.CreateBossFloatingDamageText(damage, critical);
            audioSource.PlayOneShot(weaponHitSound);
            isStaggered = true;

            // Apply the damage and check if the enemy dies
            currentHealth -= damage;
            HUDManager.Instance.SetBossHealth(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                Die();
            }

            StartCoroutine(RemoveStagger());
        }
    }

    protected override void Die()
    {
        base.Die();

        // Deactivate boss fight mode
        HUDManager.Instance.SetupBossFight(false, bossName);
    }

    private int MitigateDamage(int rawDamage)
    {
        float damageMitigation = 1f - damageReduction;
        float fDamage = (float)rawDamage;
        fDamage *= damageMitigation;
        int finalDamage = Mathf.RoundToInt(fDamage);

        return finalDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destructible"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}  
