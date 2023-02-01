using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCharacter : MonoBehaviour
{
    public int damage = 25;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);           
        }

        if (enemyStats != null)
        {
            enemyStats.TakeDamage(damage);
        }
    }
}
