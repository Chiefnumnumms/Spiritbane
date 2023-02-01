//-------------------------------------------------------------------------
//  LevelManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Level Flow

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//-------------------------------------------------------------------------
// This Class Represents The Scene Manager
public class LevelManager : Singleton<LevelManager>
{
    /*
    [SerializeField]
    private AudioClip levelMusic;
    public AudioClip LevelMusic { get { return instance.levelMusic; } }
    [SerializeField]
    private AudioClip altMusic;
    public AudioClip AltMusic { get { return instance.altMusic; } }

    [SerializeField]
    private GameObject playerSpawnPoint;
    public GameObject PlayerSpawnPoint { get { return playerSpawnPoint; } set { playerSpawnPoint = value; } }

    private void Awake()
    {
        if (playerSpawnPoint == null)
        {
            playerSpawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !GameManager.Instance.isLoading)
        {
            GameManager.Instance.LevelComplete();

            //other.gameObject.transform.position = playerSpawnPoint.transform.position;
        }
    }
    */
}
