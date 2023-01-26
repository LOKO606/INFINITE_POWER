using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public bool startWithGunDisabled = true;
    public float gunDamage, gunCadencyDelay, gunRange;
    public AudioSource hitEnemySound;
    public AudioSource ShootSound;
    public CameraShake cameraShake;
    public GameObject gunObject;
    public LineRenderer lineRenderer;
    public ParticleSystem particleSystemGun;
    public Transform rayOrigin;
    public Transform lineRendererOrigin;
    public LayerMask layerToShoot;
    public Animator animatorOfGun;
    private RaycastHit hit;
    private Battery battery;
    float multiplier;
    public bool canShoot = true, delayEnded = false;
    float highestBeamSize = 0.35f;

    void Start()
    {
        if (startWithGunDisabled)
        {
            gunObject.SetActive(false);
        }

        battery = FindObjectOfType<Battery>();

        canShoot = false;

    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && delayEnded)
        {
            Shoot();
            delayEnded = false;
            Invoke(nameof(PassDelay), gunCadencyDelay);
        }
    }

    void PassDelay()
    {
        delayEnded = true;
    }

    void FixedUpdate()
    {
        if (multiplier > 0)
        {
            multiplier -= 0.02f;
            lineRenderer.widthMultiplier = multiplier;
        }
        else
        {
            multiplier = 0;
        }

    }

    public void DisableGun()
    {
        gunObject.SetActive(false);
        canShoot = false;
        delayEnded = false;
        CancelInvoke();
    }
    public void EnableGun()
    {
        gunObject.SetActive(true);
        canShoot = true;
        delayEnded = true;
    }
    public void SpinGun()
    {
        animatorOfGun.Play("SpinGun", 0, 0);
    }
    public void HideGun()
    {
        animatorOfGun.Play("HideGun", 0, 0);
    }
    void Shoot()
    {
        cameraShake.ShakeCamera(0.77f, 0.77f, 0.77f, 0.1f, 90, 90, 40);
        animatorOfGun.Play("Shoot", 0, 0);
        ShootSound.Play();
        particleSystemGun.Stop();
        particleSystemGun.Play();
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, gunRange, layerToShoot))
        {
            lineRenderer.SetPosition(0, lineRendererOrigin.position);
            lineRenderer.SetPosition(1, hit.point);
            multiplier = highestBeamSize;

            if (hit.transform.tag == "Enemy")
            {
                hitEnemySound.Play();
                bool died = hit.transform.gameObject.GetComponentInParent<EnemyHealth>().ReceiveDamage(gunDamage);
                if (died)
                {
                    battery.ChargeBattery(0.1f);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, lineRendererOrigin.position);
            lineRenderer.SetPosition(1, lineRendererOrigin.position + lineRendererOrigin.forward * gunRange);
            multiplier = highestBeamSize;
        }
    }
}
