using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToPlayer : MonoBehaviour
{
    private Transform target;
    void OnEnable()
    {
        target = FindObjectOfType<PlayerControllerFloaty>().gameObject.transform;
    }
    void Update()
    {
        transform.LookAt(target);
    }
}
