using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager inputManager;
    private Animator animator;
    private PlayerLocomotion playerLocomotion;
    private Agreskoul agreskoulManager;

    public bool isInteracting;
    public bool canRotate;
    public bool isInAir;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        agreskoulManager = GetComponent<Agreskoul>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

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
        
        if(isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }
}
