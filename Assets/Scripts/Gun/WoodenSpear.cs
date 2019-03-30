using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpear : ThrowWeaponBase
{
    private WoodenSpearView m_WoodenSpearView;

    protected override void Init()
    {
        m_WoodenSpearView = (WoodenSpearView)M_GunViewBase;
        CanShoot(0);
    }

    protected override void LoadAudio()
    {
        Audio = Resources.Load<AudioClip>("Audios/Gun/Arrow Release");
    }

    protected override void Shoot()
    {
        GameObject spear = GameObject.Instantiate<GameObject>(m_WoodenSpearView.Spear, m_WoodenSpearView.GunPoint.position, m_WoodenSpearView.GunPoint.rotation);
        spear.GetComponent<Arrow>().Shoot(m_WoodenSpearView.GunPoint.forward, 2000, Damage);
        // Decrease durable
        Durable--;
    }
}
