using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimationManager animationManager;
    Swinging swingingManager;

    Vector3 moveDir;
    Transform playerCamera;

    Rigidbody playerRb;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVel;
    public float fallingVel;
    public float rayCastHeightOffset = 1f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isSprinting;
    public bool isJumping;

    [Header("Jumping Values")]
    public float jumpHeight = 3f;
    public float gravityIntensity = -8.91f;

    [Header("Movement Speeds")]
    public float walkSpeed = 2;
    public float runSpeed = 5;
    public float sprintSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animationManager = GetComponent<AnimationManager>();
        inputManager = GetComponent<InputManager>();
        swingingManager = GetComponent<Swinging>();
        playerRb = GetComponent<Rigidbody>();
        playerCamera = Camera.main.transform;
    }

    //-----------------------------------------------------------------------------
    // calls the methods for the players rotation and movement
    public void HandleAllMovements()
    {
        HandleFallingAndLanding();

        if(playerManager.isInteracting)
        {
            return;
        }

        if(isJumping)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
    }

    //------------------------------------------------------------------------------------
    // adjusts the players movement based on cameras direction and what the users input is
    private void HandleMovement()
    {
        moveDir = playerCamera.forward * inputManager.vertInput;
        moveDir = moveDir + playerCamera.right * inputManager.horizInput;
        moveDir.Normalize();

        if(isSprinting)
        {
            moveDir = sprintSpeed * moveDir;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
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
        Vector3 targetDir = Vector3.zero;

        targetDir = playerCamera.forward * inputManager.vertInput;
        targetDir = targetDir + playerCamera.right *inputManager.horizInput;
        targetDir.Normalize();

        targetDir.y = 0;
        if(targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        
        transform.rotation = playerRot;
    }

    //---------------------------------------------------------------------------------------
    // handles when the player should enter the falling animation state vs the landing state
    private void HandleFallingAndLanding()
    {
        if (swingingManager.isSwinging) return;

        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animationManager.PlayTargetAnim("Falling Idle", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRb.AddForce(transform.forward * leapingVel);
            playerRb.AddForce(-Vector3.up * fallingVel * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animationManager.PlayTargetAnim("Landing", true);
            }
            inAirTimer = 0;
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
    }

    //---------------------------------------------------------------------------------------
    // handles how the player moves when the jump button is pressed
    public void HandleJump()
    {
        if(isGrounded)
        {
            animationManager.animator.SetBool("isJumping", true);
            animationManager.PlayTargetAnim("Jump", false);

            float jumpingVel = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVel = moveDir;
            playerVel.y = jumpingVel;
            playerRb.velocity = playerVel;
        }
    }
}