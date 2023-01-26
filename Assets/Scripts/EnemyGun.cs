using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public float aimingTime, lockTime;
    private Transform target;

    private bool aim;
    void Start()
    {
        target = GetComponentInParent<FlyingEnemy>().target;
    }

    void OnEnable()
    {
        StartCoroutine(GunLockRaycastType());
    }

    void Update()
    {
        if (aim)
        {
            transform.LookAt(target);
        }
    }
    IEnumerator GunLockRaycastType()
    {
        aim = true;
        yield return new WaitForSeconds(aimingTime);
        aim = false;
        yield return new WaitForSeconds(lockTime);
        Shoot();
        StartCoroutine(GunLockRaycastType());
    }

    void Shoot()
    {

    }
}
