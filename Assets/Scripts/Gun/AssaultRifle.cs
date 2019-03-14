using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssaultRifle : MonoBehaviour
{
    private Transform m_Transform;
    private Animator m_Animator;
    private Camera m_EnvCamera;

    private Vector3 startPos;
    private Vector3 startRot;
    private Vector3 endPos;
    private Vector3 endRot;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_EnvCamera = GameObject.Find("EnvCamera").GetComponent<Camera>();

        startPos = m_Transform.localPosition;
        startRot = m_Transform.localRotation.eulerAngles;
        endPos = new Vector3(-0.065f, -1.85f, 0.25f);
        endRot = new Vector3(2.8f, 1.3f, 0.08f);
    }

    void Update()
    {
        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            m_Animator.SetTrigger("Fire");
        }
        // Aim
        if (Input.GetMouseButton(1))
        {
            m_Animator.SetBool("HoldPose", true);
            EnterHoldPose();
        }
        // Reset
        if (Input.GetMouseButtonUp(1))
        {
            m_Animator.SetBool("HoldPose", false);
            ExistHoldPose();
        }
    }

    // Aim
    private void EnterHoldPose()
    {
        m_Transform.DOLocalMove(endPos, 0.2f);
        m_Transform.DOLocalRotate(endRot, 0.2f);

        m_EnvCamera.DOFieldOfView(40, 0.2f);
    }
    // Reset
    private void ExistHoldPose()
    {
        m_Transform.DOLocalMove(startPos, 0.2f);
        m_Transform.DOLocalRotate(startRot, 0.2f);

        m_EnvCamera.DOFieldOfView(60, 0.2f);
    }
}
