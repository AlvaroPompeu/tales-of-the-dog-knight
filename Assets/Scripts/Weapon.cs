using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected float minDamage = 20f;
    protected float maxDamage = 35f;
    protected float critRate = 20f;
    protected float critDamage = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other game object is an enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            bool critCheck = CriticalHit();

            // Hit the enemy
            other.gameObject.GetComponentInParent<EnemyBase>().OnHit(Damage(critCheck), critCheck);
        }
    }

    private bool CriticalHit()
    {
        float critChance = Random.Range(0, 100f);

        if (critChance <= critRate)
        {
            return true;
        }

        return false;
    }

    private int Damage(bool critical)
    {
        float damage = Random.Range(minDamage, maxDamage);

        if (critical)
        {
            damage *= critDamage;
        }

        return Mathf.RoundToInt(damage);
    }
}
