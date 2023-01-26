using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    private bool doShake = false;
    private float shakeMagnitude;
    private float shakeDuration;

    float rotationX = 0;
    float rotationY = 0;
    float rotationZ = 0;

    float maxAngleX = 20;
    float maxAngleY = 20;
    float maxAngleZ = 20;

    float intensity = 0;
    float growthIntensity = 1;
    float decayIntensity = 1;

    float seedX;
    float seedY;
    float seedZ;

    float speed;

    void Start()
    {
        seedX = Random.Range(-1000, 1000);
        seedY = Random.Range(-1000, 1000);
        seedZ = Random.Range(-1000, 1000);
    }

    public void ShakeCamera(float maxAngleX, float maxAngleY, float maxAngleZ, float duration, float growthIntensity, float decayIntensity, float speed)
    {
        this.maxAngleX = maxAngleX;
        this.maxAngleY = maxAngleY;
        this.maxAngleZ = maxAngleZ;
        this.growthIntensity = growthIntensity;
        this.decayIntensity = decayIntensity;
        this.speed = speed;

        doShake = true;
        Invoke(nameof(StopShake), duration);
    }

    void StopShake()
    {
        doShake = false;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        if (doShake)
        {
            intensity += growthIntensity * deltaTime;
        }
        else
        {
            intensity -= decayIntensity * deltaTime;
        }

        intensity = Mathf.Clamp(intensity, 0, 1);

        float intensityExponential = intensity * intensity;

        float timeVariation = Time.time * speed;
        rotationX = intensityExponential * maxAngleX * ReturnPerlinNoise(seedX, timeVariation);
        rotationY = intensityExponential * maxAngleY * ReturnPerlinNoise(seedY, timeVariation);
        rotationZ = intensityExponential * maxAngleZ * ReturnPerlinNoise(seedZ, timeVariation);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
    }

    float ReturnPerlinNoise(float seed, float time)
    {
        return (1 - 2 * Mathf.PerlinNoise(seed + time, seed + time));
    }
}
