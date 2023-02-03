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

    public float MasterVolume { get { return masterVolume; } set { masterVolume = value; } }
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
    public float AmbienceVolume { get { return ambienceVolume; } set { ambienceVolume = value; } }
    public float SFXVolume { get { return sfxVolume; } set { sfxVolume = value; } }

    public AudioClip[] Music { get { return instance.music; } }
    public AudioClip[] Ambience { get { return instance.ambience; } }
    public AudioClip[] SFX { get { return instance.sfx; } }

    #endregion


    #region Editor Access Members
    //-------------------------------------------------------------------------
    // Editor Access Members

    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource ambienceAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField]
    private AudioClip[] music;
    [SerializeField]
    private AudioClip[] ambience;
    [SerializeField]
    private AudioClip[] sfx;

    #endregion


    #region Private Members
    //-------------------------------------------------------------------------
    // Private Members

    private AudioMixerGroup masterGroup;
    private AudioMixerGroup musicGroup;
    private AudioMixerGroup ambienceGroup;
    private AudioMixerGroup sfxGroup;

    private float masterVolume;
    private float musicVolume;
    private float ambienceVolume;
    private float sfxVolume;

    private float minVolume = -16.0f;
    private float maxVolume = -80.0f;

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
    private void UpdateAmbienceVolume()
    {
        instance.mainMixer.SetFloat("ambienceVolume", ambienceVolume);
    }

    private void UpdateEffectsVolume()
    {
        instance.mainMixer.SetFloat("sfxVolume", sfxVolume);
    }
    
    #endregion


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
