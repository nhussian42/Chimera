using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegs : Legs
{
    public override void ActivateAbility()
    {
        Debug.Log("Legs Activated");
        StartCoroutine(Cooldown());
    }


}
