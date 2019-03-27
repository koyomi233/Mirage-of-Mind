using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBow : GunControllerBase
{
    private WoodenBowView m_WoodenBowView;

    public override void Init()
    {
        m_WoodenBowView = (WoodenBowView)M_GunViewBase;

        // Cannot shoot without aiming
        CanShoot(1);
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
        GameObject arrow = GameObject.Instantiate<GameObject>(m_WoodenBowView.Arrow, m_WoodenBowView.GunPoint.position, m_WoodenBowView.GunPoint.rotation);
        arrow.GetComponent<Arrow>().Shoot(m_WoodenBowView.GunPoint.forward, 1000, Damage);
    }
}
