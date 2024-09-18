using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    private float m_idleTimer;

    [SerializeField] private float m_idleTime;

    private Transform m_player;

    [SerializeField] private float m_detectionAreaRadius = 18f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set timer to zero
        m_idleTimer = 0;

        // Get players Transform
        m_player = GlobalReferences.Instance.g_player.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_idleTimer += Time.deltaTime;

        if (m_idleTimer > m_idleTime)
        {
            animator.SetBool("isPatrolling", true);
        }

        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);
        if (distanceFromPlayer < m_detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

}
