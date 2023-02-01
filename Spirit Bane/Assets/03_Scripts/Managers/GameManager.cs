//-------------------------------------------------------------------------
//  GameManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Overall Game Functionality

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//-------------------------------------------------------------------------
// This Class Represents The Game Manager
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(ScenesManager))]
[RequireComponent(typeof(AudioManager))]
public class GameManager : Singleton<GameManager>
{
    #region Enums
    //-------------------------------------------------------------------------
    // Enums

    public enum GameState { START, LOAD, MAIN_MENU, PAUSE_MENU, OPTIONS, CREDITS, CUTSCENE, PLAYING, VICTORY, GAME_OVER }
    public enum Scenes { MainMenu, StartingVillage, CrystalCavernsWhitebox, FloatingRocks, ArenaWhitebox, CoreMechanicDemo }

    #endregion

    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

    // Managers
    public InputManager InputManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    public ScenesManager ScenesManager { get; private set; }
    public AudioManager AudioManager { get; private set; }

    // Cache Reference To Player Game Object
    public GameObject PlayerGO { get; private set; }

    // Cache Reference To Camera
    public Camera Cam { get; private set; }

    // Keep Track Of Game State
    public GameState CurrentState { get; private set; }

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
        CurrentState = newState;

        switch (newState)
        {
            case GameState.START:
                //StartCoroutine(InitializeGame);

                // Skip To Main Menu For Now
                CurrentState = GameState.MAIN_MENU;
                break;

            case GameState.LOAD:

                break;

            case GameState.MAIN_MENU:
                // Skip To Playing For Now
                CurrentState = GameState.PLAYING;
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
        // Initialize Things Here For Call In base.Awake()
       InputManager = GetComponent<InputManager>();
        if(debugLog)
        {
            if (InputManager != null) Debug.Log("Input Manager Cached");
            else Debug.Log("Input Manager Not Cached");
        }

        PlayerManager = GetComponent<PlayerManager>();
        if (debugLog)
        {
            if (PlayerManager != null) Debug.Log("Player Manager Cached");
            else Debug.Log("Player Manager Not Cached");
        }

        ScenesManager = GetComponent<ScenesManager>();
        if (debugLog)
        {
            if (ScenesManager != null) Debug.Log("Scenes Manager Cached");
            else Debug.Log("Scenes Manager Not Cached");
        }

        AudioManager = GetComponent<AudioManager>();
        if (debugLog)
        {
            if (AudioManager != null) Debug.Log("Audio Manager Cached");
            else Debug.Log("Audio Manager Not Cached");
        }

        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        if(debugLog)
        {
            if (PlayerGO != null) Debug.Log("PlayerGO Cached");
            else Debug.Log("PlayerGO Not Cached");
        }

        Cam = Camera.main;

        // LOCK CURSOR
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(InitializeGame());
                
        //base.Initialize();
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    #endregion


    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    private IEnumerator InitializeGame()
    {
        // Show Company Splash Screen 
        //yield return new WaitForSeconds(0.5f);

        // Show Game Title Card
        //yield return new WaitForSeconds(0.5f);

        // Show Main Menu
        UpdateGameState(GameState.START);
        yield return null;
    }

    private void StartGame()
    {
        //DesiredScene = Scenes.StartingVillage;
        //StartCoroutine("LoadNextScene", DesiredScene);
    }

    private IEnumerator LoadNextScene()
    {


        yield return null;
    }
    /*

    private void DebugWarp()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            LoadScene(0);
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            LoadScene(1);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            LoadScene(2);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            LoadScene(3);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            LoadScene(4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DebugWarp();
    }
    */

    private void Exit()
    {
        Application.Quit();
    }

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






    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    private void DebugWarp()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            LoadScene(0);
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            LoadScene(1);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            LoadScene(2);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            LoadScene(3);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            LoadScene(4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DebugWarp();
    }

 */ 