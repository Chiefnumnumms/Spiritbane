using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager inputManager;
    private Animator animator;
    private PlayerLocomotion playerLocomotion;
    private Swinging swingingManager;
    private ObjectGrapple grappleManager;
    private Agreskoul agreskoulManager;

    public bool isInteracting;
    public bool canRotate;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        swingingManager = GetComponent<Swinging>();
        grappleManager = GetComponent<ObjectGrapple>();
        agreskoulManager = GetComponent<Agreskoul>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        // SWINGING - AA
        agreskoulManager.CheckForSwingPoints();
        agreskoulManager.HighlightSwingingPoint(agreskoulManager.maxIndicationDistance);

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
