using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Trinkets/FlatBuffTrinket", order = 1)]
public class FlatBuffTrinket : Trinket
{
    [SerializeField] FloatVar modify;
    [SerializeField] MathOperation by;
    [SerializeField] float thisValue;
    [SerializeField] bool treatAsPercent;
    [SerializeField] FloatVar returnTo;
    private int amount;

    public override void Activate()
    {
        if(treatAsPercent == true)
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

            returnTo.value = returnTo.value + (modify.value - modifiedValue);
        }
    }

    public void AddDuplicate()
    {
        amount++;
    }
}
