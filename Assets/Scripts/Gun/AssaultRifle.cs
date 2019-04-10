using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssaultRifle : GunWeaponBase
{
    private AssaultRifleView m_AssaultRifeView;
    private ObjectPool[] pools;

    protected override void Init()
    {
        m_AssaultRifeView = (AssaultRifleView)M_GunViewBase;
        pools = gameObject.GetComponents<ObjectPool>();
    }

    // Play effect
    protected override void PlayEffect()
    {
        GunFireEffect();
        ShellEffect();
    }

    // Gun fire effect
    private void GunFireEffect()
    {
        GameObject gunEffect = null;
        if (pools[0].Data())                                               // Use existed gun fire effect
        {
            gunEffect = pools[0].GetObject();
            gunEffect.GetComponent<Transform>().position = Hit.point;
        }
        else                                                               // Add new gun fire effect
        {
            gunEffect = GameObject.Instantiate<GameObject>(Effect, m_AssaultRifeView.GunPoint.position, Quaternion.identity, m_AssaultRifeView.EffectParent);
            gunEffect.name = "GunPoint_Effect";
        }
        gunEffect.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Delay(pools[0], gunEffect, 1));
    }

    // Shell effect
    private void ShellEffect()
    {
        GameObject shell = null;
        // Shell out effect
        if (pools[1].Data())
        {
            shell = pools[1].GetObject();
            shell.GetComponent<Rigidbody>().isKinematic = true;
            shell.GetComponent<Transform>().position = m_AssaultRifeView.EffectPos.position;
            shell.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            shell = GameObject.Instantiate<GameObject>(m_AssaultRifeView.Shell, m_AssaultRifeView.EffectPos.position, Quaternion.identity, m_AssaultRifeView.ShellParent);
            shell.name = "shell";
        }
        shell.GetComponent<Rigidbody>().AddForce(m_AssaultRifeView.EffectPos.up * 50);
        StartCoroutine(Delay(pools[1], shell, 3));
    }

    // Open fire
    protected override void Shoot()
    {
        if (Hit.point != Vector3.zero)
        {
            if(Hit.collider.GetComponent<BulletMark>() != null)
            {
                Hit.collider.GetComponent<BulletMark>().CreateBulletMark(Hit);
                Hit.collider.GetComponent<BulletMark>().HP -= Damage;
            }
            else if (Hit.collider.GetComponentInParent<AI>() != null)
            {
                Hit.collider.GetComponentInParent<AI>().Life -= Damage;
                Hit.collider.GetComponentInParent<AI>().PlayerEffect(Hit);
                GameObject bullet = GameObject.Instantiate<GameObject>(m_AssaultRifeView.Bullet, Hit.point, Quaternion.identity);
                bullet.GetComponent<Transform>().SetParent(Hit.collider.GetComponent<Transform>());
            }
            else
            {
                GameObject.Instantiate<GameObject>(m_AssaultRifeView.Bullet, Hit.point, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Empty");
        }

        // Decrease durable
        Durable--;
    }

    protected override void LoadAudio()
    {
        Audio = Resources.Load<AudioClip>("Audios/Gun/AssaultRifle_Fire");
    }

    protected override void LoadEffect()
    {
        Effect = Resources.Load<GameObject>("Effects/Gun/AssaultRifle_GunPoint_Effect");
    }
}
