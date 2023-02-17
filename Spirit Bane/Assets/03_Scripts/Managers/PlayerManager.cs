using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Animator animator;
    PlayerLocomotion playerLocomotion;
    Swinging swingingManager;
    ObjectGrapple grappleManager;

    public bool isInteracting;
    public bool canRotate;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        swingingManager = GetComponent<Swinging>();
        grappleManager = GetComponent<ObjectGrapple>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        // SWINGING - AA
        swingingManager.CheckForSwingPoints();
        swingingManager.HighlightGrapplePoint(swingingManager.maxIndicationDistance);

        // GRAPPLING - AA
        grappleManager.CheckForGrappleObject();

        // LOCK CURSOR - AA
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovements();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        canRotate = animator.GetBool("canRotate");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);        
    }
}
