//-------------------------------------------------------------------------
//  GameManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Overall Game Functionality

using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//-------------------------------------------------------------------------
// This Class Represents The Game Manager
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(AnimationManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(SceneManager))]
public class GameManager : Singleton<GameManager>
{
    #region Enums
    //-------------------------------------------------------------------------
    // Enums

    public enum GameState { START, LOAD, MAIN_MENU, PAUSE_MENU, OPTIONS, CREDITS, CUTSCENE, PLAYING, VICTORY, GAME_OVER }

    #endregion

    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

    // Managers
    public InputManager InputManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    public SceneManager SceneManager { get; private set; }
    public AnimationManager AnimationManager { get; private set; }
    public AudioManager AudioManager { get; private set; }

    // Cache Reference To Player Game Object
    public GameObject PlayerGO { get; private set; }

    // Keep Track Of Game State
    public GameState State { get; private set; }

    #endregion


    #region Public Events
    //-------------------------------------------------------------------------
    // Public Events

    public static event Action<GameState> OnGameStateChanged;

    #endregion


    #region Editor Access Members
    //-------------------------------------------------------------------------
    // Editor Access Members

    [Header("Debug Log Toggle")]
    [SerializeField]
    public bool debugLog = false;      // Turn Debug.Log On Or Off

    #endregion


    #region Private Members
    //-------------------------------------------------------------------------
    // Private Members


    #endregion

    #region Public Functions
    //-------------------------------------------------------------------------
    // Public Functions

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.START:
                //StartCoroutine(InitializeGame);
                State = GameState.MAIN_MENU;
                break;

            case GameState.LOAD:

                break;

            case GameState.MAIN_MENU:

                break;

            case GameState.PAUSE_MENU:

                break;

            case GameState.OPTIONS:

                break;

            case GameState.CREDITS:

                break;

            case GameState.CUTSCENE:

                break;

            case GameState.PLAYING:
                //var objects = FindObjectsOfType<Collectable>();
                //if (objects.Any(c => c.Outcome == Outcome.Winner)) UpdateGameState(GameState.VICTORY);
                //if(health <= 0.0f) UpdateGameState(GameState.GAME_OVER);

                break;

            case GameState.VICTORY:

                break;

            case GameState.GAME_OVER:

                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    #endregion

    #region Protected Functions
    //-------------------------------------------------------------------------
    // Protected Functions

    //-------------------------------------------------------------------------
    // Initialize - Initialize Members And Get Ready
    //-------------------------------------------------------------------------
    protected override void Initialize()
    {
        /*
        // Initialize Things Here For Call In base.Awake()
       inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        if(debugLog)
        {
            if (inputManager != null)
                Debug.Log("Input Manager Cached");
            else
                Debug.Log("Input Manager Not Cached");
        }

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        if (debugLog)
        {
            if (playerManager != null)
                Debug.Log("Player Manager Cached");
            else
                Debug.Log("Player Manager Not Cached");
        }

        animationManager = GameObject.Find("AnimationManager").GetComponent<AnimationManager>();
        if (debugLog)
        {
            if (animationManager != null)
                Debug.Log("Animation Manager Cached");
            else
                Debug.Log("Animation Manager Not Cached");
        }

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (debugLog)
        {
            if (audioManager != null)
                Debug.Log("Audio Manager Cached");
            else
                Debug.Log("Audio Manager Not Cached");
        }

        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        if(debugLog)
        {
            if (PlayerGO != null)
                Debug.Log("PlayerGO Cached");
            else
                Debug.Log("PlayerGO Not Cached");
        }
        */

        // LOCK CURSOR
        Cursor.lockState = CursorLockMode.Locked;

        UpdateGameState(GameState.START);
    }

    #endregion


    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions


    #endregion

}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }
}

 */ 