using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectHazard : MonoBehaviour
{
    // STEP 03: POLISH >> LERP THE CRYSTALS 
    // STEP 04: RANDOMIZE THE CRYSTALS FALLING (MAKE SURE THEY ALL FALL AT RANDOM TIMES)

    public List<GameObject> hazardObjects = new List<GameObject>();
    public List<Rigidbody> hazardRigidBody = new List<Rigidbody>();

    private void Awake()
    {
        FindAllHazards();
    }

    private void OnTriggerEnter(Collider other)
    {
        // STEP 01: CHECK FOR PLAYER GOING THROUGH COLLIDER
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Found Dropping Load!");
            StartCoroutine(DropAllHazards(2.0f));
        }
    }


    public void FindAllHazards()
    {
        GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");

        foreach (GameObject h in hazards)
        {
            if (!hazardObjects.Contains(h))
            {
                // ADD ALL OBJECTS
                hazardObjects.Add(h);
            }

            // ADD ALL THE CRYSTAL RIGIDBODY
            hazardRigidBody.Add(h.GetComponent<Rigidbody>());

        }

    }

    public IEnumerator DropAllHazards(float duration)
    {
        foreach (Rigidbody rb in hazardRigidBody)
        {
            // ENABLE GRAVITY ON FALLING
            rb.isKinematic = false;
            rb.useGravity = true;

            // DELAY BETWEEN THE CRYSTALS FALLING
            yield return new WaitForSeconds(duration);

            // ENABLE KINEMATIC WHEN IT HITS THE GROUND
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

}
