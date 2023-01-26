using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticles : MonoBehaviour
{
    public ParticleSystem particleToTrigger;
    public void PlayParticle()
    {
        particleToTrigger.Play();
    }
}
