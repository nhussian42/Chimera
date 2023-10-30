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

    [SerializeField] List<Conditional> onlyModifyIf = new List<Conditional>();
    [SerializeField]
    [Tooltip("How many conditionals need to return true for this buff to activate? (Usually set to the number of conditionals you have in the list)")] 
    int trueReturnsNeeded;
    int trueConditions;
    bool activated = false;

    [SerializeField] bool debug;

    public override void Enable()
    {
        activated = false;
        amount = 0;
        base.Enable();
    }

    public override void Activate()
    {
        //Debug.Log(amount);
        if (activated == false)
        {
            trueConditions = 0;
            foreach (Conditional condition in onlyModifyIf)
            {
                if (condition.Check() == true)
                {
                    trueConditions++;
                    
                } 
            }
            if (trueConditions >= trueReturnsNeeded) 
            {
                Calculate(); 
            }
            activated = true;

            if (debug == true) 
            {
                Debug.Log("IN: " + modify.name + " value: " + modify.value + " | " + "OUT: " + returnTo.name + " value: " + returnTo.value);
            }
        }
        
    }

    private void Calculate()
    {
        float thisValueDup = 0f;
        float modifiedValue = modify.value;
        if (treatAsPercent == true)
        {
            thisValueDup = thisValue/100f;
        }
        else
        {
            thisValueDup = thisValue;
        }

        for (int i = 0; i < amount; i++)
        {
            switch (by)
            {
                case MathOperation.Adding:
                    if (treatAsPercent == true) 
                    { 
                        modifiedValue = modifiedValue + (modifiedValue * thisValueDup);
                    }
                    else { modifiedValue += thisValueDup; }
                    break;
                case MathOperation.Subtracting:
                    if (treatAsPercent == true) 
                    { 
                        modifiedValue = modifiedValue - (modifiedValue * thisValueDup); 
                    }
                    else { modifiedValue -= thisValueDup; }
                    break;
                case MathOperation.Multiplying:
                    modifiedValue *= thisValueDup;
                    break;
                case MathOperation.Dividing:
                    modifiedValue /= thisValueDup;
                    break;
            }
            returnTo.Write(returnTo.value + (modifiedValue - modify.value));
        }
    }

    public override void ResetTrinket()
    {
        activated = false;
    }
}
