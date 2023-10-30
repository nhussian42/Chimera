using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Trinkets/FlatBuffTrinket", order = 1)]
public class FlatBuffTrinket : Trinket
{
    
    [SerializeField] FloatVar modify;
    [SerializeField] MathOperation by;
    [SerializeField] float thisValue;
    [SerializeField] bool treatAsPercent;
    [SerializeField] FloatVar returnTo;

    [SerializeField] bool debug;
    bool activated;

    public override void Enable()
    {
        activated = false;
        amount = 0;
        base.Enable();
    }

    public override void Activate()
    {
        if(activated == false)
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
            activated = true;
            if(debug == true) { Debug.Log("IN: " + modify.name + " value: " + modify.value + " | " + "OUT: " + returnTo.name + " value: " + returnTo.value); }
        }
        
    }

    public override void ResetTrinket()
    {
        activated = false;
    }
}
