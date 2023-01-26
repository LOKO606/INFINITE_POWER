using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Battery : MonoBehaviour
{
    private CameraShake camShake;
    public AudioSource gotDamagedSound;
    public TextMeshPro percentage;
    public TextMeshPro batteryFace;
    public Transform batteryEnergyTransform;
    public float charge = 1;
    public float chargeRate = -0.002f;
    bool loseCharge;
    private Vector3 originalSize;
    private MeshRenderer batteryRenderer;
    public Material defaultBatteryMaterial, damagedBatteryMaterial;
    private bool damaged = false;
    void Start()
    {
        loseCharge = true;
        originalSize = batteryEnergyTransform.localScale;
        camShake = FindObjectOfType<CameraShake>();
        batteryRenderer = batteryEnergyTransform.GetComponent<MeshRenderer>();
        setDefaultBatteryMaterial();
    }

    void FixedUpdate()
    {
        if (loseCharge)
        {
            charge += chargeRate;
        }

        charge = Mathf.Clamp(charge, -0.01f, 1);

        if (charge < 0)
        {
            percentage.text = "0";
        }
        else
        {
            percentage.text = (100 * charge).ToString("F0");
        }

        if (!damaged)
        {
            if (charge > 0.60f)
            {
                batteryFace.text = ":)";
            }
            else if (charge > 0.3f)
            {
                batteryFace.text = ":|";
            }
            else if (charge > 0.0f)
            {
                batteryFace.text = ":(";
            }
            else
            {
                batteryFace.text = "X(";
            }
        }
        batteryEnergyTransform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z * charge);

        CheckDead();
    }

    void CheckDead()
    {
        if (charge <= 0)
        {
            GameObject.FindObjectOfType<GameOverManager>().Die();
            loseCharge = false;
        }
    }

    public void ChargeBattery(float chargeQuantity)
    {
        charge = Mathf.Clamp(charge + chargeQuantity, -0.5f, 1);
    }

    public void EnemyDamagedBattery()
    {
        damaged = true;
        gotDamagedSound.Play();
        batteryFace.text = ":C";
        camShake.ShakeCamera(1f, 1f, 1f, 0.35f, 90, 90, 40);
        batteryRenderer.material = damagedBatteryMaterial;
        Invoke(nameof(setDefaultBatteryMaterial), 0.3f);
    }

    public void WinGame()
    {
        batteryEnergyTransform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z);
        batteryFace.fontSize = 18;
        batteryFace.text = "XD";
        percentage.fontSize = 26;
        percentage.text = "∞";
        this.enabled = false;
    }

    void setDefaultBatteryMaterial()
    {
        batteryRenderer.material = defaultBatteryMaterial;
        damaged = false;
    }
}
