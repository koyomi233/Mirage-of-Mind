using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class of bullet
/// </summary>
public abstract class BulletBase : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private int damage;

    public Transform M_Transform { get { return m_Transform; } }
    public Rigidbody M_Rigidbody { get { return m_Rigidbody; } }
    public int Damage { get { return damage; } set { damage = value; } }

    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();

        Init();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }

    // Tail vibrating animation
    private IEnumerator TailAnimation(Transform pivot)
    {
        float stopTime = Time.time + 1.0f;
        float range = 1.0f;
        float vel = 0;

        Quaternion startRot = Quaternion.Euler(new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0));

        while (Time.time < stopTime)
        {
            pivot.localRotation = Quaternion.Euler(new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0)) * startRot;
            range = Mathf.SmoothDamp(range, 0, ref vel, stopTime - Time.time);

            yield return null;
        }
    }

    public void KillSelf()
    {
        GameObject.Destroy(gameObject);
    }

    public abstract void Init();
    public abstract void Shoot(Vector3 dir, int force, int damage, RaycastHit hit);
    public abstract void CollisionEnter(Collision collision);
}
