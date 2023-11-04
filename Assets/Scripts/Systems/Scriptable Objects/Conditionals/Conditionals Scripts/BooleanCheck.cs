using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Conditionals/BooleanCheck", order = 1)]
public class BooleanCheck : Conditional
{
    [SerializeField] BoolVar Is;
    [SerializeField] bool equalTo;

    public override bool Check()
    {
        if(Is.value == equalTo) { return true; }
        else { return false; }
    }
}
