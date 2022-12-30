using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimationManager animationManager;
    private void Awake()
    {
        animationManager = GetComponent<AnimationManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public Vector2 movementInput;
    public float moveAmount;
    public float vertInput;
    public float horizInput;

    public bool sprintPressed;
    public bool jumpPressed;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += inputContext => movementInput = inputContext.ReadValue<Vector2>();
            playerControls.PlayerActions.Sprint.performed += inputContext => sprintPressed = true;
            playerControls.PlayerActions.Sprint.canceled += inputContext => sprintPressed = false;
            playerControls.PlayerActions.Jump.performed += inputContext => jumpPressed = true;
        }
        playerControls.Enable();
    }


    private void OnDisable()
    {
        playerControls.Disable();
    }

    //-----------------------------------------------------------------------------
    // calls all the methods to check for player inputs
    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    //-----------------------------------------------------------------------------
    // stores the current input and calls the method to adjust the players movement
    private void HandleMovementInput()
    {
        vertInput = movementInput.y;
        horizInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizInput) + Mathf.Abs(vertInput));
        animationManager.UpdateAnimatiorValues(0, moveAmount,sprintPressed);
    }

    //---------------------------------------------------------------------
    // adjusts the bool to handle sprinting when the sprint button is held
    private void HandleSprintingInput()
    {
        if (sprintPressed && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    //-----------------------------------------------------------------
    // calls the method to handle jump when the jump button is pressed
    private void HandleJumpingInput()
    {
        if(jumpPressed)
        {
            playerLocomotion.HandleJump();
            jumpPressed = false;
        }
    }
}
