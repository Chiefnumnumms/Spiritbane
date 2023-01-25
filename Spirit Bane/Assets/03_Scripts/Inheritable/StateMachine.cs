//-------------------------------------------------------------------------
//  StateMachine
//  Author:  Kevin Howell  
//  Date: December 31, 2022
//  Purpose:  Script To Be The Base For A State Machine To Inheret

using UnityEngine;

//-------------------------------------------------------------------------
// This Abstract Class Represents The Base For A State Machine
public abstract class StateMachine : MonoBehaviour
{
    #region Protected Members
    //-------------------------------------------------------------------------
    // Protected Members

    protected State currentState;     // Hold The Current State

    #endregion


    #region Public Functions
    //-------------------------------------------------------------------------
    // Public Functions

    //-------------------------------------------------------------------------
    // SwitchState - Handle The Functionality Of Switching State
    //-------------------------------------------------------------------------
    public void SwitchState(State state)
    {
        // If Current State Isn't Null, Exit State
        currentState?.Exit();

        // Set Current State To Input State
        currentState = state;

        // Trigger Enter Functionality Of New Current State
        currentState.Enter();
    }

    #endregion


    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    //-------------------------------------------------------------------------
    // Update - Called Once Per Frame
    //-------------------------------------------------------------------------
    void Update()
    {
        // Use Tick Function In State To Update
        currentState?.Tick();
    }

    #endregion
}