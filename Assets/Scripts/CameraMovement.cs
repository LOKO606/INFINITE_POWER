using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensivity;
    private float totalRotationX, totalRotationY;

    public Transform orientator;
    private PauseManager pauseManager;
    public bool canMove = true;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseManager = FindObjectOfType<PauseManager>();
        sensivity = PlayerPrefs.GetFloat("Sens");
    }

    void Update()
    {
        if (canMove)
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        totalRotationY += Input.GetAxisRaw("Mouse X") * sensivity;
        totalRotationX -= Input.GetAxisRaw("Mouse Y") * sensivity;

        totalRotationX = Mathf.Clamp(totalRotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(totalRotationX, totalRotationY, 0);
        orientator.localRotation = Quaternion.Euler(0, totalRotationY, 0);
    }
}
