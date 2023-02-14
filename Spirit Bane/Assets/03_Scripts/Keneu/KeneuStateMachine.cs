//-------------------------------------------------------------------------
//  KeneuStateMachine
//  Author:  Kevin Howell  
//  Date: February 14, 2023
//  Purpose:  Script To Control Keneu's State
using UnityEngine;
using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine.ProBuilder.Shapes;

//-------------------------------------------------------------------------
// This Class Represents The Keneu's State Controller Requiring Animator Component
[RequireComponent(typeof(Animator))]
public class KeneuStateMachine : StateMachine
{
    /*
    public class AnimationData
    {
        protected AnimList id { get; private set; }
        protected string name { get; private set; }
        protected int hash { get; private set; }
        protected float crossfade { get; private set; }

        //-------------------------------------------------------------------------
        // AnimationData - Constructor For The Class
        //-----------------------------------------------------------------------
        private AnimationData() { id = AnimList.Idle; name = AnimList.Idle.ToString(); hash = Animator.StringToHash(name); crossfade = 0.2f; }
        public AnimationData(AnimList inID, string inName, int inHash, float inCrossfade) { id = inID; name = inName; hash = inHash; crossfade = inCrossfade; }

        protected void Initialize(AnimList inID, string inName, int inHash, float inCrossfade) { id = inID; name = inName; hash = inHash; crossfade = inCrossfade; }
    }
    */

    #region Enums
    //-------------------------------------------------------------------------
    // Enums

    public enum AnimList { Idle, Walking, Hover, Flying, Takeoff, Strike, Fire }

    #endregion

    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

    // Cached References
    public Animator Animator { get; private set; }
    public Transform MainCam { get; private set; }
    public State CurrentState { get; private set; }

    [Header("Player Transform For Script Access Ease")]
    public GameObject player;

    [Header("Player Movement Values")]
    public Vector3 Velocity;
    public float MovementSpeed { get; private set; } = 8.0f;
    public float LookRoatationDampFactor { get; private set; } = 10.0f;

    [Header("Animation Smoothing Values")]
    public float AnimationDampTime = 0.2f;
    public float AnimationCrossFade = 0.2f;

    //protected AnimationData[] animations;
    public AnimList CurrentAnim { get; private set; }
    //public Dictionary<int, AnimationEnumData> AnimationDictionary;
    #endregion


    #region Public Functions
    //-------------------------------------------------------------------------
    // Public Functions

    public int GetParameterHash(string parameter)
    {
        return Animator.StringToHash(parameter);
    }

    public bool GetBoolParameterFromHash(int hash)
    {
        return Animator.GetBool(hash);
    }

    public void GetAnimHashFromName(string animName)
    {
        if (Enum.TryParse<AnimList>(animName, out AnimList animHash)) CurrentAnim = animHash;

        //uint temp = Convert.ToUInt32(Enum.Parse(typeof(AnimHash), animName));
    }

    public int GetHashFromAnimHash(AnimList hash)
    {
        //if (Enum.TryParse<string>(hash, out string name))// currentHash = animHash;
        //{

        //}

        string name = hash.ToString();
        return Animator.StringToHash(name);
    }

    /*
    public int GetHashFromAnim(AnimList hash)
    {
        Enum.GetName(typeof(AnimList));
        return Animator.StringToHash(hash);
    }
    */

    public void ToggleParameterBoolFromHash(int hash)
    {
        bool value = GetBoolParameterFromHash(hash);
        Animator.SetBool(hash, !value);
    }

    public void PlayTargetAnimation(string targetAnimation)
    {
        Animator.CrossFade(targetAnimation, AnimationCrossFade);
    }

    public void PlayTargetAnimation(string targetAnimation, float crossfadeDuration)
    {
        Animator.CrossFade(targetAnimation, crossfadeDuration);
    }
    /*
    */

        /*
    protected void InitializeAnimationDictionary()
    {
        if (AnimationDictionary != null) AnimationDictionary.Clear();
        else AnimationDictionary = new Dictionary<int, AnimationEnumData>();

        int count = -1;

        foreach (string name in Enum.GetNames(typeof(AnimList)))
        {
            count++;
            int key = count;
            int hash = GetParameterHash(name);
            AnimList value = (AnimList)Enum.Parse(typeof(AnimList), name);
            AnimationEnumData temp = new AnimationEnumData(count, value, name, hash, AnimationCrossFade);

            AnimationDictionary.Add(key, temp);
        }
    }
        */

    /*
    protected void InitializeAnimationDataArray()
    { 
        if (animations.Length > 0) return;
        
        animations = new AnimationData[Enum.GetNames(typeof(AnimList)).Length];

        for(int i = 0; i < animations.Length; i++)
        {
            AnimList enumID = AnimList.Idle + i;
            string inName = enumID.ToString();
            int inHash = GetParameterHash(inName);
            float inCrossFadeDuration = AnimationCrossFade;

             animations[i] = new AnimationData(enumID, inName, inHash, inCrossFadeDuration);             
        }
    }
    */

    /*
    public void UpdateAnimatiorValues(float horizMovement, float vertMovement, bool isSprinting)
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

        if (isSprinting)
        {
            snappedHoriz = horizMovement;
            snappedVert = 2;
        }

        Animator.SetFloat(horizontal, snappedHoriz, 0.1f, Time.deltaTime);
        Animator.SetFloat(vertical, snappedVert, 0.1f, Time.deltaTime);
    }
    */

    #endregion


    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    /*
    private void SwitchActionMap()
    {
        InputReader.SwitchCurrentActionMap("Menu");
    }
    */


    //-------------------------------------------------------------------------
    // Start - Initialization Of Script
    //-------------------------------------------------------------------------
    private void Start()
    {
        // Cache The Dependancies
        Animator = GetComponent<Animator>();

        // Cache The Main Cam Transform
        MainCam = Camera.main.transform;

        // Set State To Default State  --  Currently Defaults To Player Move State
        SwitchState(new KeneuGroundState(this));

        // Get Current State
        CurrentState = base.currentState;

        // Initialize Animation Array And Dictionary
        //InitializeAnimationDataArray();
        //InitializeAnimationDictionary();
    }

    #endregion



}
