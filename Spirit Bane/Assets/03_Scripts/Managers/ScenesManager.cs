//-------------------------------------------------------------------------
//  ScenesManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Scene Flow

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static GameManager;

//-------------------------------------------------------------------------
// This Class Represents The Scenes Manager
public class ScenesManager : Singleton<ScenesManager>
{
    #region Enums
    //-------------------------------------------------------------------------
    // Enums

    public enum Scenes { MainMenu, StartingVillage, CrystalCavernsWhitebox, FloatingRocks, ArenaWhitebox, CoreMechanicDemo }

    #endregion

    #region Public Events
    //-------------------------------------------------------------------------
    // Public Events

    public static event Action<Scenes> OnSceneChanged;
    //public static event Action<Scenes> OnDesiredSceneChanged;

    #endregion

    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

    //public Scenes CurrentScene { get; private set; }
    public Observable<Scenes> CurrentScene = new Observable<Scenes>();

    public Scenes DesiredScene
    {
        get
        {
            return desiredScene;
        }
        set
        {
            DesiredSceneChanged(desiredScene, value);
            desiredScene = value;
        }
    }

    private void DesiredSceneChanged(Scenes oldValue, Scenes newValue)
    {
        //whatever
    }


    #endregion

    #region Editor Access Members
    //-------------------------------------------------------------------------
    // Editor Access Members

    [Header("Desired Scene To Load")]
    [SerializeField] private Scenes desiredScene = Scenes.StartingVillage;

    [Header("Debug Log Toggle")]
    [SerializeField]
    public bool debugLog = false;      // Turn Debug.Log On Or Off

    #endregion


    #region Private Members
    //-------------------------------------------------------------------------
    // Private Members

    //private Observable<Scenes> scene = new Observable<Scenes>();

    #endregion


    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    public void LoadNewGame()
    {
        UpdateScene(Scenes.StartingVillage);
        //SceneManager.LoadScene(Scenes.StartingVillage.ToString());
    }

    public void LoadMainMenu()
    {
        UpdateScene(Scenes.MainMenu);
        //SceneManager.LoadScene(Scenes.MainMenu.ToString());
    }

    public void LoadScene(Scene scene)
    {
        UpdateScene((Scenes)scene.buildIndex);
        //SceneManager.LoadScene(scene.ToString());
    }

    public void LoadLastScene()
    {
        UpdateScene((Scenes)SceneManager.GetActiveScene().buildIndex - 1);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadNextScene()
    {
        UpdateScene((Scenes)SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UpdateScene(Scenes newScene)
    {
        CurrentScene.Value = newScene;

        switch (newScene)
        {
            case Scenes.MainMenu:
                SceneManager.LoadScene(Scenes.MainMenu.ToString());
                break;
            case Scenes.StartingVillage:
                SceneManager.LoadScene(Scenes.StartingVillage.ToString());
                break;
            case Scenes.CrystalCavernsWhitebox:
                SceneManager.LoadScene(Scenes.CrystalCavernsWhitebox.ToString());
                break;
            case Scenes.FloatingRocks:
                SceneManager.LoadScene(Scenes.FloatingRocks.ToString());
                break;
            case Scenes.ArenaWhitebox:
                SceneManager.LoadScene(Scenes.ArenaWhitebox.ToString());
                break;
            case Scenes.CoreMechanicDemo:
                SceneManager.LoadScene(Scenes.CoreMechanicDemo.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newScene), newScene, null);
        }

        OnSceneChanged?.Invoke(newScene);
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
        UpdateScene(DesiredScene);
        CurrentScene.Changed += OnValueChanged;


    }

    protected void OnDestroy()
    {
        CurrentScene.Changed -= OnValueChanged;
    }

    protected void OnValueChanged(object target, Observable<Scenes>.ChangedEventArgs args)
    {
        if (GameManager.instance.debugLog)
        {
            Debug.Log("Old Value Was: " + args.OldValue);
            Debug.Log("New Value Was: " + args.NewValue);
        }
    }

    #endregion
}
