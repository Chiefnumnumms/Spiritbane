using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimationManager animationManager;
    Swinging swingingManager;
    ObjectGrapple grapplingManager;
    Agreskoul agreskoulManager;

    Vector3 moveDir;
    [SerializeField]
    Transform playerCamera;

    public Rigidbody playerRb;

    [Header("Wind")]
    public GameObject windZone;
    public Vector3 windDir;
    public float windStr;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVel;
    public float fallingVel = 30;
    public float rayCastHeightOffset = 1f;
    public LayerMask groundLayer;

    //[Header("Ground And Air Detection")]
    //[SerializeField]
    //private float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    private float maximumDistanceNeededToStartFall = 1f;

    [Header("Ground & Air Detection Stats")]
    //[SerializeField]
    //float minimumDistanceNeededToBeginFall = 1.0f;    // Minimum Distance needed to play fall animation
    //[SerializeField]
    //float groundDirectionRayDistance = 0.2f;        // Ray check distance
    //[SerializeField]
    //float maxDistance = 1f;
    Vector3 targetPosition;
    Vector3 normalVector;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isSprinting;
    public bool isJumping;
    public bool inWindZone;

    [Header("Jumping Values")]
    public float jumpHeight = 3f;
    public float gravityIntensity = -8.91f;

    [HideInInspector]
    public Transform myTransform;

    [Header("Movement Speeds")]
    public float walkSpeed = 2;
    public float runSpeed = 5;
    public float movementSpeed = 5f;
    public float sprintSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Grappling & Swinging")]
    private Vector3 velocityToSet;
    public bool activeGrapple = true;
    public bool enableMovementAfterGrapple;

    // Wwise
    [Header("Wwise Events")]
    public AK.Wwise.Event playerWalk;
    public AK.Wwise.Event playerRun;
    public AK.Wwise.Event playerJump;
    public AK.Wwise.Event playerLand;
    //public AK.Wwise.Event playerDodge;
    //public AK.Wwise.Event playerAttack; 
    public AK.Wwise.Event playerShield;
    public AK.Wwise.Event playerHurt;
    public AK.Wwise.Event playerDead;

    private bool stepPlaying = false;
    private float lastStepTime = 0.0f;
    //private float currentVelocity = 0.0f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animationManager = GetComponent<AnimationManager>();
        inputManager = GetComponent<InputManager>();
        agreskoulManager = GetComponent<Agreskoul>();
        playerRb = GetComponent<Rigidbody>();

        if(playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
        
        swingingManager = GetComponent<Swinging>();
        grapplingManager = GetComponent<ObjectGrapple>();

        myTransform = transform;

        lastStepTime = Time.time;

        // Subscribe To Relevant Button Input
        inputManager.OnAgreskoulStarted += AgreskoulPressed;       
        inputManager.OnAgreskoulPerformed += AgreskoulReleased;
        
        inputManager.OnJumpStarted += HandleJump;
        inputManager.OnSprintStarted += inputManager.HandleSprintingInput;
    }

    private void OnDisable()
    {
        // Unsubscribe To Relevant Button Input
        inputManager.OnAgreskoulStarted -= AgreskoulPressed;
        inputManager.OnAgreskoulPerformed -= AgreskoulReleased;
        inputManager.OnJumpStarted -= HandleJump;
        inputManager.OnSprintStarted -= inputManager.HandleSprintingInput;
    }

    private void AgreskoulPressed()
    {
        // Launch Sword On Button Press
        agreskoulManager.ExecuteSwordAction();
        Debug.Log("Extended Blade");
    }

    private void AgreskoulReleased()
    {
        // Bring Sword Back Upon Release
        agreskoulManager.RetractBlade();
        Debug.Log("Retracted Blade");
    }

    //-----------------------------------------------------------------------------
    // calls the methods for the players rotation and movement
    public void HandleAllMovements()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
        {
            return;
        }

        if (isJumping)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
    }

    // KH
    private void Step()
    {
        // If Speed Is Too Low Back Out
        if (playerRb.velocity.magnitude < 1.0f || !isGrounded) return;

        // If Sound Not Playing, Play Sound, Record Time, Set Flag For Sound Playing
        if (!stepPlaying)
        {
            playerWalk.Post(gameObject);
            lastStepTime = Time.time;
            stepPlaying = true;
        }
        else
        {
            if (Time.time - lastStepTime > 500.0f / playerRb.velocity.magnitude * Time.deltaTime)
            {
                stepPlaying = false;
            }
        }
    }

    private void StepRun()
    {
        if(playerRb.velocity.magnitude < 1.0f || !isGrounded) return;

        if(!stepPlaying)
        {
            playerRun.Post(gameObject);
            lastStepTime = Time.time;
            stepPlaying = true;
        }
        else
        {
            if(Time.time - lastStepTime > 300.0f / playerRb.velocity.magnitude * Time.deltaTime)
            {
                stepPlaying = false;
            }
        }
    }

    private void JumpSFX()
    {
        if (!isGrounded) return;

        playerJump.Post(gameObject);
    }

    private void LandSFX()
    {
        if (isGrounded) return;

        playerLand.Post(gameObject);
    }

    private void ShieldSFX()
    {
        playerShield.Post(gameObject);
    }

    /*
    private void DodgeSFX()
    {
        playerDodge.Post(gameObject);
    }

    private void AttackSFX()
    {
        playerAttack.Post(gameObject);
    }
    */

    private void HurtSFX()
    {
        playerHurt.Post(gameObject);
    }

    private void DeadSFX()
    {
        playerDead.Post(gameObject);
    }

    //------------------------------------------------------------------------------------
    // adjusts the players movement based on cameras direction and what the users input is
    private void HandleMovement()
    {
        if (swingingManager.isSwinging) return;
        //if (activeGrapple) return;

        if(inWindZone)
        {
            playerRb.AddForce(windDir * windStr);
        }


        moveDir = playerCamera.forward * inputManager.vertInput;
        moveDir = moveDir + playerCamera.right * inputManager.horizInput;
        moveDir.Normalize();

        if(isSprinting && inputManager.moveAmount > 0)
        {
            moveDir = sprintSpeed * moveDir;
        }
        else
        {
            if (inputManager.moveAmount > 0.5f)
            {
                moveDir = runSpeed * moveDir;
            }
            else
            {
                moveDir = walkSpeed * moveDir;
            }
        }

        moveDir.y = 0;
        

        Vector3 movementVel = moveDir;
        playerRb.velocity = movementVel;
    }

    //------------------------------------------------------------------------------------
    // adjusts the players rotation based on camera direction and player movement
    private void HandleRotation()
    {
        if (playerManager.canRotate)
        {
            Vector3 targetDir = Vector3.zero;

            targetDir = playerCamera.forward * inputManager.vertInput;
            targetDir = targetDir + playerCamera.right * inputManager.horizInput;
            targetDir.Normalize();

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward;
            }

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            transform.rotation = targetRot;
        }
        else
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputManager.moveAmount;

            targetDir = playerCamera.forward * inputManager.vertInput;
            targetDir += playerCamera.right * inputManager.horizInput;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * Time.deltaTime);

            myTransform.rotation = targetRotation;
        }
    }

    //---------------------------------------------------------------------------------------
    // handles when the player should enter the falling animation state vs the landing state
    private void HandleFallingAndLanding()
    {
        if (swingingManager.isSwinging) return;

        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPos;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        targetPos = transform.position;

        float origY = transform.position.y;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting && !swingingManager.isSwinging)
            {
                animationManager.PlayTargetAnim("Falling Idle", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRb.AddForce(transform.forward * leapingVel);
            playerRb.AddForce(-Vector3.up * fallingVel * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.28f, -Vector3.up, out hit, maximumDistanceNeededToStartFall, groundLayer))
        {
            if (transform.position.y == origY) { isGrounded = true; playerManager.isInteracting = false; }

            if (!isGrounded && !playerManager.isInteracting)
            {
                animationManager.PlayTargetAnim("Landing", false);
            }
            Vector3 rayCastHitPoint = hit.point;
            targetPos.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
            //LandSFX();
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPos;
            }
        }
    }

    //---------------------------------------------------------------------------------------
    // handles how the player moves when the jump button is pressed
    public void HandleJump()
    {
        if (swingingManager.isSwinging) return;

        if (isGrounded)
        {
            if (inputManager.jumpPressed)
            {
                animationManager.animator.SetBool("isJumping", true);
                animationManager.PlayTargetAnim("Jump", false, true);
                //JumpSFX();

                float jumpingVel = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                Vector3 playerVel = moveDir;
                playerVel.y = jumpingVel;
                playerRb.velocity = playerVel;
            }
        }
    }

    public void HandleAgreskoulAction()
    {
        agreskoulManager.ExecuteSwordAction();
    }

    public void HandleAgreskoulReleaseAction()
    {
        agreskoulManager.RetractBlade();
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        // DISPLACEMENT VALUES FOR XYZ
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
                                             + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);

        // APPLY FORCE WITH A LITTLE BIT OF A DELAY (AFTER 0.1 SECONDS)
        Invoke(nameof(SetVelocity), 0.1f);

        // RESET MOVEMENT INCASE OF ANYTHING GOING WRONG WHEN GRAPPLING MULTIPLE TIMES
        Invoke(nameof(ResetMovementRestrictions), 3f);
    }

    //public void HandleFalling(Vector3 moveDirection)
    //{
    //    isGrounded = false;
    //    RaycastHit hit;
    //    Vector3 origin = myTransform.position;
    //    origin.y += groundDetectionRayStartPoint;

    //    if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
    //    {
    //        moveDirection = Vector3.zero;
    //    }

    //    if (!isGrounded)
    //    {
    //        playerRb.AddForce(-Vector3.up * fallingVel);
    //        playerRb.AddForce(moveDirection * fallingVel / 10f);
    //    }

    //    Vector3 dir = moveDirection;
    //    dir.Normalize();
    //    origin = origin + dir * groundDirectionRayDistance;

    //    targetPosition = myTransform.position;

    //    Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
    //    if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundLayer))
    //    {
    //        normalVector = hit.normal;
    //        Vector3 tp = hit.point;
    //        isGrounded = true;
    //        targetPosition.y = tp.y;

    //        if (!isGrounded)
    //        {
    //            if (inAirTimer > 0.5f)
    //            {
    //                Debug.Log("You were in the air for " + inAirTimer);
    //                animationManager.PlayTargetAnim("Landing", true);
    //                inAirTimer = 0;
    //            }
    //            else
    //            {
    //                animationManager.PlayTargetAnim("Empty", false);
    //                inAirTimer = 0;
    //            }

    //            isGrounded = false;
    //        }
    //    }
    //    else
    //    {
    //        if (isGrounded)
    //        {
    //            isGrounded = false;
    //        }

    //        if (isGrounded == false)
    //        {
    //            if (playerManager.isInteracting == false)
    //            {
    //                animationManager.PlayTargetAnim("Falling Idle", true);
    //            }

    //            Vector3 vel = playerRb.velocity;
    //            vel.Normalize();
    //            playerRb.velocity = vel * (movementSpeed / 2);
    //            isGrounded = true;
    //        }
    //    }

    //    if (playerManager.isInteracting || inputManager.moveAmount > 0)
    //    {
    //        myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
    //    }
    //    else
    //    {
    //        myTransform.position = targetPosition;
    //    }

    //    // BUG FIX: This now ensures that the player is always going to the target position
    //    //          instead of behaving oddly when moving around
    //    //          This ensures players feet are not going under the ground when performing an animation
    //    if (playerManager.isInteracting || inputManager.moveAmount > 0)
    //    {
    //        myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
    //    }
    //    else
    //    {
    //        myTransform.position = targetPosition;
    //    }

    //    if (isGrounded)
    //    {
    //        if (playerManager.isInteracting || inputManager.moveAmount > 0)
    //        {
    //            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
    //        }
    //        else
    //        {
    //            myTransform.position = targetPosition;
    //        }
    //    }
    //}

    public void SetVelocity()
    {
        enableMovementAfterGrapple = true;
        playerRb.velocity = velocityToSet;
    }

    public void ResetMovementRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementAfterGrapple)
        {
            // RESET ALL MOVEMENT RESTRICTIONS AND ALLOW PLAYER TO MOVE FREELY AFTER GRAPPLE
            enableMovementAfterGrapple = false;
            ResetMovementRestrictions();

            grapplingManager.StopGrapple();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "WindArea")
        {
            windZone = other.gameObject;
            windDir = windZone.GetComponent<WindArea>().windDirection;
            windStr = windZone.GetComponent<WindArea>().windStrength;
            inWindZone = true;
        }
        else if(other.gameObject.tag == "Finish") ScenesManager.instance.LoadNextScene();


    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "WindArea")
            inWindZone = false;
    }
}
