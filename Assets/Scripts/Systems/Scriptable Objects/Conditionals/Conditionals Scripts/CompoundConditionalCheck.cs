using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Conditionals/CompoundConditionalCheck", order = 1)]
public class CompoundConditionalCheck : Conditional
{
    [SerializeField] List<Conditional> check = new List<Conditional>();
    [SerializeField]
    [Tooltip("How many conditionals need to return true for this to return true? (Usually set to the number of conditionals you have in the list)")]
    int trueReturnsNeeded;
    int trueConditions;

    public override bool Check()
    {
        trueConditions = 0;
        foreach (Conditional condition in check)
        {
            if (condition.Check() == true)
            {
                trueConditions++;
            }
        }
        if (trueConditions >= trueReturnsNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
