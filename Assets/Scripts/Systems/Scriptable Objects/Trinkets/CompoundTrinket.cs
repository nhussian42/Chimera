using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Trinkets/CompoundTrinket", order = 1)]
public class CompoundTrinket : Trinket
{
    [SerializeField] List<Trinket> trinketsToActivate = new List<Trinket>();

    public override void Activate()
    {
        //if(activated == false)
        //{
        //    foreach (Trinket trinket in trinketsToActivate)
        //    {
        //        trinket.Activate();
        //    }
        //    SetActivatedTrue();
        //}

        foreach (Trinket trinket in trinketsToActivate)
        {
            trinket.Activate();
        }

    }
}
