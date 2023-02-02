using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LockedDoors : MonoBehaviour
{
    [SerializeField]
    private string keyName;

    private bool hasKey = false;

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


    private void Update()
    {
        if (!hasKey)
        {
            CheckInvForKey();
        }
        HandleDoorOpening();


    }

    private void CheckInvForKey()
    {
        GameObject temp = playersItems.items.Find(x => x.name == keyName);

        if (temp != null)
        {
            hasKey = true;
            Debug.Log("It Fucking Works");
        }
    }

    private void HandleDoorOpening()
    {
        ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Door"))
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
                }
            }
        }
    }

    
}
