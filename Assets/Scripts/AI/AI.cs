using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private Transform m_Transform;
    private NavMeshAgent m_NavMeshAgent;

    private Vector3 dir;
    private List<Vector3> posList = new List<Vector3>();

    public Vector3 Dir { get { return dir; } set { dir = value; } }
    public List<Vector3> PosList { get { return posList; } set { posList = value; } }

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_NavMeshAgent.SetDestination(dir);
        
    }

    private void Update()
    {
        Distance();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Death();
            SendMessageUpwards("AIDeath", gameObject);
        }
    }

    private void Distance()
    {
        if (Vector3.Distance(m_Transform.position, dir) <= 1)
        {
            int index = Random.Range(0, posList.Count);
            dir = posList[index];
            m_NavMeshAgent.SetDestination(dir);
            
        }
    }

    private void Death()
    {
        GameObject.Destroy(gameObject);
    }
}
