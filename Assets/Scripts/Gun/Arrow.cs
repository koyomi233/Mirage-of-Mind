using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BulletBase
{
    private BoxCollider m_BoxCollider;
    private Transform m_Pivot;

    public override void Init()
    {
        m_BoxCollider = gameObject.GetComponent<BoxCollider>();
        m_Pivot = M_Transform.Find("Pivot").GetComponent<Transform>();
    }

    public override void Shoot(Vector3 dir, int force, int damage)
    {
        M_Rigidbody.AddForce(dir * force);
        this.Damage = damage;
    }

    public override void CollisionEnter(Collision collision)
    {
        M_Rigidbody.Sleep();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Env"))
        {
            GameObject.Destroy(M_Rigidbody);
            GameObject.Destroy(m_BoxCollider);
            collision.collider.GetComponent<BulletMark>().HP -= Damage;
            M_Transform.SetParent(collision.gameObject.transform);
            StartCoroutine("TailAnimation", m_Pivot);
        }
    }
}
