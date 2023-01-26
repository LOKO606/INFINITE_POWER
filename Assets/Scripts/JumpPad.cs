using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public AudioSource up, fwd;
    PlayerControllerFloaty controller;
    Rigidbody playerRigid;
    public float verticalBoost;
    bool canBoost = true;
    void Start()
    {
        controller = FindObjectOfType<PlayerControllerFloaty>();
        playerRigid = controller.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canBoost = false;
            Invoke(nameof(canBoostAgain), 1f);
            controller.DisableSpringAfterExplosion();
            if (controller.isDashing)
            {
                playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);
                playerRigid.AddForce(Vector3.up * verticalBoost * 0.55f);
                controller.isDashing = false;
                Invoke(nameof(enableDash), controller.dashDelay);
                fwd.Play();
            }
            else
            {
                playerRigid.velocity = new Vector3(0, 0, 0);
                playerRigid.AddForce(Vector3.up * verticalBoost);
                controller.SetSlideState(false);
                up.Play();
                Invoke(nameof(enableDash), controller.dashDelay);
            }
        }
    }

    void canBoostAgain()
    {
        canBoost = true;
    }

    void enableDash()
    {
        controller.canDash = true;
        controller.dashesDone = 0;
    }
}
