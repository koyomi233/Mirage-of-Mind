using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : BulletBase
{
    private RaycastHit hit;

    public override void Init()
    {
        Invoke("KillSelf", 3);
    }

    public override void Shoot(Vector3 dir, int force, int damage)
    {
        M_Rigidbody.AddForce(dir * force);
        this.Damage = damage;
        Ray ray = new Ray(M_Transform.position, dir);
        if (Physics.Raycast(ray, out hit, 1000, 1 << 11)) { }
    }

    public override void CollisionEnter(Collision collision)
    {
        M_Rigidbody.Sleep();
        if (collision.collider.GetComponent<BulletMark>() != null)
        {
            collision.collider.GetComponent<BulletMark>().CreateBulletMark(hit);
            collision.collider.GetComponent<BulletMark>().HP -= Damage;
        }
        GameObject.Destroy(gameObject);
    }
}
