//-------------------------------------------------------------------------
//  Singleton Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script For Inheritance Of Singleton Functionality

using UnityEngine;

//-------------------------------------------------------------------------
// This Abstract Class Represents An Inheretted Singleton Base Functionality
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members
    public static T instance { get; private set; }      // Instance Of The Singlton

    #endregion


    #region Protected Virtual Functions
    //-------------------------------------------------------------------------
    // Protected Virtual Functions

    //-------------------------------------------------------------------------
    // Awake - Fires When Script Is Called Upon
    //-------------------------------------------------------------------------
    protected virtual void Awake()
    {
        // Self Destruct If Instance Is Not Null As One Already Exists And Return
        if (instance != null)
        {
            Debug.Log("Singleton Already Exists.  Self-Destruct.");
            Destroy(this);
            return;
        }

        // Declare Self As New Singleton
        instance = this as T;

        // Make Sure It Stays Between Loads
        DontDestroyOnLoad(gameObject);

        // Initialize Singleton
        Initialize();
    }

    //-------------------------------------------------------------------------
    // Initialize - Hold Functionality For When Initializing
    //-------------------------------------------------------------------------
    protected virtual void Initialize() { }

    #endregion


    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    //-------------------------------------------------------------------------
    // OnDestroy - When Singleton Is Destroyed Make Sure To Clean Up
    //-------------------------------------------------------------------------
    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    #endregion

}
