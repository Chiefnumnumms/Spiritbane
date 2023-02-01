//-------------------------------------------------------------------------
//  AudioManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Game Audio

using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//-------------------------------------------------------------------------
// This Class Represents The Audio Manager
public class AudioManager : Singleton<AudioManager>
{
    #region Public Members
    //-------------------------------------------------------------------------
    // Public Members

    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float EffectsVolume { get; set; }
    public float AmbienceVolume { get; set; }

    public AudioClip AreaMusic { get { return instance.areaMusic; } }
    public AudioClip Ambience { get { return instance.ambience; } }
    public AudioClip SoundEffects { get { return instance.sfx; } }

    #endregion


    #region Editor Access Members
    //-------------------------------------------------------------------------
    // Editor Access Members

    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource ambienceAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField] private AudioClip[] music; //List<AudioClip> music = new List<AudioClip>();

    #endregion


    #region Private Members
    //-------------------------------------------------------------------------
    // Private Members

    //private AudioMixerGroup masterMixerGroup;
    //private AudioMixerGroup musicMixerGroup;
    //private AudioMixerGroup ambienceMixerGroup;
    //private AudioMixerGroup sfxMixerGroup;

    private AudioClip areaMusic;
    private AudioClip ambience;
    private AudioClip sfx;

    private float masterVolume;
    private float musicVolume;
    private float ambienceVolume;
    private float sfxVolume;

    private float minVolume = -16.0f;

    #endregion

    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    private void UpdateMasterVolume()
    {
        instance.mainMixer.SetFloat("masterVolume", masterVolume);
    }

    private void UpdateMusicVolume()
    {
        instance.mainMixer.SetFloat("musicVolume", musicVolume);
    }

    private void UpdateSFXVolume()
    {
        instance.mainMixer.SetFloat("effectsVolume", sfxVolume);
    }
    
    private void UpdateAmbienceVolume()
    {
        instance.mainMixer.SetFloat("ambienceVolume", ambienceVolume);
    }

    #endregion


    public void LoadLevelMusic()
    {
        int areaIndex = (int)ScenesManager.instance.CurrentScene.Value;
        areaMusic = music[areaIndex];

        if (areaMusic)
        {
            // Play The Level Music For The Current Level
            instance.musicAudioSource.loop = true;
            instance.musicAudioSource.clip = areaMusic;
            instance.musicAudioSource.Play();
            instance.AudioFadeIn();
        }
    }

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
        instance.StartCoroutine(LerpVolume(-80.0f, minVolume, 1.5f));
    }

    public IEnumerator AudioFadeOut()
    {
        yield return LerpVolume(minVolume, -80.0f, 1.0f);
    }
}
