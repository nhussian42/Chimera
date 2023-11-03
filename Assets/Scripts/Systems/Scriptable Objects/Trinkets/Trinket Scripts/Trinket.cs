using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trinket : EventListener
{
    [SerializeField] string trinketName;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] Rarity rarity;
    [SerializeField] TrinketType type;
    [HideInInspector]public int amount;

    public string TrinketName { get { return trinketName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public Rarity Rarity { get { return rarity; } }
    public TrinketType TrinketType { get { return type; } }
    public int Amount { get { return amount; } }

    public override void Activate() { }

    public virtual void Add(int number)
    {
        amount += number;
    }

    public virtual void ResetTrinket() { }
    
}
