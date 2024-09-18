using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingState : StateMachineBehaviour
{
    private Transform m_player;
    private NavMeshAgent m_navMeshAgent;

    [SerializeField] private float m_stopAttackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_player = GlobalReferences.Instance.g_player.transform; // Get player ref
        m_navMeshAgent = animator.GetComponent<NavMeshAgent>(); // Get nav agent
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!SoundManager.Instance.ZombieAttack.isPlaying)
            SoundManager.Instance.ZombieAttack.Play();

        LookAtPlayer();

        // check if agent should stop attacking

        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);

        // Check if the agent should stop chasing
        if (distanceFromPlayer > m_stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = m_player.position - m_navMeshAgent.transform.position;
        m_navMeshAgent.transform.rotation = Quaternion.LookRotation(direction);

        float yRotation = m_navMeshAgent.transform.eulerAngles.y;
        m_navMeshAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.ZombieAttack.isPlaying)
            SoundManager.Instance.ZombieAttack.Stop();

    }
}
