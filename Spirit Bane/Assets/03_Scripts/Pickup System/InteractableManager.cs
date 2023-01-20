using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public float pickupRadius = 1.0f;
    public string interactableText;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        // CALLED WHEN PLAYER INTERACTS/PICKSUP AN ITEM
        Debug.Log("Player Has Interacted With An Item");
    }

}
