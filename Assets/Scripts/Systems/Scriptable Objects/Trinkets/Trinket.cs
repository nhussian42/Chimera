using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trinket : EventListener
{
    [SerializeField] string trinketName;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] TrinketType type;
    protected int amount;

    public string TrinketName { get { return trinketName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public TrinketType TrinketType { get { return type; } }
    public int Amount { get { return amount; } }  

    public abstract override void Activate();

    public virtual void Add(int number)
    {
        amount += number;
    }

    public abstract void ResetTrinket();

    
}
