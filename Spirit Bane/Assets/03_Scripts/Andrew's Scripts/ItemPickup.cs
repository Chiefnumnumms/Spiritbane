using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemCount;
    public List<GameObject> items = new List<GameObject>();

    public Transform raycastOrigin;

    public Ray ray;
    public RaycastHit hit;
    public float rayLength = 3f;

    // LIST OF KEYS
    private enum KeyList { keyOne, keyTwo, keyThree, keyFour}

    [Header("Camera Shake Settings")]
    public CameraShake shakeCamera;

    void Update()
    {
        HandleItemPickup();
    }

    public void HandleItemPickup()
    {
        ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                itemText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    items.Add(hit.collider.gameObject);
                    itemCount.text = "ITEMS [" + items.Count + "]";
                    hit.collider.gameObject.SetActive(false);

                    itemText.gameObject.SetActive(false);
                    shakeCamera.ScreenShake(ray.direction);
                }
                else
                {
                    itemText.gameObject.SetActive(true);
                }
            }
            else
            {
                itemText.gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + raycastOrigin.forward * rayLength);
    }
}
