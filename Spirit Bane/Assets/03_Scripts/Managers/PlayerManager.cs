using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Animator animator;
    PlayerLocomotion playerLocomotion;
    Swinging swingingManager;
    PickupManager pickupManager;
    Grappling grappleManager;

    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        swingingManager = GetComponent<Swinging>();
        grappleManager = GetComponent<Grappling>();
        pickupManager = FindObjectOfType<PickupManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        // SWINGING - AA
        swingingManager.CheckForSwingPoints();
        swingingManager.HighlightGrapplePoint(swingingManager.maxIndicationDistance);

        // LOCK CURSOR - AA
        Cursor.lockState = CursorLockMode.Locked;

        // PICKUP SYSTEM - AA
        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovements();

        // SWINGING - AA
        if (inputManager.swing_Pressed)
        {
            swingingManager.DrawRope();
        }
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);


        // PICKUP SYSTEM - AA
        inputManager.e_Pressed = false;
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 2.5f, swingingManager.gunTip.forward, out hit, 1f, pickupManager.pickupLayer))
        {
            if (hit.collider.tag == "Interactable")
            {
                InteractableManager interactableObject = hit.collider.GetComponent<InteractableManager>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;

                    // SET THE UI TEXT TO THE INTERACTABLE OBJECT'S TEXT
                    // SET THE TEXT POP UP TO TRUE

                    if (inputManager.e_Pressed)
                    {
                        hit.collider.GetComponent<InteractableManager>().Interact(this);
                    }
                }
            }
        }
    }
}
