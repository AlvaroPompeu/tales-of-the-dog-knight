using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected float attackDamage = 40f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other game object is an enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Hit the enemy
            other.gameObject.GetComponentInParent<Enemy>().OnHit(attackDamage);
        }
    }
}
