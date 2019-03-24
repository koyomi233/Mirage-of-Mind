using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleView : GunViewBase
{
    private Transform effectPos;

    private GameObject bullet;
    private GameObject shell;

    private Transform effectParent;
    private Transform shellParent;
  
    public Transform EffectPos { get { return effectPos; } }
    
    public GameObject Bullet { get { return bullet; } }
    public GameObject Shell { get { return shell; } }

    public Transform EffectParent { get { return effectParent; } }
    public Transform ShellParent { get { return shellParent; } }

    public override void Init()
    {
        effectPos = M_Transform.Find("Assault_Rifle/EffectPosB");
        bullet = Resources.Load<GameObject>("Gun/Bullet");
        shell = Resources.Load<GameObject>("Gun/Shell");
        effectParent = GameObject.Find("TempObject/AssaultRifle_Effect_Parent").GetComponent<Transform>();
        shellParent = GameObject.Find("TempObject/AssaultRifle_Shell_Parent").GetComponent<Transform>();
    }

    public override void InitHoldPoseValue()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(-0.065f, -1.85f, 0.25f);
        EndRot = new Vector3(2.8f, 1.3f, 0.08f);
    }

    public override void FindGunPoint()
    {
        GunPoint = M_Transform.Find("Assault_Rifle/EffectPosA");
    }
}
