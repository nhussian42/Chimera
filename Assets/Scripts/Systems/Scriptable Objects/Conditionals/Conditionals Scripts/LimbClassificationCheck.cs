using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Conditionals/LimbClassificationCheck", order = 1)]
public class LimbClassificationCheck : Conditional
{
    [SerializeField] ClassificationVar Is;
    [SerializeField] Classification equalTo;

    public override bool Check()
    {
        if (Is.value == equalTo) { return true; }
        else { return false; }
    }
}
