using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Trinkets/ConditionalTrinket", order = 1)]
public class ConditionalTrinket : Trinket
{
    [SerializeField] FloatVar modify;
    [SerializeField] MathOperation by;
    [SerializeField] float thisValue;
    [SerializeField] bool treatAsPercent;
    [SerializeField] FloatVar returnTo;
    private int amount;
    private int trueConditions;

    [SerializeField] List<Conditional> onlyModifyIf = new List<Conditional>();

    public override void Activate()
    {
        trueConditions= 0;
        foreach(Conditional condition in onlyModifyIf)
        {
            if (condition.Check() == true)
                trueConditions++;
        }
        if (trueConditions == onlyModifyIf.Count)
            Calculate();
    }

    private void Calculate()
    {
        if (treatAsPercent == true)
        {
            thisValue /= 100f;
        }

        float modifiedValue = modify.value;

        for (int i = 0; i < amount; i++)
        {
            switch (by)
            {
                case MathOperation.Adding:
                    if (treatAsPercent == true) { modifiedValue = modifiedValue + (modifiedValue * thisValue); }
                    else { modifiedValue += thisValue; }
                    break;
                case MathOperation.Subtracting:
                    if (treatAsPercent == true) { modifiedValue = modifiedValue - (modifiedValue * thisValue); }
                    else { modifiedValue -= thisValue; }
                    break;
                case MathOperation.Multiplying:
                    modifiedValue *= thisValue;
                    break;
                case MathOperation.Dividing:
                    modifiedValue /= thisValue;
                    break;
            }

            returnTo.Write(returnTo.value + (modifiedValue - modify.value)); //Debug here
        }
    }

    public void AddDuplicate()
    {
        amount++;
    }
}
