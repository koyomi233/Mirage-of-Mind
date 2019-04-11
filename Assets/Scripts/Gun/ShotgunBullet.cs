using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : BulletBase
{
    private RaycastHit hit;
    private Ray ray;

    public override void Init()
    {
        Invoke("KillSelf", 3);
    }

    public override void Shoot(Vector3 dir, int force, int damage, RaycastHit hit)
    {
        M_Rigidbody.AddForce(dir * force);
        this.Damage = damage;
        ray = new Ray(M_Transform.position, dir);
    }

    public override void CollisionEnter(Collision collision)
    {
        M_Rigidbody.Sleep();
        if (collision.collider.GetComponent<BulletMark>() != null)
        {
            if (Physics.Raycast(ray, out hit, 1000, 1 << 11)) { }
            collision.collider.GetComponent<BulletMark>().CreateBulletMark(hit);
            collision.collider.GetComponent<BulletMark>().HP -= Damage;
        }

        if(collision.collider.GetComponentInParent<AI>() != null)
        {
            if (Physics.Raycast(ray, out hit, 1000, 1 << 12)) { }

            if (collision.collider.gameObject.name == "Head")
            {
                collision.collider.GetComponentInParent<AI>().HeadHit(Damage * 2);
            }
            else
            {
                collision.collider.GetComponentInParent<AI>().NormalHit(Damage);
            }

            collision.collider.GetComponentInParent<AI>().PlayerEffect(hit);
        }

        GameObject.Destroy(gameObject);
    }
}
