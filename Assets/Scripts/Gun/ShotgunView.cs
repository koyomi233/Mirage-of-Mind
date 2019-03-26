using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunView : GunViewBase
{
    private Transform effectPos;
    private AudioClip effectAudio;
    private GameObject shell;
    private GameObject bullet;

    public Transform EffectPos { get { return effectPos; } }
    public AudioClip EffectAudio { get { return effectAudio; } }
    public GameObject Shell { get { return shell; } }
    public GameObject Bullet { get { return bullet; } }

    public override void Init()
    {
        effectPos = M_Transform.Find("Armature/Weapon/EffectPosB");
        effectAudio = Resources.Load<AudioClip>("Audios/Gun/Shotgun_Pump");
        shell = Resources.Load<GameObject>("Gun/Shotgun_Shell");
        bullet = Resources.Load<GameObject>("Gun/Shotgun_Bullet");
    }

    public override void FindGunPoint()
    {
        GunPoint = M_Transform.Find("Armature/Weapon/EffectPosA");
    }

    public override void InitHoldPoseValue()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(-0.14f, -1.78f, -0.03f);
        EndRot = new Vector3(0, 10, -0.25f);
    }
}
