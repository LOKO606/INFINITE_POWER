using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowOnSpawn : MonoBehaviour
{
    public float targetSizeAllAxis;
    void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * targetSizeAllAxis, Time.deltaTime * 2f);
    }
}
