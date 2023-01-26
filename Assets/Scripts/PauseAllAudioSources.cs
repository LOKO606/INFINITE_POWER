using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAllAudioSources : MonoBehaviour
{
    private List<AudioSource> audioSourcesThatWherePlaying = new List<AudioSource>();
    public void PauseAllNow()
    {
        audioSourcesThatWherePlaying.Clear();
        List<AudioSource> allAudioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
        for (int i = 0; i < allAudioSources.Count; i++)
        {
            if (allAudioSources[i].isPlaying)
            {
                audioSourcesThatWherePlaying.Add(allAudioSources[i]);
            }
        }

        for (int i = 0; i < audioSourcesThatWherePlaying.Count; i++)
        {
            audioSourcesThatWherePlaying[i].Pause();
        }
    }

    public void UnpauseAudio()
    {
        for (int i = 0; i < audioSourcesThatWherePlaying.Count; i++)
        {
            audioSourcesThatWherePlaying[i].Play();
        }
    }
}
