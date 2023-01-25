//-------------------------------------------------------------------------
//  State
//  Author:  Kevin Howell  
//  Date: December 31, 2022
//  Purpose:  Script To Be The Base State To Inheret

//-------------------------------------------------------------------------
// This Abstract Class Represents The Base For States
public abstract class State
{
    #region Public Abstract Functions
    //-------------------------------------------------------------------------
    // Public Abstract Functions

    //-------------------------------------------------------------------------
    // Enter - Handle The Functionality When Entering State
    //-------------------------------------------------------------------------
    public abstract void Enter();

    //-------------------------------------------------------------------------
    // Tick - Handle The Functionality During State
    //-------------------------------------------------------------------------
    public abstract void Tick();

    //-------------------------------------------------------------------------
    // Exit - Handle The Functionality When Exiting State
    //-------------------------------------------------------------------------
    public abstract void Exit();

    #endregion
}