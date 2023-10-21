using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultArm : Arm
{
    public override void Attack()
    {
        base.Attack();
        //Debug.Log("Default attack");
    }
}
