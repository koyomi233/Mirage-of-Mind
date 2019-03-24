using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// View of Guns
/// </summary>
public abstract class GunViewBase : MonoBehaviour
{
    // Basic components
    private Transform m_Transform;
    private Animator m_Animator;
    private Camera m_EnvCamera;

    // Aim action
    private Vector3 startPos;
    private Vector3 startRot;
    private Vector3 endPos;
    private Vector3 endRot;

    private Transform frontSight;                                         // Front sight UI
    private Transform gunPoint;                                           // Muzzle

    // Basic components' attribute
    public Transform M_Transform { get { return m_Transform; } }
    public Animator M_Animator { get { return m_Animator; } }
    public Camera M_EnvCamera { get { return m_EnvCamera; } }

    // Aim action's attribute
    public Vector3 StartPos { get { return startPos; } set { startPos = value; } }
    public Vector3 StartRot { get { return startRot; } set { startRot = value; } }
    public Vector3 EndPos { get { return endPos; } set { endPos = value; } }
    public Vector3 EndRot { get { return endRot; } set { endRot = value; } }

    public Transform FrontSight { get { return frontSight; } }
    public Transform GunPoint { get { return gunPoint; } set { gunPoint = value; } }

    public virtual void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_EnvCamera = GameObject.Find("EnvCamera").GetComponent<Camera>();
        frontSight = GameObject.Find("FrontSight").GetComponent<Transform>();

        Init();
        InitHoldPoseValue();
        FindGunPoint();
    }

    // Aim
    public void EnterHoldPose(float time = 0.2f, int fov = 40)
    {
        M_Transform.DOLocalMove(EndPos, time);
        M_Transform.DOLocalRotate(EndRot, time);

        M_EnvCamera.DOFieldOfView(fov, time);
    }
    // Reset
    public void ExistHoldPose(float time = 0.2f, int fov = 60)
    {
        M_Transform.DOLocalMove(StartPos, time);
        M_Transform.DOLocalRotate(StartRot, time);

        M_EnvCamera.DOFieldOfView(fov, time);
    }

    // Initialize all components
    public abstract void Init();

    // Initialize actions of aiming
    public abstract void InitHoldPoseValue();

    // Search the position of muzzle
    public abstract void FindGunPoint();
}
