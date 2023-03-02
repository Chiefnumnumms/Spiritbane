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

    private void UpdateMasterVolume(float value)
    {
        wwwiseMasterVolume.SetGlobalValue(value);
    }

    private void UpdateMusicVolume(float value)
    {
        wwwiseMusicVolume.SetGlobalValue(value);
    }
    private void UpdateAmbienceVolume(float value)
    {
        wwwiseAmbienceVolume.SetGlobalValue(value);
    }

    private void UpdateSFXVolume(float value)
    {
        wwwiseSFXVolume.SetGlobalValue(value);
    }
    
    #endregion


        /*
    public void StartAreaMusic()
    {
        int areaIndex = (int)ScenesManager.instance.CurrentScene.Value;

        AudioClip areaMusic = music[areaIndex];

        if (areaMusic)
        {
            instance.StartCoroutine(AudioFadeOut());

            // Play The Level Music For The Current Level
            instance.musicAudioSource.loop = true;
            instance.musicAudioSource.clip = areaMusic;
            instance.musicAudioSource.Play();
            instance.AudioFadeIn();
        }
    }
        */

    /*
    private IEnumerator LerpVolume(float startVol, float endVol, float duration)
    {
        float currentVolume = startVol;
        float currentTime = 0.0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0.0f, duration);
            currentVolume = Mathf.Lerp(startVol, endVol, currentTime / duration);
            instance.mainMixer.SetFloat("masterVolume", currentVolume);

            yield return null;
        }

        yield return null;
    }

    public void AudioFadeIn()
    {
        instance.StartCoroutine(LerpVolume(maxVolume, minVolume, 1.5f));
    }

    public IEnumerator AudioFadeOut()
    {
        yield return LerpVolume(minVolume, maxVolume, 1.0f);
    }
    */

    /*
    public void SetMusic(int index)
    {
        if (music.Length < index) return;

        instance.StartCoroutine(AudioFadeOut());
        musicAudioSource.clip = music[index];
        musicAudioSource.enabled = true;


    }
    */
}
