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

    [SerializeField] List<Conditional> onlyModifyIf = new List<Conditional>();
    [SerializeField]
    [Tooltip("How many conditionals need to return true for this buff to activate? (Usually set to the number of conditionals you have in the list)")] 
    int trueReturnsNeeded;
    private int trueConditions;

    [SerializeField] bool debug;

    public override void Activate()
    {
        Debug.Log("called Activate");
        //if(activated == false)
        //{
            trueConditions = 0;
            foreach (Conditional condition in onlyModifyIf)
            {
                if (condition.Check() == true)
                    trueConditions++;
            }
            if (trueConditions == trueReturnsNeeded)
                Calculate();
            //SetActivatedTrue();

            if (debug == true) 
            {
                Debug.Log("IN: " + modify.name + " value: " + modify.value + " | " + "OUT: " + returnTo.name + " value: " + returnTo.value);
            }
        //}
        
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
