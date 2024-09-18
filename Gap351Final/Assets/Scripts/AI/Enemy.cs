using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_health = 100f;
    private NavMeshAgent m_navAgent;
    private Animator m_animator;
    private bool m_isDead;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float damageAmount)
    {
        m_health -= damageAmount;

        if (m_health <= 0)
        {
            m_health = 0;

            int randomValue = Random.Range(0, 2);

            if (randomValue == 0)
            {
                m_animator.SetTrigger("DIE1");
            }
            else
            {
                m_animator.SetTrigger("DIE2");
            }
            
            m_isDead = true;

            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            SoundManager.Instance.ZombieDeath.Play();
        }
        else
        {
            m_animator.SetTrigger("DAMAGE");

            SoundManager.Instance.ZombieHurt.Play();
        }
    }

    public bool IsDead()
    {
        return m_isDead;
    }

    public void IncreaseEnemyHealthBy(float num)
    {
        m_health += num;
    }
}
