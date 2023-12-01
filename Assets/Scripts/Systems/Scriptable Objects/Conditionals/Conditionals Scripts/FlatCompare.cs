using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Conditionals/FlatCompare", order = 1)]
public class FlatCompare : Conditional
{
    [SerializeField] FloatVar Is;
    [SerializeField] Comparison compare;
    [SerializeField] float value;

    // Start is called before the first frame update
    public override bool Check()
    {

        switch (compare)
        {
            case Comparison.greaterThan:
                if (Is.value > value) { return true; }
                else { return false; }

            case Comparison.lessThan:
                if (Is.value < value) { return true; }
                else { return false; }

            case Comparison.equalTo:
                if (Is.value == value) { return true; }
                else { return false; }

            case Comparison.greaterThanOrEqualTo:
                if (Is.value >= value) { return true; }
                else { return false; }

            case Comparison.lessThanOrEqualTo:
                if (Is.value <= value) { return true; }
                else { return false; }
            default: return false;
        }
    }
}
