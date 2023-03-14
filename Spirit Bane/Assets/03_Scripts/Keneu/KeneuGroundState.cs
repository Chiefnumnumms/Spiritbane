using UnityEngine;

public class KeneuGroundState : KeneuBaseState
{
    #region Public Functions
    //-------------------------------------------------------------------------
    //Public Functions

    //-------------------------------------------------------------------------
    // KeneuGroundState - Constructor For The State
    //-------------------------------------------------------------------------
    public KeneuGroundState(KeneuStateMachine stateMachine) : base(stateMachine) { }

    //-------------------------------------------------------------------------
    // Enter - Handle The Functionality When Entering State
    //-------------------------------------------------------------------------
    public override void Enter()
    {
        //if (GameManager.instance.debugLog) Debug.Log("Entered Ground State");

        ////////////////////////////////////////
        // Subscribe To Relevant Events

        

        // Find All Places To Move To
        movePoints = null;

        //movePoints = GameObject.FindGameObjectsWithTag("GroundPoints");

        // Crossfade The Animations Transitioning Into State
        // stateMachine.Animator.CrossFadeInFixedTime(LandHash, stateMachine.AnimationCrossFade);
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
