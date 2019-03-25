using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunControllerBase
{
    private ShotgunView m_ShotgunView;

    public override void Init()
    {
        m_ShotgunView = (ShotgunView)M_GunViewBase;
    }

    public override void LoadAudio()
    {
        Audio = Resources.Load<AudioClip>("Audios/Gun/Shotgun_Fire");
    }

    public override void LoadEffect()
    {
        Effect = Resources.Load<GameObject>("Effects/Gun/Shotgun_GunPoint_Effect");
    }

    public override void PlayEffect()
    {
        // Gun fire effect
        GameObject tempEffect = GameObject.Instantiate<GameObject>(Effect, m_ShotgunView.GunPoint.position, Quaternion.identity);
        tempEffect.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DelayDestory(tempEffect, 2.0f));

        
    }

    // Shell out effect
    private void ShellOutEffect()
    {
        GameObject tempShell = GameObject.Instantiate<GameObject>(m_ShotgunView.Shell, m_ShotgunView.EffectPos.position, Quaternion.identity);
        tempShell.GetComponent<Rigidbody>().AddForce(m_ShotgunView.EffectPos.up * 70);
        StartCoroutine(DelayDestory(tempShell, 5.0f));
    }

    public override void Shoot()
    {
        
    }

    private void PlayEffectAudio()
    {
        AudioSource.PlayClipAtPoint(m_ShotgunView.EffectAudio, m_ShotgunView.EffectPos.position);
    }

    IEnumerator DelayDestory(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Destroy(obj);
    }
}
