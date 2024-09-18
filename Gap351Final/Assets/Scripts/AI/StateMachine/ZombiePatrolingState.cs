using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    private float m_patrolTimer;

    [SerializeField] private float m_patrolTime;

    private Transform m_player;
    private NavMeshAgent m_navMeshAgent;

    [SerializeField] private float m_detectionAreaRadius = 18f;
    [SerializeField] private float m_patrolSpeed = 2f;

    List<Transform> m_waypointsList = new List<Transform>();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_player = GlobalReferences.Instance.g_player.transform; // Get player ref
        m_navMeshAgent = animator.GetComponent<NavMeshAgent>(); // Get nav agent

        m_navMeshAgent.speed = m_patrolSpeed;
        m_patrolTimer = 0;


        // Get all waypoints and move to the first one
        foreach (Transform transform in GlobalReferences.Instance.g_waypointCluster.transform)
        {
            m_waypointsList.Add(transform);
        }

        Vector3 nextPosition = m_waypointsList[Random.Range(0, m_waypointsList.Count)].position;
        m_navMeshAgent.SetDestination(nextPosition);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!SoundManager.Instance.ZombieWalking.isPlaying)
            SoundManager.Instance.ZombieWalking.PlayDelayed(2);

        // Check if we reached the waypoint
        if (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
        {
            m_navMeshAgent.SetDestination(m_waypointsList[Random.Range(0, m_waypointsList.Count)].position);
        }

        // Check if patrolling time is over
        m_patrolTimer += Time.deltaTime;
        if (m_patrolTimer > m_patrolTime)
        {
            animator.SetBool("isPatrolling", false );
        }

        // if we see payer
        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);
        if (distanceFromPlayer < m_detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.ZombieWalking.isPlaying)
            SoundManager.Instance.ZombieWalking.Stop();

        // stop our agent
        m_navMeshAgent.SetDestination(m_navMeshAgent.transform.position);
    }
}
