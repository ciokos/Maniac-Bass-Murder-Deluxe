using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    private bool isActive = false;
    private Enemy enemy;
    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemy.ActivateEvent.AddListener(onActivate);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.enabled = false;
    }

    private void Update()
    {
        if(isActive)
            agent.SetDestination(target.position);
    }
    private void onActivate()
    {
        agent.enabled = true;
        isActive = true; 
    }


}