using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MenuOptions : MonoBehaviour
{
    public GameObject SettingsCanva;
    public Button settingsButton;
    public void OpenSettings()
    {
        SettingsCanva.SetActive(true);
        settingsButton.enabled = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseSettings()
    {
        SettingsCanva.SetActive(false);
        settingsButton.enabled = true;
    }
}
