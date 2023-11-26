using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoArm : Arm
{
    public override void Attack()
    {
        base.Attack();
        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerRhinoAttack);
    }
}
