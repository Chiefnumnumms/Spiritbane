using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : InteractableManager
{
    public GameObject pickupItem;
    public LayerMask pickupLayer;

    // LIST TO STORE ALL ITEMS VIA UI
    public List<GameObject> itemList = new List<GameObject>();
        
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        // PICKUP THE ITEM AND ADD IT TO THE PLAYERS UI
        PickUpItem(playerManager);

        // ADD TO THE UI (COUNT?)

    }


    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerLocomotion playerLocomotion;
        AnimationManager animationManager;

        playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        animationManager = playerManager.GetComponent<AnimationManager>();

        // PLAYERS MOVEMENT STOPS FULLY WHEN PICKING UP THE ITEM
        playerLocomotion.playerRb.velocity = Vector3.zero;

        // PLAY THE PICKUP ANIMATION
        //animationManager.PlayTargetAnim("PickUpItem", true);

        // IF THE ITEM DOESNT EXIST ALREADY
        if (!itemList.Contains(pickupItem))
        {
            // ADD THE ITEM TO THE LIST
            itemList.Add(pickupItem);

            // DESTROY THE ITEM
            Destroy(gameObject);

            // DISPLAY IT ON THE UI

        }
        else 
        {
            Debug.Log("Item Already Exists In Inventory!");
        }


    }
}
