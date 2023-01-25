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
    [SerializeField]
    private AudioMixer mainMixer;

    [SerializeField]
    private AudioSource musicAudioSource;

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

}
