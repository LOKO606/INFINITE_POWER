using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolumes : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
    void Awake()
    {
        float musicVolume = PlayerPrefs.GetFloat("Music");
        float sfxVolume = PlayerPrefs.GetFloat("SFX");
        if (musicVolume <= 0)
        {
            musicVolume = 0.00001f;
        }
        if (sfxVolume <= 0)
        {
            sfxVolume = 0.00001f;
        }
        musicMixer.SetFloat("MasterVolume", 20f * Mathf.Log10(musicVolume));
        sfxMixer.SetFloat("MasterVolume", 20f * Mathf.Log10(sfxVolume));
    }
}
