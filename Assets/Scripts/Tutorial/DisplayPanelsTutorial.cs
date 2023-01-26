using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPanelsTutorial : MonoBehaviour
{
    public PauseAllAudioSources pauser;
    public Battery battery;
    public GameObject theCanvas;
    public GameObject firstMessage, secondMessage, thirdMessage;
    bool pressetE = false;
    bool firstMessageShowed;
    private PlayerGun playerGun;
    bool secondMessageSent;
    bool closed2Message;
    bool send3Message;
    private GameOverManager gameOverManager;
    private WorldManager worldManager;
    private PauseManager pauseManager;
    private CameraMovement cameraMovement;
    void Start()
    {
        battery = FindObjectOfType<Battery>();
        pauser = FindObjectOfType<PauseAllAudioSources>();
        playerGun = FindObjectOfType<PlayerGun>();
        gameOverManager = FindObjectOfType<GameOverManager>();
        worldManager = FindObjectOfType<WorldManager>();
        pauseManager = FindObjectOfType<PauseManager>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        worldManager.canWarp = false;
    }

    void Update()
    {
        if (!gameOverManager.died)
        {
            if (battery.charge <= 0.3f && !firstMessageShowed)
            {
                EnableMessage(firstMessage);
                firstMessageShowed = true;
            }

            if (firstMessageShowed && Input.GetKeyDown(KeyCode.Q) && !secondMessageSent)
            {
                EndMessage(firstMessage);
                secondMessageSent = true;
                Invoke(nameof(EnableSecondMessage), 2f);
                worldManager.ChangeWorldState();
            }

            if (closed2Message && !send3Message)
            {
                send3Message = true;
                Invoke(nameof(EnableThirdMessage), 7);
            }
        }
        else
        {
            CancelInvoke();
            Destroy(gameObject);
        }
    }

    void EnableSecondMessage()
    {
        EnableMessage(secondMessage);
    }

    public void End2Message()
    {
        EndMessage(secondMessage);
        closed2Message = true;
    }

    void EnableThirdMessage()
    {
        EnableMessage(thirdMessage);
    }

    public void End3Message()
    {
        EndMessage(thirdMessage);
    }

    void EnableMessage(GameObject message)
    {
        pauser.PauseAllNow();
        theCanvas.SetActive(true);
        message.SetActive(true);

        Time.timeScale = 0;
        playerGun.canShoot = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseManager.enabled = false;
        worldManager.canWarp = false;
        cameraMovement.canMove = false;
    }

    public void EndMessage(GameObject message)
    {
        theCanvas.SetActive(false);
        message.SetActive(false);
        pauser.UnpauseAudio();

        Time.timeScale = 1;
        playerGun.canShoot = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseManager.enabled = true;
        worldManager.canWarp = true;
        cameraMovement.canMove = true;
    }
}
