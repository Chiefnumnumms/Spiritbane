//-------------------------------------------------------------------------
//  AudioManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Game Audio

using UnityEngine;

//-------------------------------------------------------------------------
// This Class Represents The Audio Manager
public class AudioManager : Singleton<AudioManager>
{
    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

    public float MasterVolume { get { return wwwiseMasterVolume.GetGlobalValue(); } set { UpdateMasterVolume(value); } }
    public float MusicVolume { get { return wwwiseMusicVolume.GetGlobalValue(); } set { UpdateMusicVolume(value); } }
    public float AmbienceVolume { get { return wwwiseAmbienceVolume.GetGlobalValue(); } set { UpdateAmbienceVolume(value); } }
    public float SFXVolume { get { return wwwiseSFXVolume.GetGlobalValue(); } set { UpdateSFXVolume(value); } }

    #endregion


    #region Editor Access Members
    //-------------------------------------------------------------------------
    // Editor Access Members

    [Header("Volume Property Values")]
    [SerializeField] private AK.Wwise.RTPC wwwiseMasterVolume;
    [SerializeField] private AK.Wwise.RTPC wwwiseMusicVolume;
    [SerializeField] private AK.Wwise.RTPC wwwiseAmbienceVolume;
    [SerializeField] private AK.Wwise.RTPC wwwiseSFXVolume;

    /*
    [Header("Wwise Audio Buses")]
    public AK.Wwise.AuxBus main;
    public AK.Wwise.AuxBus music;
    public AK.Wwise.AuxBus ambience;
    public AK.Wwise.AuxBus sfx;

    [Header("Gaoh Wwise Events And Property")]
    [SerializeField] private AK.Wwise.RTPC wwiseSwingSpeed;
    public AK.Wwise.Event playerWalk;
    public AK.Wwise.Event playerRun;
    public AK.Wwise.Event playerJump;
    public AK.Wwise.Event playerLand;
    public AK.Wwise.Event playerShield;
    public AK.Wwise.Event playerHurt;
    public AK.Wwise.Event playerDead;
    */

    #endregion

    #region Private Members
    //-------------------------------------------------------------------------
    // Private Members

    //private AudioMixerGroup masterGroup;
    //private AudioMixerGroup musicGroup;
    //private AudioMixerGroup ambienceGroup;
    //private AudioMixerGroup sfxGroup;

    //private float masterVolume;
    //private float musicVolume;
    //private float ambienceVolume;
    //private float sfxVolume;

    //private float minVolume = -16.0f;
    //private float maxVolume = -80.0f;

    #endregion

    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    public void UpdateMasterVolume(float value)
    {
        wwwiseMasterVolume.SetGlobalValue(value);
    }

    public void UpdateMusicVolume(float value)
    {
        wwwiseMusicVolume.SetGlobalValue(value);
    }

    public void UpdateAmbienceVolume(float value)
    {
        wwwiseAmbienceVolume.SetGlobalValue(value);
    }

    public void UpdateSFXVolume(float value)
    {
        wwwiseSFXVolume.SetGlobalValue(value);
    }
    
    #endregion

}
