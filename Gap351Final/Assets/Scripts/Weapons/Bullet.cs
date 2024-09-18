using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float m_BulletDamage;

    private void OnCollisionEnter(Collision collision)
    {
       // Check if the bullet hit a target
       if (collision.gameObject.CompareTag("Target"))
       {
           CreateBulletImpactEffect(collision);

           // Destroy the bullet
           Destroy(gameObject);
       }

       // Check if the bullet hit a target
       if (collision.gameObject.CompareTag("Wall"))
       {
           CreateBulletImpactEffect(collision);

           // Destroy the bullet
           Destroy(gameObject);
       }

       if (collision.gameObject.CompareTag("Enemy"))
       {

           if (!collision.gameObject.GetComponent<Enemy>().IsDead())
           { 
               collision.gameObject.GetComponent<Enemy>().TakeDamage(m_BulletDamage);
           }

           CreateBloodSprayEffect(collision);

           // Destroy the bullet
           Destroy(gameObject);
       }
    }

    private void CreateBloodSprayEffect(Collision collision)
    {
        // Get the first contact point of the collision
        ContactPoint contact = collision.contacts[0];

        // Create the bullet impact effect
        GameObject bloodSprayEffect = Instantiate(GlobalReferences.Instance.g_bloodSprayEffect,
            contact.point, Quaternion.LookRotation(contact.normal)
        );

        bloodSprayEffect.transform.SetParent(collision.transform);
    }

    // Create Bullet Impact Effect
    void CreateBulletImpactEffect(Collision collision)
    {
        // Get the first contact point of the collision
        ContactPoint contact = collision.contacts[0];

        // Create the bullet impact effect
        GameObject bulletImpactEffect = Instantiate(GlobalReferences.Instance.g_bulletImpactEffect,
            contact.point, Quaternion.LookRotation(contact.normal)
            );

        bulletImpactEffect.transform.SetParent(collision.transform);
    }

    public void SetBulletDamage(float damageToTake)
    {
        m_BulletDamage = damageToTake; 
    }
}
