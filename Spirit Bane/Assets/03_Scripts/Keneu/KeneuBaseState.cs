//-------------------------------------------------------------------------
//  KeneuBaseState
//  Author:  Kevin Howell  
//  Date: February 14, 2023
//  Purpose:  Script To Control The Keneu Base State To Inheret

using UnityEngine;

//-------------------------------------------------------------------------
// This Abstract Class Represents The Base For Keneu States
public abstract class KeneuBaseState : State
{
    #region Enums
    //-------------------------------------------------------------------------
    // Enums

    public enum StateNames { Default, Grounded, Flying, Dead }
    public enum KeneuActions { Idle, Fly, Walk, TakeOff, Strike, Fire, Push }

    #endregion

    #region Protected Members
    //-------------------------------------------------------------------------
    // Protected Members

    protected readonly KeneuStateMachine stateMachine;

    private int health = 8;
    public float Health
    {
        get { return health; }
        set
        {
            if (health == value) return;

        }
    }

    protected GameObject[] movePoints;

    #endregion


    #region Protected Functions
    //-------------------------------------------------------------------------
    // Protected Functions

    //-------------------------------------------------------------------------
    // KeneuBaseState - Constructor For The State
    //-------------------------------------------------------------------------
    protected KeneuBaseState(KeneuStateMachine stateMachine) { this.stateMachine = stateMachine; }

    #endregion


    #region Protected Movement Functions
    //-------------------------------------------------------------------------
    // Protected Movement Functions

    //-------------------------------------------------------------------------
    // CalculateMoveDirection - Calculate The Move Direction
    //-------------------------------------------------------------------------
    protected void CalculateMoveDirection()
    {
        //float speed = 10.0f;
        //if (isSprinting) speed *= sprintAdjust;

        //Vector3 relativeMovement = ConvertToCameraSpace(stateMachine.InputReader.Move);
        //if (relativeMovement.sqrMagnitude > 1.0f) relativeMovement.Normalize();

        //Vector3 forward = new(stateMachine.Cam.transform.forward.x, 0, stateMachine.Cam.transform.forward.z);
        //Vector3 right = new(stateMachine.Cam.transform.right.x, 0, stateMachine.Cam.transform.right.z);

        //Vector3 moveDir = stateMachine.InputReader.Move.y * forward.normalized + stateMachine.InputReader.Move.x * right.normalized;

        /*
        Vector2 input = stateMachine.InputReader.Move;
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 move = new Vector3(currentInputVector.x, 0.0f, currentInputVector.y);
        */

        //Vector3 moveDir = currentInputVector.y * forward.normalized + currentInputVector.x * right.normalized;

        //if (moveDir.sqrMagnitude > 1.0f) moveDir.Normalize();

        //stateMachine.Velocity.x = relativeMovement.x * speed;
        //stateMachine.Velocity.z = relativeMovement.z * speed;

        /*
        float speed = stateMachine.MovementSpeed;

        Vector2 input = stateMachine.InputReader.Move;
        //currentPos = Vector2.SmoothDamp(currentPos, Velocity, ref smoothInputVelocity, smoothInputSpeed);

        Vector3 forward = new(stateMachine.Cam.transform.forward.x, 0, stateMachine.Cam.transform.forward.z);
        Vector3 right = new(stateMachine.Cam.transform.right.x, 0, stateMachine.Cam.transform.right.z);

        Vector3 moveDir = (currentInputVector.y * forward.normalized + currentInputVector.x * right.normalized) * speed * Time.deltaTime;
        if (moveDir.sqrMagnitude > 1.0f) moveDir.Normalize();

        moveDir.y = 0.0f;

        stateMachine.Velocity = moveDir;
        */

        //Vector3 currentMove = new Vector3(-horz, 0, vert) * Time.deltaTime;  //stateMachine.Controller.isGrounded ? 0.0f : -1.0f, vert) * Time.deltaTime;
    }

    //-------------------------------------------------------------------------
    // FaceMoveDirection - Rotate Towards Forward Direction
    //-------------------------------------------------------------------------
    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(stateMachine.Velocity.x, 0.0f, stateMachine.Velocity.z);

        if (faceDirection.z < 0.0f) return;
        if (faceDirection == Vector3.zero) return;

        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.LookRoatationDampFactor * Time.deltaTime);
    }

    /*
    //-------------------------------------------------------------------------
    // ApplyGravity - Apply Gravity To Y Velocity
    //-------------------------------------------------------------------------
    protected void ApplyGravity()
    {
        if (stateMachine.Velocity.y > Physics.gravity.y)
        {
            stateMachine.Velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    protected Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        Vector3 forward = new(stateMachine.MainCam.transform.forward.x, 0, stateMachine.MainCam.transform.forward.z);
        Vector3 right = new(stateMachine.MainCam.transform.right.x, 0, stateMachine.MainCam.transform.right.z);

        //Vector3 right = stateMachine.Cam.transform.right;
        //right.y = 0.0f;
        //right = right.normalized;

        //Vector3 forward = stateMachine.Cam.transform.forward;
        //forward.y = 0.0f;
        //forward = forward.normalized;

        Vector3 rightProduct = right.normalized * vectorToRotate.x;
        Vector3 forwardProduct = forward.normalized * vectorToRotate.z;

        Vector3 vectorRotated = rightProduct + forwardProduct;
        vectorRotated.y = vectorToRotate.y;

        return vectorRotated;
    }
    */

    #endregion



    #region Protected State Switch Functions
    //-------------------------------------------------------------------------
    // Protected State Switch Functions

    //-------------------------------------------------------------------------
    // SwitchState - Transition To Requested State
    //-------------------------------------------------------------------------
    protected void SwitchState(StateNames stateName)
    {
        if (GameManager.instance.debugLog) Debug.Log("Go To " + stateName + " State From " + stateMachine.CurrentState.ToString());

        switch (stateName)
        {
            default:
                stateMachine.SwitchState(new KeneuGroundState(stateMachine));
                break;
            case StateNames.Default:
                stateMachine.SwitchState(new KeneuGroundState(stateMachine));
                break;
            case StateNames.Grounded:
                stateMachine.SwitchState(new KeneuGroundState(stateMachine));
                break;
            case StateNames.Flying:
                stateMachine.SwitchState(new KeneuFlyState(stateMachine));
                break;
            case StateNames.Dead:
                stateMachine.SwitchState(new KeneuDeadState(stateMachine));
                break;
        }

        movePoints = null;
    }

    protected void NextPhase()
    {
        if (stateMachine.CurrentFightState == KeneuStateMachine.FightState.Finish) return;

        stateMachine.CurrentFightState += 1;
    }

    //-------------------------------------------------------------------------
    // SwitchToJumpState - Transition To Jump State
    //-------------------------------------------------------------------------
    protected void SwitchToGroundState()
    {
        if (GameManager.instance.debugLog) Debug.Log("Go To Ground State From " + stateMachine.CurrentState.ToString());

        stateMachine.SwitchState(new KeneuGroundState(stateMachine));
    }

    //-------------------------------------------------------------------------
    // SwitchToJumpState - Transition To Jump State
    //-------------------------------------------------------------------------
    protected void SwitchToFlyState()
    {
        if (GameManager.instance.debugLog) Debug.Log("Go To Fly State From " + stateMachine.CurrentState.ToString());

        stateMachine.SwitchState(new KeneuFlyState(stateMachine));
    }

    //-------------------------------------------------------------------------
    // SwitchToAimState - Transition To Aim Mode State
    //-------------------------------------------------------------------------
    protected void SwitchToDeadState()
    {
        if (GameManager.instance.debugLog) Debug.Log("Go To Dead State From " + stateMachine.CurrentState.ToString());

        stateMachine.SwitchState(new KeneuDeadState(stateMachine));
    }

    //-------------------------------------------------------------------------
    // FireAttack - 
    //-------------------------------------------------------------------------    
    protected void FireAttack()
    {

    }

    //-------------------------------------------------------------------------
    // StrikeAttack - 
    //-------------------------------------------------------------------------    
    protected void StrikeAttack()
    {

    }

    //-------------------------------------------------------------------------
    // TakeOff - 
    //-------------------------------------------------------------------------    
    protected void TakeOff()
    {

    }

    //-------------------------------------------------------------------------
    // Land - 
    //-------------------------------------------------------------------------    
    protected void Land()
    {
        
    }



    #endregion
}
