using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LockedDoors : MonoBehaviour
{
    [SerializeField]
    private List<string> keyNames;

    [SerializeField]
    private List<string> interactableNames;

    private bool hasKey = false;
    private bool hasInteracted = false;

    private bool doorOpened = false;

    public bool cavernDoor;


    public Ray ray;
    public RaycastHit hit;
    public float rayLength = 20f;
    public Transform raycastOrigin;

    [Header("Door Components")]
    [SerializeField]
    private GameObject leftDoor;
    [SerializeField]
    private GameObject rightDoor;
    [SerializeField]
    private GameObject doorLock;
    [SerializeField]
    private GameObject doorChain1;
    [SerializeField]
    private GameObject doorChain2;

    [SerializeField]
    private Animator leftDoorAnim;
    [SerializeField]
    private Animator rightDoorAnim;
    

    [Header("Players Inventory")]
    [SerializeField]
    private ItemPickup playersItems;

    [Header("Interacted Items")]
    [SerializeField]
    private InteractionHandler interactedItems;


    private void Update()
    {
        if(keyNames.Count != 0)
        {
            if (!hasKey)
            {
                CheckInvForKey();
            }
        }

        if(interactableNames.Count != 0)
        {
            if(!hasInteracted)
            {
                CheckForInteraction();
            }
        }

        HandleDoorOpening();
        HandleCavernEnterance();

    }

    private void CheckInvForKey()
    {
        if(playersItems.items.Count != 0)
        {
            foreach (GameObject items in playersItems.items)
            {
                GameObject temp = playersItems.items.Find(x => x.name == items.name);

                if (temp != null)
                {
                    hasKey = true;
                    Debug.Log("It Fucking Works");
                }
            }
            
        }
    }

    private void CheckForInteraction()
    {
        
        if(interactedItems.interactableObjects.Count != 0)
        {
            foreach(GameObject interactables in interactedItems.interactableObjects)
            {
                GameObject temp = interactedItems.interactableObjects.Find(x => x.name == interactables.name);

                if (temp != null)
            {
                hasInteracted = true;
                Debug.Log("It Fucking Works");
            }
            }
        }
    }

    private void HandleDoorOpening()
    {
        ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider == this.gameObject.GetComponent<BoxCollider>() && !doorOpened)
            {
                if (Input.GetKeyDown(KeyCode.E) && hasKey)
                {
                    Debug.Log("Oh Shit Is That A Key");
                    this.gameObject.GetComponent<BoxCollider>().enabled = false;

                    doorLock.SetActive(false);
                    doorChain1.SetActive(false);
                    doorChain2.SetActive(false);

                    leftDoorAnim.Play("DoorSwingLeft", 0, 0f);
                    rightDoorAnim.Play("DoorSwingRight", 0, 0f);
                    doorOpened = true;
                }
                else if(Input.GetKeyDown(KeyCode.E) && hasInteracted)
                {
                    Debug.Log("Oh Shit Is That A Key");
                    this.gameObject.GetComponent<BoxCollider>().enabled = false;

                    doorLock.SetActive(false);
                    doorChain1.SetActive(false);
                    doorChain2.SetActive(false);

                    leftDoorAnim.Play("DoorSwingLeft", 0, 0f);
                    rightDoorAnim.Play("DoorSwingRight", 0, 0f);
                    doorOpened = true;
                }
            }
        }

    }

    private void HandleCavernEnterance()
    {

    }


}
