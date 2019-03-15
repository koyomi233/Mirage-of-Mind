using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssaultRifleView : MonoBehaviour
{
    private Transform m_Transform;
    private Animator m_Animator;
    private Camera m_EnvCamera;

    // Aim action
    private Vector3 startPos;
    private Vector3 startRot;
    private Vector3 endPos;
    private Vector3 endRot;

    private Transform gunPoint;
    private Transform frontSight;
    private Transform effectPos;

    private GameObject bullet;
    private GameObject shell;

    public Transform M_Transform { get { return m_Transform; } }
    public Animator M_Animator { get { return m_Animator; } }
    public Camera M_EnvCamera { get { return m_EnvCamera; } }

    public Transform GunPoint { get { return gunPoint; } }
    public Transform EffectPos { get { return effectPos; } }
    public Transform FrontSight { get { return frontSight; } }

    public GameObject Bullet { get { return bullet; } }
    public GameObject Shell { get { return shell; } }

    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_EnvCamera = GameObject.Find("EnvCamera").GetComponent<Camera>();

        startPos = m_Transform.localPosition;
        startRot = m_Transform.localRotation.eulerAngles;
        endPos = new Vector3(-0.065f, -1.85f, 0.25f);
        endRot = new Vector3(2.8f, 1.3f, 0.08f);

        gunPoint = m_Transform.Find("Assault_Rifle/EffectPosA");
        effectPos = m_Transform.Find("Assault_Rifle/EffectPosB");
        frontSight = GameObject.Find("FrontSight").GetComponent<Transform>();

        bullet = Resources.Load<GameObject>("Gun/Bullet");
        shell = Resources.Load<GameObject>("Gun/Shell");
    }

    // Aim
    public void EnterHoldPose()
    {
        m_Transform.DOLocalMove(endPos, 0.2f);
        m_Transform.DOLocalRotate(endRot, 0.2f);

        m_EnvCamera.DOFieldOfView(40, 0.2f);
    }
    // Reset
    public void ExistHoldPose()
    {
        m_Transform.DOLocalMove(startPos, 0.2f);
        m_Transform.DOLocalRotate(startRot, 0.2f);

        m_EnvCamera.DOFieldOfView(60, 0.2f);
    }
}
