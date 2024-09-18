using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private ZombieHandHitBox m_zombieHandHitBox;

    [SerializeField] private float m_zombieDamage;

    private void Start()
    {
        m_zombieHandHitBox.SetDamage(m_zombieDamage);
    }
}
