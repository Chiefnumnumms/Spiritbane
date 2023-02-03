using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public List<GameObject> interactableObjects = new List<GameObject>();

    public Transform raycastOrigin;

    public Ray ray;
    public RaycastHit hit;
    public float rayLength = 3f;

    void Update()
    {
        HandleInteraction();
    }

    public void HandleInteraction()
    {
        ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactableObjects.Add(hit.collider.gameObject);
                    Debug.Log("Hit Boyo");
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + raycastOrigin.forward * rayLength);
    }
}
