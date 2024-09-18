using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent m_navMeshAgent;

    private void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if ray hit ground
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                // Move the agent to clicked position
                m_navMeshAgent.SetDestination(hit.point);
            }

        }
    }
}
