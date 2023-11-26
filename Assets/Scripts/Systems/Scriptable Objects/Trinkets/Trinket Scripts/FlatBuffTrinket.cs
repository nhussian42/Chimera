using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Trinkets/FlatBuffTrinket", order = 1)]
public class FlatBuffTrinket : Trinket
{
    
    [SerializeField] FloatVar modify;
    [SerializeField] MathOperation by;
    [SerializeField] float thisValue;
    [SerializeField] bool treatAsPercent;
    [SerializeField] bool trackQuantity;
    [SerializeField][HideInInspector] int quantity;
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
        //Debug.Log(amount);
        if (activated == false)
        {
            float thisValueDup = 0f;
            
            if (treatAsPercent == true)
            {
                thisValueDup = thisValue / 100f;
            }
            else
            {
                thisValueDup = thisValue;
            }

            for (int i = 0; i < amount; i++)
            {
                float modifiedValue = modify.value;

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

            if(trackQuantity == true)
            {
                quantity++;
            }

            if (debug == true)
            {
                Debug.Log("IN: " + modify.name + " value: " + modify.value + " | " + "OUT: " + returnTo.name + " value: " + returnTo.value);
            }

        }
    }
    public override void ResetTrinket()
    {
        activated = false;
    }
}
