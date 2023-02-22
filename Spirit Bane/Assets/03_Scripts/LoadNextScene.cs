using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private ScenesManager scenesManager;

    private void Update()
    {
        if(scenesManager == null)
        {
            scenesManager = GameObject.Find("--Managers--").GetComponent<ScenesManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            scenesManager.LoadNextScene();
        }
    }
}
