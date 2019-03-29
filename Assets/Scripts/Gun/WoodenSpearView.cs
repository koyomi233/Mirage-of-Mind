using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpearView : GunViewBase
{
    private GameObject spear;

    public GameObject Spear { get { return spear; } }

    protected override void Init()
    {
        spear = Resources.Load<GameObject>("Gun/Wooden_Spear");
    }

    protected override void FindGunPoint()
    {
        GunPoint = M_Transform.Find("Armature/Arm_R/Forearm_R/Wrist_R/Weapon/EffectPosA");
    }

    protected override void InitHoldPoseValue()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(0, -1.58f, 0.32f);
        EndRot = new Vector3(0f, 4, 0.3f);
    }
}
