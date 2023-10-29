using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trinket : EventListener
{
    [SerializeField] string trinketName;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] TrinketType type;
    public int amount;

    public string TrinketName { get { return trinketName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public TrinketType TrinketType { get { return type; } }
    public int Amount { get { return amount; } }

    public override void Activate() { }

    public virtual void Add(int number)
    {
        amount += number;
    }

    public virtual void ResetAmount() { amount = 0; }

    public virtual void ResetTrinket() { }
    
}
