using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    protected float minDamage = 25f;
    protected float maxDamage = 40f;
    protected float knockbackForce = 50f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other game object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Hit the player
            other.gameObject.GetComponent<PlayerController>().OnHit(Damage());

            // Knockback the player
            Knockback(other.gameObject);
        }
    }

    private void Knockback(GameObject player)
    {
        // Get knockback direction and apply the force
        Vector3 knockbackDir = (player.transform.position - transform.position).normalized;
        knockbackDir.y = 0;
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
    }

    private int Damage()
    {
        float damage = Random.Range(minDamage, maxDamage);

        return Mathf.RoundToInt(damage);
    }
}
