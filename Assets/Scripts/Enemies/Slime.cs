using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyBase
{
    private void Awake()
    {
        attackRange = 2f;
        moveSpeed = 8f;
        attackSpeed = 1f;
        maxHealth = 100f;
        minDamage = 8f;
        maxDamage = 15f;
        staggerTime = 0.3f;
    }
}
