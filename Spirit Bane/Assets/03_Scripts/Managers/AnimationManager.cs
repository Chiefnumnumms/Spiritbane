using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    private int horizontal;
    private int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnim(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }
    public void UpdateAnimatiorValues(float horizMovement, float vertMovement,bool isSprinting)
    {
        float snappedHoriz;
        float snappedVert;

        //snap animations so they dont look broken
        #region Snapped Horizontal
        if (horizMovement > 0 && horizMovement < 0.55f)
        {
            snappedHoriz = 0.5f;
        }
        else if (horizMovement > 0.55f)
        {
            snappedHoriz = 1;
        }
        else if (horizMovement < 0 && horizMovement > -0.55f)
        {
            snappedHoriz = -0.5f;
        }
        else if (horizMovement < -0.55f)
        {
            snappedHoriz = -1;
        }
        else
        {
            snappedHoriz = 0;
        }
        #endregion

        #region Snapped Vertical
        if (vertMovement > 0 && vertMovement < 0.55f)
        {
            snappedVert = 0.5f;
        }
        else if (vertMovement > 0.55f)
        {
            snappedVert = 1;
        }
        else if (vertMovement < 0 && vertMovement > -0.55f)
        {
            snappedVert = -0.5f;
        }
        else if (vertMovement < -0.55f)
        {
            snappedVert = -1;
        }
        else
        {
            snappedVert = 0;
        }
        #endregion

        if(isSprinting)
        {
            snappedHoriz = horizMovement;
            snappedVert = 2;
        }

        animator.SetFloat(horizontal, snappedHoriz, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVert, 0.1f, Time.deltaTime);
    }
}
