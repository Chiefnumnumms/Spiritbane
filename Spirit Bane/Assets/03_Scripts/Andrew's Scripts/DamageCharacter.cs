using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCharacter : MonoBehaviour
{
    public int damage = 25;

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager characterManager = other.GetComponent<CharacterManager>();

        if (characterManager != null)
        {
            characterManager.TakeDamage(damage);
           
        }
    }
}
