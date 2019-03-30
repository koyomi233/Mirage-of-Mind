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

    private GameObject prefab_FrontSight;                                 // Front sight prefab
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

    protected virtual void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_EnvCamera = GameObject.Find("EnvCamera").GetComponent<Camera>();

        prefab_FrontSight = Resources.Load<GameObject>("Gun/FrontSight");
        frontSight = GameObject.Instantiate<GameObject>(prefab_FrontSight, GameObject.Find("MainPanel").GetComponent<Transform>()).GetComponent<Transform>();

        Init();
        InitHoldPoseValue();
        FindGunPoint();
    }

    // Initialize all components
    protected abstract void Init();

    private void OnEnable()
    {
        ShowFrontSight();
    }

    private void OnDisable()
    {
        HideFrontSight();
    }

    private void ShowFrontSight()
    {
        frontSight.gameObject.SetActive(true);
    }

    private void HideFrontSight()
    {
        if(frontSight != null)
        {
            frontSight.gameObject.SetActive(false);
        }
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

    // Initialize actions of aiming
    protected abstract void InitHoldPoseValue();

    // Search the position of muzzle
    protected abstract void FindGunPoint();
}
