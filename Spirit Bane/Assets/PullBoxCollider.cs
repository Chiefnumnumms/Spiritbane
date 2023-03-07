using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBoxCollider : MonoBehaviour
{
    [SerializeField] private GameObject pullDisc;
    [SerializeField] private float rotationSpeed = 25.0f;

    private bool spinObject = false;

    private void Update()
    {
        if (spinObject)
        {
            pullDisc.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BladeTip")
        {
            Debug.Log("Spinning Object");
            
            spinObject = true;
        }
    }
}
