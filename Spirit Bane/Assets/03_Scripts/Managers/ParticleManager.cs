using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static List<GameObject> particleList = new List<GameObject>();
    

    [SerializeField]
    private static Transform playerTransform;

    [SerializeField]
    private float activateDistance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        /*
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        */
        if(particleList.Count == 0)
        {
            foreach (GameObject particle in GameObject.FindGameObjectsWithTag("Particle"))
            {
                particleList.Add(particle);
            }
        }
    }

    public void GatherParticles()
    {
        foreach (GameObject particle in GameObject.FindGameObjectsWithTag("Particle"))
        {
            particleList.Add(particle);
        }
    }

    public void ClearParticleList()
    {
        foreach( GameObject particle in particleList)
        {
            particleList.Remove(particle);
        }
    }

    private void Update()
    {
        if(particleList.Count != 0)
        {
            foreach (var particle in particleList)
            {
                if (Vector3.Distance(particle.transform.position, playerTransform.position) <= activateDistance)
                {
                    EnableParticals(particle);
                }
                else if (Vector3.Distance(particle.transform.position, playerTransform.position) > activateDistance)
                {
                    DisableParticals(particle);
                }
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
