using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeOpeningGame : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("AlreadyOpened"))
        {
            PlayerPrefs.SetInt("AlreadyOpened", 1);
            FirstTimeSetUp();
        }
    }
    void FirstTimeSetUp()
    {
        PlayerPrefs.SetFloat("SFX", 1);
        PlayerPrefs.SetFloat("Music", 0.8f);
        PlayerPrefs.SetFloat("Sens", 2);
        //0 = full screen, 1 = window full screen, 2 = window
        PlayerPrefs.SetFloat("ScreenMode", 0);
        PlayerPrefs.SetFloat("Level", 1);
        PlayerPrefs.SetFloat("PostProcess", 1);
    }

}
