using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public AudioSource ChargingSound;
    public AudioSource ShootingSound;
    public Animator animatorEnemy;
    public ParticleSystem chargeParticle;
    public ParticleSystem shootParticle;
    public Transform[] guns;
    public float aimingTime, lockTime;
    public Transform target;
    public bool aim = true;
    public LayerMask maskToShoot;
    public LayerMask maskThatBlocks;
    public float gunRange = 100;
    public float aimSpeed = 6;
    public float batteryDischargeByShoot = -0.2f;
    private Battery battery;
    private bool isInRange, shooting;
    private bool aimBlocked;
    private bool didFirstShoot;
    void OnEnable()
    {
        battery = FindObjectOfType<Battery>();
        target = GameObject.Find("TargetForEnemies").transform;
    }

    void Update()
    {
        isInRange = Vector3.Distance(target.position, transform.position) <= gunRange;
        aimBlocked = Physics.Linecast(transform.position, target.position, maskThatBlocks);

        if (aim && isInRange)
        {
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
        }

        if (!aimBlocked && isInRange && !shooting)
        {
            StartCoroutine(GunLockRaycastType());
        }

    }
    IEnumerator GunLockRaycastType()
    {
        shooting = true;
        aim = true;
        float aimTime = aimingTime;
        if (!didFirstShoot)
        {
            didFirstShoot = true;
            aimTime += Random.Range(0, 25) * 0.1f;
        }
        yield return new WaitForSeconds(aimTime);
        chargeParticle.Play();
        ChargingSound.Play();
        yield return new WaitForSeconds(1f);
        chargeParticle.Stop();
        yield return new WaitForSeconds(0.5f);
        ShootingSound.Play();
        shootParticle.Play();
        animatorEnemy.Play("recoil");
        aim = false;
        Shoot();
        yield return new WaitForSeconds(lockTime);
        shooting = false;
    }

    void Shoot()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            RaycastHit hit;

            LineRenderer lineRender = guns[i].GetComponent<LineRenderer>();

            lineRender.enabled = true;

            if (Physics.Raycast(guns[i].position, guns[i].forward, out hit, gunRange, maskToShoot))
            {
                DrawGunRay(guns[i].position, hit.point, ref lineRender);
                if (hit.transform.tag == "Player")
                {
                    battery.ChargeBattery(batteryDischargeByShoot);
                    battery.EnemyDamagedBattery();
                }
            }
            else
            {
                DrawGunRay(guns[i].position, guns[i].transform.position + guns[i].transform.forward * gunRange, ref lineRender);
            }

            lineRender.gameObject.GetComponent<AnimateRay>().StartAnimation();
        }
    }

    void DrawGunRay(Vector3 startPos, Vector3 endPos, ref LineRenderer lineRender)
    {
        lineRender.SetPosition(0, startPos);
        lineRender.SetPosition(1, endPos);
        lineRender.widthMultiplier = 0.5f;
    }


}
