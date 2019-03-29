using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunWeaponBase : GunControllerBase
{
    protected override void Start()
    {
        base.Start();
        LoadEffect();
    }

    protected override void MouseButtonLeftDown()
    {
        base.MouseButtonLeftDown();
        PlayEffect();
    }

    // Load effect resources
    protected abstract void LoadEffect();

    // Play effect
    protected abstract void PlayEffect();
}
