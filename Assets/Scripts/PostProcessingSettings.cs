using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
public class PostProcessingSettings : MonoBehaviour
{
    Volume postProcessVolume;
    void Start()
    {
        postProcessVolume = FindObjectOfType<Volume>();
        ChangeStateOfPostProcess(Convert.ToBoolean(PlayerPrefs.GetFloat("PostProcess")));
    }
    public void ChangeStateOfPostProcess(bool state)
    {
        postProcessVolume.enabled = state;
    }
}
