using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardCollision : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private int hazardDamage = 25;

    private void Awake()
    {
        playerStats = GameObject.Find("Gaoh").GetComponent<PlayerStats>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStats.TakeDamage(hazardDamage);
        }
    }
}
