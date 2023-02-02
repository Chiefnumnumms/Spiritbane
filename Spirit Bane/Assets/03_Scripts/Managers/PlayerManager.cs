using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Animator animator;
    PlayerLocomotion playerLocomotion;
    Swinging swingingManager;
    Grappling grappleManager;

    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        swingingManager = GetComponent<Swinging>();
        grappleManager = GetComponent<Grappling>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        // SWINGING - AA
        swingingManager.CheckForSwingPoints();
        swingingManager.HighlightGrapplePoint(swingingManager.maxIndicationDistance);

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
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);        
    }
}
