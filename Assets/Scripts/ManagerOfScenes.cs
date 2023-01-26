using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerOfScenes : MonoBehaviour
{
    public bool canResetWithR = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canResetWithR)
        {
            RestartScene();
        }
    }
    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void GoToScene(int SceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneIndex);
    }

    public void GoToNextScene()
    {
        Time.timeScale = 1;
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }

    public void GoToSelectedLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(((int)PlayerPrefs.GetFloat("Level")));
    }
}
