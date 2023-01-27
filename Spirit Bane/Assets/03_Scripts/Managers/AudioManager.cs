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
    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private AudioSource musicAudioSource;    
    [SerializeField] private AudioSource fxAudioSource;
    
    private AudioMixerGroup master;
    private AudioMixerGroup music;
    private AudioMixerGroup effects;

    private float minVolume = -16.0f;

    private float masterVolume;
    public float MasterVolume { get; set; }
    private float musicVolume;
    public float MusicVolume { get; set; }
    private float effectsVolume;
    public float EffectsVolume { get; set; }

    void Update()
    {
        instance.mainMixer.SetFloat("masterVolume", masterVolume);
        instance.mainMixer.SetFloat("musicVolume", musicVolume);
        instance.mainMixer.SetFloat("effectsVolume", effectsVolume);
    }

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
