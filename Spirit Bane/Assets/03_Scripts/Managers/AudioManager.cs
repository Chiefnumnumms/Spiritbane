//-------------------------------------------------------------------------
//  AudioManager Script 
//  Author:  Kevin Howell  
//  Date: January 24, 2023
//  Purpose:  Script To Handle Game Audio

using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private AudioSource effectsAudioSource;
    [SerializeField] private AudioSource ambienceAudioSource;

    [SerializeField]
    private AudioClip areaMusic;
    [SerializeField]
    private AudioClip ambience;
    [SerializeField]
    private AudioClip sfx;

    #endregion


    #region Private Members
    //-------------------------------------------------------------------------
    // Private Members

    private AudioMixerGroup master;
    private AudioMixerGroup music;
    private AudioMixerGroup effects;

    private float masterVolume;
    private float musicVolume;
    private float effectsVolume;

    private float minVolume = -16.0f;

    #endregion

    #region Private Functions
    //-------------------------------------------------------------------------
    // Private Functions

    private void UpdateMasterVolume()
    {
        instance.mainMixer.SetFloat("masterVolume", masterVolume);
        instance.mainMixer.SetFloat("musicVolume", musicVolume);
        instance.mainMixer.SetFloat("effectsVolume", effectsVolume);
    }

    private void UpdateMusicVolume()
    {
        instance.mainMixer.SetFloat("masterVolume", masterVolume);
        instance.mainMixer.SetFloat("musicVolume", musicVolume);
        instance.mainMixer.SetFloat("effectsVolume", effectsVolume);
    }

    private void UpdateEffectsVolume()
    {
        instance.mainMixer.SetFloat("masterVolume", masterVolume);
        instance.mainMixer.SetFloat("musicVolume", musicVolume);
        instance.mainMixer.SetFloat("effectsVolume", effectsVolume);
    }
    
    private void UpdateAmbienceVolume()
    {
        instance.mainMixer.SetFloat("masterVolume", masterVolume);
        instance.mainMixer.SetFloat("musicVolume", musicVolume);
        instance.mainMixer.SetFloat("effectsVolume", effectsVolume);
    }



    #endregion


    public static void LevelLoadComplete()
    {
        //AudioClip levelMusic;
        //if (GameManager.instance.CurrentLevelIndex >= 2)
        //{
            //levelMusic = LevelManager.Instance.AltMusic;
        //}
        //else
        //{
            //levelMusic = LevelManager.Instance.LevelMusic;
        //}

        //if (levelMusic)
        //{
            // Play The Level Music For The Current Level
          //  instance.musicAudioSource.loop = true;
            //instance.musicAudioSource.clip = levelMusic;
            //instance.musicAudioSource.Play();
            //instance.AudioFadeLevelStart();
        //}
    }

    private IEnumerator LerpVolume(float startVol, float endVol, float time)
    {
        float currentVolume = startVol;
        float currentTime = 0.0f;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0.0f, time);
            currentVolume = Mathf.Lerp(startVol, endVol, currentTime / time);
            instance.mainMixer.SetFloat("masterVolume", currentVolume);

            yield return null;
        }

        yield return null;
    }

    public void IntroSound()
    {
        musicAudioSource.Play();
        //fxAudioSource.Play();
    }

    public void AudioFadeLevelStart()
    {
        instance.StartCoroutine(LerpVolume(-80.0f, minVolume, 1.5f));
    }

    public IEnumerator UnloadLevel()
    {
        yield return LerpVolume(minVolume, -80.0f, 1.0f);
    }
}
