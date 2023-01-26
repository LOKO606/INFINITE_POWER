using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameOverManager : MonoBehaviour
{
    public AudioSource chargeDepleted;
    public AudioSource diedMusic;
    public bool died = false;
    public GameObject canvasGameOver;

    public Material playerMaterial;
    [ColorUsage(true, true)]
    public Color onColor;
    [ColorUsage(true, true)]
    public Color offColor;
    public bool canDie = true;

    void Start()
    {
        canvasGameOver.SetActive(false);
        playerMaterial.SetColor("_EmissionColor", onColor);
    }
    public void Die()
    {
        if (!died && canDie)
        {
            FindObjectOfType<PauseAllAudioSources>().PauseAllNow();
            chargeDepleted.Play();
            diedMusic.Play();
            died = true;
            PlayerControllerFloaty controller = FindObjectOfType<PlayerControllerFloaty>();
            controller.SetSlideState(false);
            controller.enabled = false;
            FindObjectOfType<CameraMovement>().enabled = false;
            FindObjectOfType<TiltCamera>().enabled = false;
            canvasGameOver.SetActive(true);
            FindObjectOfType<PlayerGun>().enabled = false;
            FindObjectOfType<WorldManager>().enabled = false;
            FindObjectOfType<Battery>().ChargeBattery(-99);
            playerMaterial.SetColor("_EmissionColor", offColor);
        }
    }
}
