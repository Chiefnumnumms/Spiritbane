using UnityEngine;

public class KeneuFlyState : KeneuBaseState
{
    #region Public Functions
    //-------------------------------------------------------------------------
    //Public Functions

    //-------------------------------------------------------------------------
    // PlayerMoveState - Constructor For The State
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

        // Crossfade The Animations Transitioning Into State
        //stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, stateMachine.AnimationCrossFade);
        
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
