using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRagdoll : MonoBehaviour
{
    private Transform m_Transform;
    private BoxCollider m_BoxCollider_A;
    private BoxCollider m_BoxCollider_B;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_BoxCollider_A = m_Transform.Find("Armature").GetComponent<BoxCollider>();
        m_BoxCollider_B = m_Transform.Find("Armature/Hips/Middle_Spine").GetComponent<BoxCollider>();
    }

    public void StartRagdoll()
    {
        m_BoxCollider_A.enabled = false;
        m_BoxCollider_B.enabled = false;
    }
}
