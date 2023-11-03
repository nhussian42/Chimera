using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Conditionals/StatComparisonCheck", order = 1)]
public class StatComparisonCheck : Conditional
{
    [SerializeField] FloatVar Is;
    [SerializeField] Comparison compare;
    [SerializeField] float percent;
    [SerializeField] FloatVar Of;

    public override bool Check()
    {
        float value = percent / 100;
        float comparedStat = Of.value * value;

        switch (compare)
        {
            case Comparison.greaterThan:
                if (Is.value > comparedStat) { return true; }
                else { return false; }

            case Comparison.lessThan:
                if (Is.value < comparedStat) { return true; }
                else { return false; }
                
            case Comparison.equalTo:
                if(Is.value == comparedStat) { return true; }
                else { return false; }
                
            case Comparison.greaterThanOrEqualTo:
                if(Is.value >= comparedStat) { return true; }
                else { return false; }

            case Comparison.lessThanOrEqualTo:
                if(Is.value <= comparedStat) { return true; }
                else { return false; }
            default: return false;
        }
    }

}
