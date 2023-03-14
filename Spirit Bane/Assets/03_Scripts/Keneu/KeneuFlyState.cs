using UnityEngine;

public class KeneuFlyState : KeneuBaseState
{
    #region Public Functions
    //-------------------------------------------------------------------------
    //Public Functions

    //-------------------------------------------------------------------------
    // KeneuFlyState - Constructor For The State
    //-------------------------------------------------------------------------
    public KeneuFlyState(KeneuStateMachine stateMachine) : base(stateMachine) { }

    //-------------------------------------------------------------------------
    // Enter - Handle The Functionality When Entering State
    //-------------------------------------------------------------------------
    public override void Enter()
    {
        if (GameManager.instance.debugLog) Debug.Log("Entered Flying State");

        ////////////////////////////////////////
        // Subscribe To Relevant Events

        // Find All Places To Move To
        movePoints = null;

        // Crossfade The Animations Transitioning Into State
        //stateMachine.Animator.CrossFadeInFixedTime(TakeOffHash, stateMachine.AnimationCrossFade);

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
        movePoints = null;

        ////////////////////////////////////////
        // Unsubscribe From Relevant Events
    }

    #endregion

}
