using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedSlime : EnemyBase
{
    private float damageReduction = 0.4f;

    private void Awake()
    {
        attackRange = 1.7f;
        moveSpeed = 5f;
        attackSpeed = 1.4f;
        maxHealth = 120f;
        minDamage = 10f;
        maxDamage = 16f;
        staggerTime = 0.25f;
    }

    public override void OnHit(int damage, bool critical)
    {
        float damageMitigation = 1f - 0.4f;
        float fDamage = (float)damage;
        fDamage *= damageMitigation;
        damage = Mathf.RoundToInt(fDamage);

        base.OnHit(damage, critical);
    }
}
