using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocArm : Arm
{
    public override void Attack()
    {
        base.Attack();
        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerCrocAttack);
    }
}
