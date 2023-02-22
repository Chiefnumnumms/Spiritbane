using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour, PlayerControls.IPlayerActionsActions
{

    // Public Actions
    public Action OnAgreskoulStarted;
    public Action OnAgreskoulPerformed;

    public Action OnJumpStarted;
    public Action OnSprintStarted;

    // OnAction - Invoke Subscritions On Sprint Canceled Input
    public void OnAgreskoul(InputAction.CallbackContext context)
    {
        // Avoid Double Calling
        if (!context.started) return;

        // Invoke Subscriptions To The Action If Not Null
        OnAgreskoulStarted?.Invoke();
    }

    // OnAction - Invoke Subscritions On Action Performed Input
    public void OnAgreskoulReleased(InputAction.CallbackContext context)
    {
        // Avoid Double Calling
        if (!context.performed) return;

        // Invoke Subscriptions To The Action If Not Null
        OnAgreskoulPerformed?.Invoke();
    }

    // OnAction - Invoke Subscritions On Sprint Canceled Input
    public void OnJump(InputAction.CallbackContext context)
    {
        // Avoid Double Calling
        if (!context.started) return;

        // Invoke Subscriptions To The Action If Not Null
        OnJumpStarted?.Invoke();
    }

    // OnAction - Invoke Subscritions On Sprint Canceled Input
    public void OnSprint(InputAction.CallbackContext context)
    {
        // Avoid Double Calling
        if (!context.started) return;

        // Invoke Subscriptions To The Action If Not Null
        OnSprintStarted?.Invoke();
    }


    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimationManager animationManager;
    Swinging swingingManager;
    ObjectGrapple grapplingManager;
    ItemPickup itemPickup;
    PlayerStats characterManager;
    Agreskoul agreskoulManager;

    private void Awake()
    {
        animationManager = GetComponent<AnimationManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        swingingManager = GetComponent<Swinging>();
        grapplingManager = GetComponent<ObjectGrapple>();
        itemPickup = FindObjectOfType<ItemPickup>();
        characterManager = GetComponent<PlayerStats>();
        agreskoulManager = GetComponent<Agreskoul>();
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

    // AGRESKOUL
    public bool agreskoul_Pressed;

    // GRAPPLING - AA
    public bool grapple_Pressed;

    // GRAPPLE OBJECT - AA
    public bool grappleObject_Pressed;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += inputContext => movementInput = inputContext.ReadValue<Vector2>();
            playerControls.PlayerActions.Sprint.performed += inputContext => sprintPressed = true;
            playerControls.PlayerActions.Sprint.canceled += inputContext => sprintPressed = false;
            playerControls.PlayerActions.Jump.performed += inputContext => jumpPressed = true;
            
            // AGRESKOUL
            playerControls.PlayerActions.Agreskoul.performed += inputContext => agreskoul_Pressed = true;
            playerControls.PlayerActions.Agreskoul.canceled += inputContext => agreskoul_Pressed = false;


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

            playerControls.Grapple.GrappleObject.performed += inputContext => grappleObject_Pressed = true;
            playerControls.Grapple.GrappleObject.canceled += inputContext => grappleObject_Pressed = false;

            // PICKUP - AA
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
        if (characterManager.isDead)
        {
            return;
        }

        HandleMovementInput();

        HandleSprintingInput();
        HandleJumpingInput();

        // SWINGING - AA
        HandleSwingingInput();

        // GRAPPLING - AA
        HandleGrapplingInput();

        // ITEM PICKUP - AA
        HandlePickupInput();

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
    public void HandleSprintingInput()
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
        if (agreskoul_Pressed) // PLAYER SWINGING
        {
            agreskoulManager.HandleSwingAction();
            agreskoulManager.ExecuteSwordSwing();
        }
        else // NOT SWINGING
        {
            agreskoulManager.StopSwing();
            agreskoulManager.RetractBlade();
            agreskoul_Pressed = false;
        }
    }

    private void HandleGrapplingInput()
    {
        if (grappleObject_Pressed)
        {
            // GRAPPLE
            grapplingManager.StartGrapple();
            grappleObject_Pressed = false;
        }
    }

    private void HandlePickupInput()
    {
        itemPickup.ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(itemPickup.ray, out itemPickup.hit, itemPickup.rayLength))
        {
            if (itemPickup.hit.collider.CompareTag("Interactable"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    itemPickup.items.Add(itemPickup.hit.collider.gameObject);
                    itemPickup.itemText.text = "Items: " + itemPickup.items.Count;
                    itemPickup.hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }

}
