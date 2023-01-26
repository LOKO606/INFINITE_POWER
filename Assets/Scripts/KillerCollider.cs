using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !FindObjectOfType<GameOverManager>().died)
        {
            FindObjectOfType<GameOverManager>().Die();
        }
    }
}
