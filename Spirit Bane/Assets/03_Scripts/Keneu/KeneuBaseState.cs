//-------------------------------------------------------------------------
//  KeneuBaseState
//  Author:  Kevin Howell  
//  Date: February 14, 2023
//  Purpose:  Script To Control The Keneu Base State To Inheret

using UnityEngine;
using static UnityEditorInternal.ReorderableList;

//-------------------------------------------------------------------------
// This Abstract Class Represents The Base For Player States
public abstract class KeneuBaseState : State
{
    #region Enums
    //-------------------------------------------------------------------------
    // Enums

    public enum StateNames { Default, Grounded, Flying, Dead }
    public enum PhaseNames { Begin, Phase1, Phase2, Phase3, End }

    #endregion

    #region Protected Members
    //-------------------------------------------------------------------------
    // Protected Members

    protected readonly KeneuStateMachine stateMachine;

    protected const float MIN_DISTANCE = 0.5f;
    protected const float MAX_DISTANCE = 25.0f;

    //protected const float AnimationDampTime = 0.2f;
    //protected const float AnimationCrossFade = 0.2f;
    //protected const float SmoothInputVelocity = 0.2f;
    //protected const float SmoothInputSpeed = 0.2f;

    //protected Dictionary<int, AnimationEnumData> animations = new Dictionary<int, AnimationEnumData>(); //AnimHashEnum> animHashDictionary = new Dictionary<int, AnimHashEnum>();

    //protected Vector2 mousePos = Mouse.current.position.ReadValue();
    //protected bool keyPressed = false;

    //protected bool isSprinting = false;
    //protected float sprintAdjust = 2.0f;

    //protected float fov;
    //protected float cameraDist;
    //protected float sensitivity;
    //protected float zoomSpeed = 1.0f;
    //protected float zoomAcceleration = 2.5f;
    //protected float zoomInnerRange = 3.0f;
    //protected float zoomOuterRange = 50.0f;

    //private float zoomYAxis = 0.0f;
    //public float ZoomYAxis
    /*{
        get { return zoomYAxis; }
        set
        {
            if (zoomYAxis == value) return;

            zoomYAxis = value;
            AdjustCameraZoomIndex(ZoomYAxis);
        }
    }
    */

    protected GameObject targetGO { get; private set; }
    protected RaycastHit lastHit { get; private set; }
    protected Renderer targetRenderer;
    private Material origMat;


    //private Vector2 currentInputVector;
    //private Vector2 smoothInputVelocity;
    //private float smoothInputSpeed = 0.2f;
    #endregion


    #region Protected Functions
    //-------------------------------------------------------------------------
    // Protected Functions

    //-------------------------------------------------------------------------
    // PlayerBaseState - Constructor For The State
    //-------------------------------------------------------------------------
    protected KeneuBaseState(KeneuStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

    }

    /*
    //-------------------------------------------------------------------------
    // UpdateTarget - Raycast To Get Target, And Update Last Hit Info
    //-------------------------------------------------------------------------
    protected void UpdateTarget()
    {
        Ray ray = new Ray(Camera.main.transform.position, Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MAX_DISTANCE))
        {
            lastHit = hit;

            if (targetGO != null && targetGO != hit.collider.transform.gameObject)
            {
                UnhighlightTarget();
            }

            targetGO = hit.collider.transform.gameObject;
            HighlightTarget();
        }
    }

    //-------------------------------------------------------------------------
    // HighlightTarget - Change Material Of Target To Highlight Target
    //-------------------------------------------------------------------------
    protected void HighlightTarget()
    {
        Renderer rend = targetGO.GetComponent<Renderer>();
        origMat = rend.material;

        if (targetGO.CompareTag("GrapplePoint") || targetGO.CompareTag("PullPoint"))
        {
            targetGO.GetComponent<Renderer>().material.color = new Color(230, 230, 250);
        }
    }

    protected void UnhighlightTarget()
    {
        targetGO.GetComponent<Renderer>().material = origMat;
    }
    */

    /*
    protected int ReturnKeyHashPressed()
    {
        int keyHash = -1;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
            keyHash = Keyboard.current.anyKey.GetHashCode;

        return keyHash;
    }

    protected void Pause()
    {

    }
    */

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
        if (isSprinting) speed *= sprintAdjust;

        float horz = stateMachine.InputReader.Move.x;
        float vert = stateMachine.InputReader.Move.y;

        Vector2 input = stateMachine.InputReader.Move;
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);

        Vector3 forward = new(stateMachine.Cam.transform.forward.x, 0, stateMachine.Cam.transform.forward.z);
        Vector3 right = new(stateMachine.Cam.transform.right.x, 0, stateMachine.Cam.transform.right.z);

        Vector3 moveDir = (currentInputVector.y * forward.normalized + currentInputVector.x * right.normalized) * speed * Time.deltaTime;
        if (moveDir.sqrMagnitude > 1.0f) moveDir.Normalize();

        moveDir.y = 0.0f;

        stateMachine.Velocity = moveDir;
        */


        //Vector3 currentMove = new Vector3(-horz, 0, vert) * Time.deltaTime;  //stateMachine.Controller.isGrounded ? 0.0f : -1.0f, vert) * Time.deltaTime;
    }

    /*
    protected void CalculateInAirMoveDirection()
    {
        Vector3 camForward = new(stateMachine.Cam.transform.forward.x, 0, stateMachine.Cam.transform.forward.z);
        Vector3 camRight = new(stateMachine.Cam.transform.right.x, 0, stateMachine.Cam.transform.right.z);
        Vector3 moveDirection = camForward.normalized * stateMachine.InputReader.Move.y + camRight.normalized * stateMachine.InputReader.Move.x;

        stateMachine.Velocity.x = moveDirection.x * stateMachine.MovementSpeed;

        if (stateMachine.InputReader.Move.y < 0.0f) return;

        stateMachine.Velocity.z = moveDirection.z * stateMachine.MovementSpeed;
    }
    */

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
        //Vector3 forward = stateMachine.Cam.transform.forward;

        //right.y = 0.0f;
        //forward.y = 0.0f;

        //right = right.normalized;
        //forward = forward.normalized;

        Vector3 rightProduct = right.normalized * vectorToRotate.x;
        Vector3 forwardProduct = forward.normalized * vectorToRotate.z;

        Vector3 vectorRotated = rightProduct + forwardProduct;
        vectorRotated.y = vectorToRotate.y;

        return vectorRotated;
    }

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
    }

    protected void NextPhase()
    {
        //switch(currentPhase)
        //{

        //}
    }

    //-------------------------------------------------------------------------
    // SwitchToJumpState - Transition To Jump State
    //-------------------------------------------------------------------------
    protected void SwitchToGroundState()
    {
        if (GameManager.instance.debugLog) Debug.Log("Go To Ground State From " + stateMachine.CurrentState.ToString());

        // Player Enters Jump Button, Move To Jump State
        stateMachine.SwitchState(new KeneuGroundState(stateMachine));
    }

    //-------------------------------------------------------------------------
    // SwitchToJumpState - Transition To Jump State
    //-------------------------------------------------------------------------
    protected void SwitchToFlyState()
    {
        if (GameManager.instance.debugLog) Debug.Log("Go To Fly State From " + stateMachine.CurrentState.ToString());

        // Player Enters Jump Button, Move To Jump State
        stateMachine.SwitchState(new KeneuFlyState(stateMachine));
    }

    //-------------------------------------------------------------------------
    // SwitchToAimState - Transition To Aim Mode State
    //-------------------------------------------------------------------------
    protected void SwitchToDeadState()
    {
        if (GameManager.instance.debugLog) Debug.Log("Go To Dead State From " + stateMachine.CurrentState.ToString());

        // Player Enters Aim Button, Move To Aim State
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

    #endregion
}
