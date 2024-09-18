using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHandHitBox : MonoBehaviour
{
    private float m_damage;

    public float GetDamage()
    {
        return m_damage;
    }

    public void SetDamage(float value)
    { 
        m_damage = value;
    }
}
