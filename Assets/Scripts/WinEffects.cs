using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class WinEffects : MonoBehaviour
{
    public GameObject winOptions;
    public GameObject winCanvas;
    public AudioSource INFINITEaudio;
    public AudioSource POWERaudio;
    public AudioSource winExplosinoAudio;
    public AudioSource winMusic;
    public ParticleSystem particleSystemWin;
    public TextMeshProUGUI onScreenText;
    public TextMeshPro winText;
    public BoxCollider winCol;
    public BoxCollider loseCol;
    public bool wonGame;

    public void DoWinEffects()
    {
        FindObjectOfType<PauseAllAudioSources>().PauseAllNow();
        FindObjectOfType<PlayerGun>().enabled = false;
        FindObjectOfType<WorldManager>().enabled = false;
        FindObjectOfType<GameOverManager>().canDie = false;

        winMusic.Play();
        winExplosinoAudio.Play();

        winCanvas.SetActive(true);

        particleSystemWin.Play();

        FindObjectOfType<CameraShake>().ShakeCamera(2f, 2f, 2f, 5f, 90, 90, 40); ;

        PlayerControllerFloaty controller = FindObjectOfType<PlayerControllerFloaty>();
        controller.SetSlideState(false);
        controller.disabledSpring = true;
        controller.enabled = false;
        Rigidbody playerRB = FindObjectOfType<PlayerControllerFloaty>().GetComponent<Rigidbody>();
        playerRB.velocity = Vector3.zero;
        playerRB.AddForce(new Vector3(0, 1500, 0));
        playerRB.AddForce(-1000 * Camera.main.transform.forward);

        FindObjectOfType<Battery>().WinGame();

        StartCoroutine(InfinitePower());

        wonGame = true;

        int index = SceneManager.GetActiveScene().buildIndex + 1;

        if (index != 0 && index != 22)
        {
            PlayerPrefs.SetFloat("Level", index);
        }

        Invoke(nameof(makeButtonsAppear), 2.5f);
    }

    void makeButtonsAppear()
    {
        winOptions.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void DoLoseEffects()
    {
        GameObject.FindObjectOfType<GameOverManager>().Die();
        winText.text = "BRUH";
    }

    public void Invert()
    {
        winText.text = "LOSE";
        winCol.enabled = false;
        loseCol.enabled = true;
    }
    public void Default()
    {
        winText.text = "WIN";
        winCol.enabled = true;
        loseCol.enabled = false;
    }

    IEnumerator InfinitePower()
    {
        INFINITEaudio.Play();
        onScreenText.text = "INFINITE";
        yield return new WaitForSecondsRealtime(1.5f);
        POWERaudio.Play();
        onScreenText.text = "POWER";
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(InfinitePower());
    }
}
