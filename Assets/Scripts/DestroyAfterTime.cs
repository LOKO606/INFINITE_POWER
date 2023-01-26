using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public void DestroyAfterThisTime(float time)
    {
        Invoke(nameof(DestroyNow), time);
    }

    private void DestroyNow()
    {
        Destroy(gameObject);
    }
}
