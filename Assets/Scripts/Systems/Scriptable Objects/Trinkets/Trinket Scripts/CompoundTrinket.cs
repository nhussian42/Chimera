using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Trinkets/CompoundTrinket", order = 1)]
public class CompoundTrinket : Trinket
{
    [SerializeField] List<Trinket> trinketsToActivate = new List<Trinket>();
    [SerializeField][HideInInspector] int quantity;
    [SerializeField] bool trackQuantity;
    bool activated;

    public override void Enable()
    {
        activated = false;
        amount = 0;
        foreach (Trinket trinket in trinketsToActivate)
        {
            trinket.amount = 0;
            trinket.Add(1);
            trinket.ResetTrinket();
        }
        base.Enable();
    }

    public override void Activate()
    {
        if (activated == false)
        {
            for(int i = 0; i < amount; i++)
            {
                foreach (Trinket trinket in trinketsToActivate)
                {
                    trinket.Activate();
                    trinket.ResetTrinket();
                }
            }
            activated = true;

            if (trackQuantity == true)
            {
                quantity++;
            }
        }
    }

    public override void ResetTrinket()
    {
        activated = false;
        foreach (Trinket trinket in trinketsToActivate)
        {
            trinket.ResetTrinket();
        }
    }
}
