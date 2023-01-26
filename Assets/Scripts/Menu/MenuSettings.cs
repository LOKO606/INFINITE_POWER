using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Rendering;
public class MenuSettings : MonoBehaviour
{
    public Slider musicSlider;
    public TextMeshProUGUI musicText;
    public Slider sfxSlider;
    public TextMeshProUGUI sfxText;
    public Slider sensSlider;
    public TextMeshProUGUI sensText;
    public Slider screenSlider;
    public TextMeshProUGUI screenText;
    public Slider levelSlider;
    public TextMeshProUGUI levelText;
    public Toggle postProcessToggle;
    float musicValue;
    float sfxValue;
    float sensValue;
    float screenValue;
    float levelValue;
    float postProcessValue;

    void Start()
    {
        float valueScreen = PlayerPrefs.GetFloat("ScreenMode");
        if (valueScreen == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (valueScreen == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (valueScreen == 2)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        levelSlider.maxValue = 21;
        levelSlider.minValue = 1;
    }

    public void GetPrefsToValues()
    {
        sfxValue = PlayerPrefs.GetFloat("SFX");
        musicValue = PlayerPrefs.GetFloat("Music");
        sensValue = PlayerPrefs.GetFloat("Sens");
        screenValue = PlayerPrefs.GetFloat("ScreenMode");
        levelValue = PlayerPrefs.GetFloat("Level");
        postProcessValue = PlayerPrefs.GetFloat("PostProcess");

        musicSlider.value = PlayerPrefs.GetFloat("Music");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        sensSlider.value = PlayerPrefs.GetFloat("Sens");
        screenSlider.value = PlayerPrefs.GetFloat("ScreenMode");
        levelSlider.value = PlayerPrefs.GetFloat("Level");
        postProcessToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetFloat("PostProcess"));

        UpdateValueText();
    }
    public void UpdateValues()
    {
        musicValue = musicSlider.value;
        sfxValue = sfxSlider.value;
        sensValue = sensSlider.value;
        screenValue = screenSlider.value;
        levelValue = levelSlider.value;
        postProcessValue = Convert.ToSingle(PlayerPrefs.GetFloat("PostProcess"));
        UpdateValueText();
    }
    public void UpdateValueText()
    {
        musicText.text = (musicValue * 100).ToString("F2");
        sfxText.text = (sfxValue * 100).ToString("F2");
        sensText.text = sensValue.ToString("F2");
        if (screenValue == 0)
        {
            screenText.text = "Fullscreen";
        }
        else if (screenValue == 1)
        {
            screenText.text = "Windowed Fullscreen";
        }
        else if (screenValue == 2)
        {
            screenText.text = "Windowed";
        }
        levelText.text = levelValue.ToString("F0");
    }
    public void SavePrefs()
    {
        PlayerPrefs.SetFloat("SFX", sfxValue);
        PlayerPrefs.SetFloat("Music", musicValue);
        PlayerPrefs.SetFloat("Sens", sensValue);
        PlayerPrefs.SetFloat("ScreenMode", screenValue);
        if (screenValue == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (screenValue == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (screenValue == 2)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        PlayerPrefs.SetFloat("Level", levelValue);
        PlayerPrefs.SetFloat("PostProcess", Convert.ToSingle(postProcessToggle.isOn));
        FindObjectOfType<Volume>().enabled = postProcessToggle.isOn;
    }

    public void SetDefaultValues()
    {
        float defaultMusic = 0.8f;
        float defaultSFX = 1f;
        float defaultScreen = 0f;
        float defaultSens = 2f;
        float defaultLevel = 1;
        float defaultPostProcess = 1;

        musicValue = defaultMusic;
        sfxValue = defaultSFX;
        screenValue = defaultScreen;
        sensValue = defaultSens;
        levelValue = defaultLevel;
        postProcessValue = defaultPostProcess;

        musicSlider.value = defaultMusic;
        sfxSlider.value = defaultSFX;
        sensSlider.value = defaultSens;
        screenSlider.value = defaultScreen;
        levelSlider.value = defaultLevel;
        postProcessToggle.isOn = Convert.ToBoolean(defaultPostProcess);

        if (screenValue == 0)
        {
            screenText.text = "Fullscreen";
        }
        else if (screenValue == 1)
        {
            screenText.text = "Windowed Fullscreen";
        }
        else if (screenValue == 2)
        {
            screenText.text = "Windowed";
        }
    }

}
