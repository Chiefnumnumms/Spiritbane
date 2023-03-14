//-------------------------------------------------------------------------
//  KeneuStateMachine
//  Author:  Kevin Howell  
//  Date: February 14, 2023
//  Purpose:  Script To Control Keneu's State

using System;
using UnityEngine;

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
    // Idle - Standing Around
    // Walk - Moving Around The Ground
    // Hover - Flying Idle
    // Fly - Flying Moving
    // TakeOff - Transition To Flight
    // Land - Transition To Ground
    // Strike - Keneu Slams Ground Dealing Area Of Effect Damage
    // Fire - Launch Fireball (Have Telegraph Of Charging Up & For Players Screen)

    public enum FightState { Prefight, Start, Phase1, Phase2, Phase3, Finish }
    // Prefight - Wait For Gaoh Trigger Transition To Start

    // Start - Run Dialogue/Cutscene, initialize fight then switch to Phase1

    // Phase 1 Loop - Idle > Fly > Strike > Walk > TakeOff
    //      Fly Away > Dive At Gaoh > Gaoh Use Agreskoul To Pull Tail To Slam Into Ground Twice To Transition Phase

    // Phase 2 Loop - TakeOff > Fly > Strike/Fire > Walk
    //      Main Platform Become A Hazard Area Via Tornado Forcing Goah To Use The 4 Smaller Platforms
    //      Keneu Will Stay Between The Four Platforms In FLying State And Attack By Diving Towards Or Shooting Fireball
    //      Gaoh Must Reflect Back 3 Fireballs To Transition Phase

    // Phase 3 Loop - Flying > Fireball/WingPushback
    //      Keneu Will Power Up While The Main Platform Breaks Into Smaller Platforms & Rocks
    //      Keneu Will Then Fly Outside Gaohs Reach From The Platforms And Lob Fireballs And Use WingPushback 
    //      3 Tornados Spawn That Gaoh Needs To Pull Rocks Into Which Fling Into Keneu (3 Times To Transition)

    // Finish
    //     Cutscene > Credits


    #endregion

    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

    // Cached References
    public Animator Animator { get; private set; }
    public Transform MainCam { get; private set; }
    public State CurrentState { get; private set; }
    public FightState CurrentFightState;

    [Header("Player Transform For Script Access Ease")]
    public GameObject Player;

    [Header("Keneu Movement Values")]
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

    public void UpdateAnimHashFromName(string animName)
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


    #endregion


    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    //-------------------------------------------------------------------------
    // Start - Initialization Of Script
    //-------------------------------------------------------------------------
    private void Start()
    {
        // Cache The Dependancies
        Animator = GetComponent<Animator>();

        // Cache The Main Cam Transform
        MainCam = Camera.main.transform;

        // Set State To Default State
        SwitchState(new KeneuGroundState(this));

        // Get Current State
        CurrentState = base.currentState;

        CurrentFightState = FightState.Prefight;

        // Initialize Animation Array And Dictionary
        //InitializeAnimationDataArray();
        //InitializeAnimationDictionary();
    }

    #endregion



}
