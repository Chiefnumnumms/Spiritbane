using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimationManager animationManager;
    Swinging swingingManager;

    private void Awake()
    {
        animationManager = GetComponent<AnimationManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        swingingManager = GetComponent<Swinging>();
    }

    public Vector2 movementInput;
    public float moveAmount;
    public float vertInput;
    public float horizInput;

    public bool sprintPressed;
    public bool jumpPressed;

    // SWINGING - AA
    public bool swingPressed;
    public bool w_Pressed;
    public bool a_Pressed;
    public bool s_Pressed;
    public bool d_Pressed;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += inputContext => movementInput = inputContext.ReadValue<Vector2>();
            playerControls.PlayerActions.Sprint.performed += inputContext => sprintPressed = true;
            playerControls.PlayerActions.Sprint.canceled += inputContext => sprintPressed = false;
            playerControls.PlayerActions.Jump.performed += inputContext => jumpPressed = true;

            // SWINGING - AA
            playerControls.PlayerActions.Swing.performed += inputContext => swingPressed = true;
            playerControls.PlayerActions.Swing.canceled += inputContext => swingPressed = false;
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
        HandleSwingingInput();
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

    private void HandleSwingingInput()
    {
        if (swingPressed)
        {
            swingingManager.HandleSwingAction();

        }
        else // NOT SWINGING
        {
            swingingManager.StopSwing();
        }
    }
}
