//-------------------------------------------------------------------------
//  AudioManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Game Audio

using UnityEngine;
using UnityEngine.UI;

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

    #region Private Members


    #endregion

    #region Editor Access Members
    //-------------------------------------------------------------------------
    // Editor Access Members

    [Header("Volume Property Values")]
    [SerializeField] private AK.Wwise.RTPC wwwiseMasterVolume;
    [SerializeField] private AK.Wwise.RTPC wwwiseMusicVolume;
    [SerializeField] private AK.Wwise.RTPC wwwiseAmbienceVolume;
    [SerializeField] private AK.Wwise.RTPC wwwiseSFXVolume;

    [SerializeField] private Slider masterVolume_Slider;
    [SerializeField] private Slider musicVolume_Slider;
    [SerializeField] private Slider ambienceVolume_Slider;
    [SerializeField] private Slider sfxVolume_Slider;

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
