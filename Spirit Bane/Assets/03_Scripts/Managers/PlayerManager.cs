using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Animator animator;
    PlayerLocomotion playerLocomotion;
    Swinging swingingManager;

    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        swingingManager = GetComponent<Swinging>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        // LOCK CURSOR
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovements();

        // SWINGING - AA
        if (inputManager.swingPressed)
        {
            swingingManager.DrawRope();
        }
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}
