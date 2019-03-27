using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpear : GunControllerBase
{
    private WoodenSpearView m_WoodenSpearView;

    public override void Init()
    {
        m_WoodenSpearView = (WoodenSpearView)M_GunViewBase;
        CanShoot(0);
    }

    public override void LoadAudio()
    {
        Audio = Resources.Load<AudioClip>("Audios/Gun/Arrow Release");
    }

    public override void LoadEffect()
    {
        // No effect here
    }

    public override void PlayEffect()
    {
        // No effect here
    }

    public override void Shoot()
    {
        GameObject spear = GameObject.Instantiate<GameObject>(m_WoodenSpearView.Spear, m_WoodenSpearView.GunPoint.position, m_WoodenSpearView.GunPoint.rotation);
        spear.GetComponent<Arrow>().Shoot(m_WoodenSpearView.GunPoint.forward, 2000, Damage);
    }
}
