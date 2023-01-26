using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public PauseAllAudioSources pauser;
    public GameObject pauseCanvas;
    public bool paused = false;
    private WorldManager worldManager;
    private PlayerGun playerGun;
    private WinEffects winManager;
    private CameraMovement cameraMovement;
    void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();
        playerGun = FindObjectOfType<PlayerGun>();
        winManager = FindObjectOfType<WinEffects>();
        cameraMovement = FindObjectOfType<CameraMovement>();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !worldManager.transitioning && !winManager.wonGame)
        {
            paused = !paused;

            if (paused)
            {
                PauseGame();
            }
            else
            {
                Unpause();
            }
        }
    }
    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauser.PauseAllNow();
        playerGun.canShoot = false;
        cameraMovement.canMove = false;
    }

    public void Unpause()
    {
        paused = false;
        pauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pauser.UnpauseAudio();
        playerGun.canShoot = true;
        cameraMovement.canMove = true;
    }
}
