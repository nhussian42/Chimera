using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NumberComparisonCheck : Conditional
{
    [SerializeField] FloatVar Is;
    [SerializeField] Comparison compare;
    [SerializeField] float value;
    [SerializeField] bool treatAsPercent;

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
                if(Is.value == value) { return true; }
                else { return false; }
                
            case Comparison.greaterThanOrEqualTo:
                if(Is.value >= value) { return true; }
                else { return false; }

            case Comparison.lessThanOrEqualTo:
                if(Is.value <= value) { return true; }
                else { return false; }
            default: return false;
        }
    }

}
