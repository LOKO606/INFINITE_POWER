using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedViewSize : MonoBehaviour
{
    private Transform target;
    public Vector3 fixedSize;
    void OnEnable()
    {
        target = FindObjectOfType<PlayerControllerFloaty>().gameObject.transform;
        if (fixedSize == Vector3.zero)
        {
            fixedSize = transform.localScale;
        }
    }
    void Update()
    {
        transform.localScale = fixedSize * Vector3.Distance(transform.position, target.position);
    }
}
