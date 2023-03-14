using UnityEngine;

public class KeneuDeadState : KeneuBaseState
{
    #region Public Functions
    //-------------------------------------------------------------------------
    //Public Functions

    //-------------------------------------------------------------------------
    // KeneuDeadState - Constructor For The State
    //-------------------------------------------------------------------------
    public KeneuDeadState(KeneuStateMachine stateMachine) : base(stateMachine) { }

    //-------------------------------------------------------------------------
    // Enter - Handle The Functionality When Entering State
    //-------------------------------------------------------------------------
    public override void Enter()
    {
        if (GameManager.instance.debugLog) Debug.Log("Entered Dead State");

        ////////////////////////////////////////
        // Subscribe To Relevant Events

    }

    //-------------------------------------------------------------------------
    // Tick - Handle The Functionality During State
    //-------------------------------------------------------------------------
    public override void Tick()
    {

    }

    //-------------------------------------------------------------------------
    // Exit - Handle The Functionality When Exiting State
    //-------------------------------------------------------------------------
    public override void Exit()
    {
        ////////////////////////////////////////
        // Unsubscribe From Relevant Events

    }

    #endregion
}
