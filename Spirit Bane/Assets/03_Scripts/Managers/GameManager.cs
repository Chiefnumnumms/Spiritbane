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
[RequireComponent(typeof(ScenesManager))]
[RequireComponent(typeof(AudioManager))]
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
        //Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(InitializeGame());
                
        //base.Initialize();
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
        ScenesManager.instance.LoadNewGame();
    }

    private void GoToMainMenu()
    {
        ScenesManager.instance.LoadMainMenu();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.anyKeyDown) SwitchScenesByKey();
    }

    private void SwitchScenesByKey()
    {
        if (Input.inputString != "")
        {
            int index;
            bool isNum = Int32.TryParse(Input.inputString, out index);

            if (isNum && index >= 0 && index < Enum.GetNames(typeof(ScenesManager.Scenes)).Length)
            {
                ScenesManager.instance.DesiredScene = (ScenesManager.Scenes)index;
            }
        }
    }

    #endregion

}