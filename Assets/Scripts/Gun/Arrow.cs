using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private BoxCollider m_BoxCollider;

    private int damage;

    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_BoxCollider = gameObject.GetComponent<BoxCollider>();
    }

    public void Shoot(Vector3 dir, int force, int damage)
    {
        m_Rigidbody.AddForce(dir * force);
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_Rigidbody.Sleep();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Env"))
        {
            GameObject.Destroy(m_Rigidbody);
            GameObject.Destroy(m_BoxCollider);
            collision.collider.GetComponent<BulletMark>().HP -= damage;
            m_Transform.SetParent(collision.gameObject.transform);
        }
    }
}
