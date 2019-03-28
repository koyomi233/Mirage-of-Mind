using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private BoxCollider m_BoxCollider;

    private Transform m_Pivot;

    private int damage;

    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_BoxCollider = gameObject.GetComponent<BoxCollider>();

        m_Pivot = m_Transform.Find("Pivot").GetComponent<Transform>();
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
            StartCoroutine("TailAnimation");
        }
    }

    // Tail vibrating animation
    private IEnumerator TailAnimation()
    {
        float stopTime = Time.time + 1.0f;
        float range = 1.0f;
        float vel = 0;

        Quaternion startRot = Quaternion.Euler(new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0));

        while(Time.time < stopTime)
        {
            m_Pivot.localRotation = Quaternion.Euler(new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0)) * startRot;
            range = Mathf.SmoothDamp(range, 0, ref vel, stopTime - Time.time);

            yield return null;
        }
    }
}
