using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBowView : GunViewBase
{
    private GameObject arrow;

    public GameObject Arrow { get { return arrow; } }

    protected override void Init()
    {
        arrow = Resources.Load<GameObject>("Gun/Arrow");
    }

    protected override void FindGunPoint()
    {
        GunPoint = M_Transform.Find("Armature/Arm_L/Forearm_L/Wrist_L/Weapon/EffectPosA");
    }

    protected override void InitHoldPoseValue()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(0.75f, -1.2f, 0.22f);
        EndRot = new Vector3(2.5f, -8, 35);
    }
}
