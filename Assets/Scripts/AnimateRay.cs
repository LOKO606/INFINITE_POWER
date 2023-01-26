using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateRay : MonoBehaviour
{
    float multiplier = 0;
    private LineRenderer linerende;
    void OnEnable()
    {
        linerende = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (multiplier > 0)
        {
            multiplier -= 0.03f;
            linerende.widthMultiplier = multiplier;
        }
        else
        {
            multiplier = 0;
            linerende.enabled = false;
        }
    }

    public void StartAnimation()
    {
        multiplier = 0.7f;
    }
}
