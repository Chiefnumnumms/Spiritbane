using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimationManager animationManager;
    Swinging swingingManager;
    Grappling grapplingManager;

    private void Awake()
    {
        animationManager = GetComponent<AnimationManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        swingingManager = GetComponent<Swinging>();
        grapplingManager = GetComponent<Grappling>();
    }

    public Vector2 movementInput;
    public float moveAmount;
    public float vertInput;
    public float horizInput;

    public bool sprintPressed;
    public bool jumpPressed;

    // SWINGING - AA
    public bool swing_Pressed;
    public bool rope_Adjust_Pressed;
    public bool w_Pressed;
    public bool a_Pressed;
    public bool s_Pressed;
    public bool d_Pressed;

    // GRAPPLING - AA
    public bool grapple_Pressed;

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
            playerControls.SwingingActions.Swing.performed += inputContext => swing_Pressed = true;               
            playerControls.SwingingActions.Swing.canceled += inputContext => swing_Pressed = false;
            playerControls.SwingingActions.AdjustRope.performed += inputContext => rope_Adjust_Pressed = true;
            playerControls.SwingingActions.AdjustRope.canceled += inputContext => rope_Adjust_Pressed = false;

            playerControls.SwingingActions.SwingForward.performed += inputContext => w_Pressed = true;
            playerControls.SwingingActions.SwingForward.canceled += inputContext => w_Pressed = false;
            playerControls.SwingingActions.SwingRight.performed += inputContext => d_Pressed = true;
            playerControls.SwingingActions.SwingRight.canceled += inputContext => d_Pressed = false;
            playerControls.SwingingActions.SwingLeft.performed += inputContext => a_Pressed = true;
            playerControls.SwingingActions.SwingLeft.canceled += inputContext => a_Pressed = false;

            // GRAPPLING - AA
            playerControls.GrapplingActions.Grapple.performed += inputContext => grapple_Pressed = true;
            playerControls.GrapplingActions.Grapple.canceled += inputContext => grapple_Pressed = false;
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

        // SWINGING - AA
        HandleSwingingInput();

        // GRAPPLING - AA
        HandleGrapplingInput();
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

    //-----------------------------------------------------------------
    // HANDLES ALL SWINGING MECHANICS - AA
    private void HandleSwingingInput()
    {
        if (swing_Pressed) // PLAYER SWINGING
        {
            swingingManager.HandleSwingAction();
        }
        else // NOT SWINGING
        {
            swingingManager.StopSwing();
            swing_Pressed = false;
        }
    }

    private void HandleGrapplingInput()
    {
        if (grapple_Pressed)
        {
            // GRAPPLE
            grapplingManager.StartGrapple();
        }
        else // NOT GRAPPLING
        {
            grapplingManager.StopGrapple();
            grapple_Pressed = false;
        }
    }

}
