﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// State of AI
public enum AIState
{
    IDLE,
    WALK,
    ENTERRUN,
    EXISTRUN,
    ENTERATTACK,
    EXISTATTACK,
    DEATH
}

public class AI : MonoBehaviour
{
    private Transform m_Transform;
    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    private Transform playerTransform;

    private Vector3 dir;
    private List<Vector3> posList = new List<Vector3>();

    private AIState m_AIState;

    public Vector3 Dir { get { return dir; } set { dir = value; } }
    public List<Vector3> PosList { get { return posList; } set { posList = value; } }

    public AIState M_AIState { get { return m_AIState; } set { m_AIState = value; } } 

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_NavMeshAgent.SetDestination(dir);
        m_Animator = gameObject.GetComponent<Animator>();
        playerTransform = GameObject.Find("FPSController").GetComponent<Transform>();

        m_AIState = AIState.IDLE;
    }

    private void Update()
    {
        Distance();
        AIFollowPlayer();
        AIAttackPlayer();

        // Test death
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleState(AIState.DEATH);
        }
        // Test hit
        if (Input.GetKeyDown(KeyCode.J))
        {
            HitHard();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            HitNormal();
        }
    }

    // Change destination based on distance between AI and node
    private void Distance()
    {
        if(m_AIState == AIState.IDLE || m_AIState == AIState.WALK)
        {
            if (Vector3.Distance(m_Transform.position, dir) <= 1)
            {
                int index = Random.Range(0, posList.Count);
                dir = posList[index];
                m_NavMeshAgent.SetDestination(dir);
                ToggleState(AIState.IDLE);
            }
            else
            {
                ToggleState(AIState.WALK);
            }
        }
    }

    // Change AI state
    private void ToggleState(AIState state)
    {
        switch (state)
        {
            case AIState.IDLE:
                IdleState();
                break;
            case AIState.WALK:
                WalkState();
                break;
            case AIState.ENTERRUN:
                EnterRunState();
                break;
            case AIState.EXISTRUN:
                ExistRunState();
                break;
            case AIState.ENTERATTACK:
                EnterAttackState();
                break;
            case AIState.EXISTATTACK:
                ExistAttackState();
                break;
            case AIState.DEATH:
                AIDie();
                break;
        }
    }

    // AI state of idle
    private void IdleState()
    {
        m_Animator.SetBool("Walk", false);
        m_AIState = AIState.IDLE;
    }

    // AI state of walk
    private void WalkState()
    {
        m_Animator.SetBool("Walk", true);
        m_AIState = AIState.WALK;
    }

    // AI run to player
    private void AIFollowPlayer()
    {
        if (Vector3.Distance(m_Transform.position, playerTransform.position) <= 18)
        {
            // Follow the player
            ToggleState(AIState.ENTERRUN);
        }
        else
        {
            // Leave the player
            ToggleState(AIState.EXISTRUN);
        }
    }

    // AI attack player
    private void AIAttackPlayer()
    {
        if (m_AIState == AIState.ENTERRUN)
        {
            if (Vector3.Distance(m_Transform.position, playerTransform.position) <= 2)
            {
                // Enter attack state
                ToggleState(AIState.ENTERATTACK);
            }
            else
            {
                // Exist attack state
                ToggleState(AIState.EXISTATTACK);
            }
        }
    }

    // AI die
    private void AIDie()
    {
        m_AIState = AIState.DEATH;
        m_NavMeshAgent.isStopped = true;
        m_Animator.SetTrigger("Death");
        StartCoroutine("Death");
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(5);
        GameObject.Destroy(gameObject);
        SendMessageUpwards("AIDeath", gameObject);
    }

    private void EnterRunState()
    {
        m_Animator.SetBool("Run", true);
        m_AIState = AIState.ENTERRUN;
        m_NavMeshAgent.speed = 2;
        m_NavMeshAgent.SetDestination(playerTransform.position);
    }

    private void ExistRunState()
    {
        m_Animator.SetBool("Run", false);
        ToggleState(AIState.WALK);
        m_NavMeshAgent.speed = 0.8f;
        m_NavMeshAgent.SetDestination(dir);
    }

    private void EnterAttackState()
    {
        m_Animator.SetBool("Attack", true);
        m_AIState = AIState.ENTERATTACK;
        //m_NavMeshAgent.enabled = false;
    }

    private void ExistAttackState()
    {
        m_Animator.SetBool("Attack", false);
        //m_NavMeshAgent.enabled = true;
        ToggleState(AIState.ENTERRUN); 
    }
    
    private void HitHard()
    {
        m_Animator.SetTrigger("HitHard");
    }

    private void HitNormal()
    {
        m_Animator.SetTrigger("HitNormal");
    }
}
