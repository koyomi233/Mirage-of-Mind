using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;

    private RaycastHit hit;
    private int damage;

    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();

        Invoke("KillSelf", 3);
    }

    private void KillSelf()
    {
        GameObject.Destroy(gameObject);
    }

    public void Shoot(Vector3 dir, int force, int damage)
    {
        m_Rigidbody.AddForce(dir * force);
        this.damage = damage;
        Ray ray = new Ray(m_Transform.position, dir);
        if (Physics.Raycast(ray, out hit, 1000, 1 << 11)){}
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_Rigidbody.Sleep();
        if (collision.collider.GetComponent<BulletMark>() != null)
        {
            collision.collider.GetComponent<BulletMark>().CreateBulletMark(hit);
            collision.collider.GetComponent<BulletMark>().HP -= damage;
        }
        GameObject.Destroy(gameObject);
    }
}
