using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinTrigger : MonoBehaviour
{
    public WinEffects winManager;
    public bool inverted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !FindObjectOfType<GameOverManager>().died)
        {

            if (inverted)
            {
                winManager.DoLoseEffects();
            }
            else
            {
                winManager.DoWinEffects();
            }
            Destroy(gameObject);
        }

    }
}
