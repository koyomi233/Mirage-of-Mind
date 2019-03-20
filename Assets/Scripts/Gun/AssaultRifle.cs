using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssaultRifle : MonoBehaviour
{
    private AssaultRifleView m_AssaultRifeView;
    private ObjectPool[] pools;

    // Field
    private int id;
    private int demage;
    private int durable;
    private GunType gunWeaponType;

    private AudioClip audio;
    private GameObject effect;

    private Ray ray;
    private RaycastHit hit;

    #region Attributes
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public int Demage
    {
        get { return demage; }
        set { demage = value; }
    }
    public int Durable
    {
        get { return durable; }
        set { durable = value; }
    }
    public GunType GunWeaponType
    {
        get { return gunWeaponType; }
        set { gunWeaponType = value; }
    }
    public AudioClip Audio
    {
        get { return audio; }
        set { audio = value; }
    }
    public GameObject Effect
    {
        get { return effect; }
        set { effect = value; }
    }
    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        MouseControl();
        ShootReady();
    }

    private void Init()
    {
        m_AssaultRifeView = gameObject.GetComponent<AssaultRifleView>();
        audio = Resources.Load<AudioClip>("Audios/Gun/AssaultRifle_Fire");
        effect = Resources.Load<GameObject>("Effects/Gun/AssaultRifle_GunPoint_Effect");

        pools = gameObject.GetComponents<ObjectPool>();
    }

    // Play sound
    private void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(audio, m_AssaultRifeView.GunPoint.position);
    }

    // Play effect
    private void PlayEffect()
    {
        GameObject gunEffect = null;
        GameObject shell = null;

        // Gun fire effect
        if (pools[0].Data())                                               // Use existed gun fire effect
        {
            gunEffect = pools[0].GetObject();
            gunEffect.GetComponent<Transform>().position = hit.point;
        }
        else                                                               // Add new gun fire effect
        {
            gunEffect = GameObject.Instantiate<GameObject>(effect, m_AssaultRifeView.GunPoint.position, Quaternion.identity, m_AssaultRifeView.EffectParent);
            gunEffect.name = "GunPoint_Effect";
        }
        gunEffect.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Delay(pools[0], gunEffect, 1));

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

    private IEnumerator Delay(ObjectPool pool, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        pool.AddObject(obj);
    }

    // Ready aim fire
    private void ShootReady()
    {
        ray = new Ray(m_AssaultRifeView.GunPoint.position, m_AssaultRifeView.GunPoint.forward);
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(m_AssaultRifeView.M_EnvCamera, hit.point);
            m_AssaultRifeView.FrontSight.position = uiPos;
        }
        else
        {
            hit.point = Vector3.zero;
            Debug.Log("no hit message");
        }
    }

    // Open fire
    private void Shoot()
    {
        if (hit.point != Vector3.zero)
        {
            if(hit.collider.GetComponent<BulletMark>() != null)
            {
                hit.collider.GetComponent<BulletMark>().CreateBulletMark(hit);
            }
            else
            {
                GameObject.Instantiate<GameObject>(m_AssaultRifeView.Bullet, hit.point, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Empty");
        }
    }

    // Control using mouse
    private void MouseControl()
    {
        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            m_AssaultRifeView.M_Animator.SetTrigger("Fire");
            PlayEffect();
            PlayAudio();
            Shoot();
        }
        // Aim
        if (Input.GetMouseButton(1))
        {
            m_AssaultRifeView.M_Animator.SetBool("HoldPose", true);
            m_AssaultRifeView.EnterHoldPose();
            m_AssaultRifeView.FrontSight.gameObject.SetActive(false);
        }
        // Reset
        if (Input.GetMouseButtonUp(1))
        {
            m_AssaultRifeView.M_Animator.SetBool("HoldPose", false);
            m_AssaultRifeView.ExistHoldPose();
            m_AssaultRifeView.FrontSight.gameObject.SetActive(true);
        }
    }
}
