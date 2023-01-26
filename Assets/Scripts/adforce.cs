using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adforce : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, 10);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if(rb.gameObject.tag == "Player"){
                        rb.gameObject.GetComponent<PlayerControllerFloaty>().DisableSpringAfterExplosion();
                    }
                    rb.AddExplosionForce(1800, explosionPos, 10, 3.0F);
                }

            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4);
    }
}
