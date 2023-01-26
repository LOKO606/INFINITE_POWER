using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltCamera : MonoBehaviour
{
    public float tiltMaxAngle = 3.5f, tiltSpeed = 0.17f;
    private float tiltLerpInterpolation, lastInput, currentTiltValue;

    //comes after CameraShake in script execution order
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != lastInput)
        {
            tiltLerpInterpolation = 0;
        }
        else if (tiltLerpInterpolation < 1)
        {
            tiltLerpInterpolation += tiltSpeed;
        }

        lastInput = horizontalInput;

        float tiltLerpValueTarget = -tiltMaxAngle * horizontalInput;

        currentTiltValue = Mathf.Lerp(currentTiltValue, tiltLerpValueTarget, tiltLerpInterpolation);

        transform.localRotation = Quaternion.Euler(0, 0, currentTiltValue);
    }

}
