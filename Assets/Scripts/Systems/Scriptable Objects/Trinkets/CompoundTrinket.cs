using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Trinkets/CompoundTrinket", order = 1)]
public class CompoundTrinket : Trinket
{
    [SerializeField] List<Trinket> trinketsToActivate = new List<Trinket>();
    bool activated;

    public override void Enable()
    {
        activated = false;
        foreach (Trinket trinket in trinketsToActivate)
        {
            trinket.Add(1);
            trinket.ResetTrinket();
        }
        base.Enable();
    }

    public override void Activate()
    {
        if (activated == false)
        {
            foreach (Trinket trinket in trinketsToActivate)
            {
                trinket.Activate();
            }
            activated = true;
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
