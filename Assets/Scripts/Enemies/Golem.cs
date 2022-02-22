using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyBase
{
    [SerializeField] Collider rightHand;
    [SerializeField] Collider leftHand;

    private void Awake()
    {
        attackRange = 5f;
        moveSpeed = 5f;
        maxHealth = 100f;
        minDamage = 20f;
        maxDamage = 35f;
        staggerTime = 0.3f;
    }

    protected override void Attack()
    {
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
}  
