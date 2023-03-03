//-------------------------------------------------------------------------
//  ScenesManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Scene Flow

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//-------------------------------------------------------------------------
// This Class Represents The Scenes Manager
public class ScenesManager : Singleton<ScenesManager>
{
    #region Enums
    //-------------------------------------------------------------------------
    // Enums

    public enum Scenes { MainMenu, StartingVillage, CrystalCavernsWhitebox, FloatingIslands, ArenaWhitebox, CoreMechanicDemo, SwordDemo }

    #endregion

    #region Public Events
    //-------------------------------------------------------------------------
    // Public Events

    public static event Action<Scenes> OnSceneChanged;

    #endregion

    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

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
        // Do Whatever Needed Before Updating Scene

        
        UpdateScene(newValue);
    }


    #endregion

    #region Editor Access Members
    //-------------------------------------------------------------------------
    // Editor Access Members

    [Header("Desired Scene To Load")]
    [SerializeField] private Scenes desiredScene = Scenes.StartingVillage;

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
    }

    public void LoadMainMenu()
    {
        UpdateScene(Scenes.MainMenu);
    }

    public void LoadCrystalCavern()
    {
        UpdateScene(Scenes.CrystalCavernsWhitebox);
    }

    public void LoadFloatingRocks()
    {
        UpdateScene(Scenes.FloatingIslands);
    }
    public void LoadArena()
    {
        UpdateScene(Scenes.ArenaWhitebox);
    }
    public void LoadCoreMechanicDemo()
    {
        UpdateScene(Scenes.CoreMechanicDemo);
    }

    public void LoadSwordDemo()
    {
        UpdateScene(Scenes.SwordDemo);
    }

    public void LoadDesiredScene()
    {
        UpdateScene(DesiredScene);
    }

    public void LoadScene(Scene scene)
    {
        UpdateScene((Scenes)scene.buildIndex);
    }

    public void LoadLastScene()
    {
        UpdateScene((Scenes)SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadNextScene()
    {
        UpdateScene((Scenes)SceneManager.GetActiveScene().buildIndex + 1);
    }

    //-------------------------------------------------------------------------
    // UpdateScene - Handle The Switching Of Scene, Updating CurrentScene
    //-------------------------------------------------------------------------
    private void UpdateScene(Scenes newScene)
    {
        CurrentScene.Value = newScene;

        //AudioManager.instance.StartAreaMusic();
    
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
            case Scenes.FloatingIslands:
                SceneManager.LoadScene(Scenes.FloatingIslands.ToString());
                break;
            case Scenes.ArenaWhitebox:
                SceneManager.LoadScene(Scenes.ArenaWhitebox.ToString());
                break;
            case Scenes.CoreMechanicDemo:
                SceneManager.LoadScene(Scenes.CoreMechanicDemo.ToString());
                break;
            case Scenes.SwordDemo:
                SceneManager.LoadScene(Scenes.SwordDemo.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newScene), newScene, null);
        }

        // If OnSceneChanged Subscribers Is Not Null, Invoke Anything Subscribed
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

        // Subscribe To Event For Observable Class
        CurrentScene.Changed += OnValueChanged;
    }

    //-------------------------------------------------------------------------
    // OnDestroy - Housekeeping By Unsubscribing To Event For Observable Class
    //-------------------------------------------------------------------------
    protected void OnDestroy()
    {
        CurrentScene.Changed -= OnValueChanged;
    }

    //-------------------------------------------------------------------------
    // OnValueChanged - Triggered Upon Enum Scenes Variable (CurrentScene) Change
    //-------------------------------------------------------------------------
    protected void OnValueChanged(object target, Observable<Scenes>.ChangedEventArgs args)
    {
        if (GameManager.instance.debugLog)
        {
            Debug.Log("Old Value Was: " + args.OldValue);
            Debug.Log("New Value Was: " + args.NewValue);
        }

        // Anything Needed To Do When Switching CurrentScene
    }

    #endregion
}
