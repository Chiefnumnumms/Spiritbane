using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> particleList = new List<GameObject>();

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float activateDistance;

    private void Awake()
    {
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        foreach (var particle in particleList)
        {
            if(Vector3.Distance(particle.transform.position,playerTransform.position) <= activateDistance)
            {
                EnableParticals(particle);
            }
            else if(Vector3.Distance(particle.transform.position,playerTransform.position) > activateDistance )
            {
                DisableParticals(particle);
            }
        }
    }

    //---------------------------------------------------------------
    //Function to enable partical systems when player is within range
    private void EnableParticals(GameObject obj)
    {
        obj.SetActive(true);
    }

    //---------------------------------------------------------------
    //Function to disable particals when player is out of range
    private void DisableParticals(GameObject obj)
    {
        obj.SetActive(false);
    }
}
