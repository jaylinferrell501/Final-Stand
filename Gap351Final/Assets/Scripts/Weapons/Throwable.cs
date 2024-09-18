using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [Header("Throwable Settings")]
    [SerializeField] private float m_delay = 3f; // Delay before explosion
    [SerializeField] private float m_damageRadius = 20f; // radis of damage
    [SerializeField] private float m_explosionForce = 1200f; // Force of the explosion
    [SerializeField] private float m_damage = 100f; // Force of the explosion

    private float m_countdown;

    private bool m_hasExploded = false;
    private bool m_hasBeenThrown = false;

    public enum ThrowableType
    {
        kGrenade
    }

    [SerializeField] private ThrowableType m_throwableType; // The type of throwable

    private void Start()
    {
        m_countdown = m_delay;
    }

    private void Update()
    {
        if (m_hasBeenThrown)
        {
            m_countdown -= Time.deltaTime;
            if (m_countdown < 0 && !m_hasExploded)
            {
                Exploded();
                m_hasExploded = true;
            }
        }
    }

    private void Exploded()
    {
        // Play explode sound
        SoundManager.Instance.GernadeSound.Play();


        // Cause the effect
        GetThrowableEffect();

        // Destroy the game object
        Destroy(gameObject);

    }

    private void GetThrowableEffect()
    {
        switch (m_throwableType)
        {
            case ThrowableType.kGrenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        // Visual Effect
        GameObject explosionEffect = GlobalReferences.Instance.g_grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_damageRadius);
        foreach (Collider objectInRange  in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(m_explosionForce, transform.position, m_damageRadius);
            }

            if (objectInRange.gameObject.tag == "Enemy")
            {
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(m_damage);
            }
        }
    }

    public ThrowableType GetThrowableType()
    {
        return m_throwableType;
    }

    public void SetHasBeenThrown(bool value)
    {
        m_hasBeenThrown = value; 
    }
}
