using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public AudioSource deathSound;
    public ParticleSystem diesParticle;
    public float maxLifeValue;
    float currentLife;
    void OnEnable()
    {
        currentLife = maxLifeValue;
    }
    public bool ReceiveDamage(float damage)
    {
        currentLife -= damage;
        return IsDead();
    }
    bool IsDead()
    {
        if (currentLife <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    void Die()
    {
        diesParticle.gameObject.transform.SetParent(null);
        diesParticle.GetComponent<DestroyAfterTime>().DestroyAfterThisTime(3);
        diesParticle.Play();
        deathSound.Play();
        Destroy(gameObject);
    }
}
