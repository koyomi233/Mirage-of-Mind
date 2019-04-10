using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunWeaponBase
{
    private ShotgunView m_ShotgunView;

    protected override void Init()
    {
        m_ShotgunView = (ShotgunView)M_GunViewBase;
    }

    protected override void LoadAudio()
    {
        Audio = Resources.Load<AudioClip>("Audios/Gun/Shotgun_Fire");
    }

    protected override void LoadEffect()
    {
        Effect = Resources.Load<GameObject>("Effects/Gun/Shotgun_GunPoint_Effect");
    }

    protected override void PlayEffect()
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

    protected override void Shoot()
    {
        StartCoroutine("CreateBullets");
        // Decrease durable
        Durable--;
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

    private IEnumerator CreateBullets()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
            GameObject tempBullet = GameObject.Instantiate<GameObject>(m_ShotgunView.Bullet, m_ShotgunView.GunPoint.position, Quaternion.identity);
            tempBullet.GetComponent<ShotgunBullet>().Shoot(m_ShotgunView.GunPoint.forward + offset, 6000, Damage / 5, Hit);
            //tempBullet.GetComponent<ShotgunBullet>().Shoot(m_ShotgunView.GunPoint.forward, 6000);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
