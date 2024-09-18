using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChasingState : StateMachineBehaviour
{
    private Transform m_player;
    private NavMeshAgent m_navMeshAgent;

    [SerializeField] private float m_chaseSpeed = 6f;
    [SerializeField] private float m_stopChasingDistance = 21f;
    [SerializeField] private float m_attackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_player = GlobalReferences.Instance.g_player.transform; // Get player ref
        m_navMeshAgent = animator.GetComponent<NavMeshAgent>(); // Get nav agent

        m_navMeshAgent.speed = m_chaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!SoundManager.Instance.ZombieChase.isPlaying)
            SoundManager.Instance.ZombieChase.Play();

        m_navMeshAgent.SetDestination(m_player.position);
        animator.transform.LookAt(m_player);

        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);

        // Check if the agent should stop chasing
        if (distanceFromPlayer > m_stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        // Check if the player should attack
        if (distanceFromPlayer < m_attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_navMeshAgent.SetDestination(m_navMeshAgent.transform.position);

        if (SoundManager.Instance.ZombieChase.isPlaying)
            SoundManager.Instance.ZombieChase.Stop();
    }
}
