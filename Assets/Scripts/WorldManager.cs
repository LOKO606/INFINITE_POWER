using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WorldManager : MonoBehaviour
{
    public AudioSource warpSound;
    public AudioSource defaultMusic;
    public AudioSource invertedMusic;
    public bool worldInverted = false;
    public GameObject defaultLoationIndicator;
    public float delayBetweenWarps = 3;

    [Header("Default")]
    public Material defaultSkyBox;
    public float batteryDecayRate = -0.002f;

    [Header("Inverted")]
    public Material invertedSkyBox;
    public float batteryChargeRate = 0.002f;

    private Battery battery;
    private GameObject playerObject;
    private Vector3 playerDefaultLocation = new Vector3(0, 3, 0);
    private GameObject indicatorObject;
    private HudManager hudManager;
    private EnemyManager enemyManager;
    private PlayerGun playerGun;
    private WinEffects winManager;
    private ChangeMaterial changeMaterial;
    public bool canWarp = true;
    private Camera mainCamera;


    private float cameraFovTarget;
    [HideInInspector] public bool transitioning;

    private GameObject[] Unstables;
    void Start()
    {
        battery = GameObject.FindObjectOfType<Battery>();
        playerObject = GameObject.FindObjectOfType<PlayerControllerFloaty>().gameObject;
        hudManager = GameObject.FindObjectOfType<HudManager>();
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        playerGun = GameObject.FindObjectOfType<PlayerGun>();
        winManager = GameObject.FindObjectOfType<WinEffects>();
        changeMaterial = GameObject.FindObjectOfType<ChangeMaterial>();
        playerDefaultLocation = playerObject.transform.position;
        Unstables = GameObject.FindGameObjectsWithTag("Unstable");


        mainCamera = Camera.main;
        DefaultWorldChanges();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWorldState();
        }

        if (transitioning)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraFovTarget, Time.unscaledDeltaTime * 7f);
        }
        else
        {
            mainCamera.fieldOfView = 95;
        }
    }

    public void ChangeWorldState()
    {
        if (canWarp)
        {
            worldInverted = !worldInverted;
            canWarp = false;
            Invoke(nameof(canWarpAgain), delayBetweenWarps);
            hudManager.warpEnabled = false;
            StartCoroutine(Transition(worldInverted));
        }
    }
    void canWarpAgain()
    {
        hudManager.warpEnabled = true;
        canWarp = true;
    }
    IEnumerator Transition(bool inverted)
    {
        Time.timeScale = 0;
        transitioning = true;
        cameraFovTarget = 180;

        warpSound.Play();

        playerGun.canShoot = false;
        if (!inverted)
        {
            playerGun.HideGun();
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (inverted)
        {
            InvertedWorldChanges();
        }
        else
        {
            DefaultWorldChanges();
        }
        playerGun.canShoot = false;
        cameraFovTarget = 95;

        yield return new WaitForSecondsRealtime(0.5f);

        transitioning = false;
        Time.timeScale = 1;

        if (inverted)
        {
            playerGun.canShoot = true;
        }
    }

    void DefaultWorldChanges()
    {
        invertedMusic.Stop();
        defaultMusic.Play();
        RenderSettings.skybox = defaultSkyBox;
        battery.chargeRate = batteryDecayRate;
        playerObject.transform.position = playerDefaultLocation;
        DynamicGI.UpdateEnvironment();
        Destroy(indicatorObject);
        enemyManager.DestroyEnemies();
        playerGun.DisableGun();
        winManager.Default();
        playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        changeMaterial.SetDefault();
        SetUnstable(true);
    }

    void InvertedWorldChanges()
    {
        invertedMusic.Play();
        defaultMusic.Stop();
        RenderSettings.skybox = invertedSkyBox;
        battery.chargeRate = batteryChargeRate;
        playerDefaultLocation = playerObject.transform.position;
        DynamicGI.UpdateEnvironment();
        indicatorObject = Instantiate(defaultLoationIndicator, playerObject.transform.position, Quaternion.identity);
        enemyManager.CreateEnemies();
        playerGun.EnableGun();
        playerGun.SpinGun();
        winManager.Invert();
        changeMaterial.SetInvert();
        SetUnstable(false);
    }


    void SetUnstable(bool state)
    {
        for (int i = 0; i < Unstables.Length; i++)
        {
            Unstables[i].SetActive(state);
        }
    }
}
