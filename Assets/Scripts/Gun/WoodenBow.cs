using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBow : ThrowWeaponBase
{
    private WoodenBowView m_WoodenBowView;

    protected override void Init()
    {
        m_WoodenBowView = (WoodenBowView)M_GunViewBase;

        // Cannot shoot without aiming
        CanShoot(1);
    }

    protected override void LoadAudio()
    {
        Audio = Resources.Load<AudioClip>("Audios/Gun/Arrow Release");
    }

    protected override void Shoot()
    {
        GameObject arrow = GameObject.Instantiate<GameObject>(m_WoodenBowView.Arrow, m_WoodenBowView.GunPoint.position, m_WoodenBowView.GunPoint.rotation);
        arrow.GetComponent<Arrow>().Shoot(m_WoodenBowView.GunPoint.forward, 1000, Damage);
    }
}
