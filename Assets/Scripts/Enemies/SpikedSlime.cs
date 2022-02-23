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
        base.OnHit(MitigateDamage(damage), critical);
    }

    private int MitigateDamage(int rawDamage)
    {
        float damageMitigation = 1f - damageReduction;
        float fDamage = (float)rawDamage;
        fDamage *= damageMitigation;
        int finalDamage = Mathf.RoundToInt(fDamage);

        return finalDamage;
    }
}
