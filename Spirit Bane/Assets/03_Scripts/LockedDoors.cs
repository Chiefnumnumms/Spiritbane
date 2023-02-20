using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LockedDoors : MonoBehaviour
{
    [SerializeField]
    private string keyName;

    [SerializeField]
    private string interactableName;

    private bool hasKey = false;
    private bool hasInteracted = false;

    private bool doorOpened = false;


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
        if(keyName != "")
        {
            if (!hasKey)
            {
                CheckInvForKey();
            }
        }

        if(interactableName != "")
        {
            if(!hasInteracted)
            {
                CheckForInteraction();
            }
        }

        HandleDoorOpening();


    }

    private void CheckInvForKey()
    {
        if(playersItems.items.Count != 0)
        {
            GameObject temp = playersItems.items.Find(x => x.name == keyName);

            if (temp != null)
            {
                hasKey = true;
                Debug.Log("It Fucking Works");
            }
        }
    }

    private void CheckForInteraction()
    {
        
        if(interactedItems.interactableObjects.Count != 0)
        {
            GameObject temp = interactedItems.interactableObjects.Find(x => x.name == interactableName);

            if (temp != null)
            {
                hasInteracted = true;
                Debug.Log("It Fucking Works");
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

    
}
