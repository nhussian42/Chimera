using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trinket : EventListener
{
    [SerializeField] string trinketName;
    [SerializeField] string description;
    [SerializeField] Sprite icon;

    public string TrinketName { get { return trinketName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }

    public abstract override void Activate();

    
}
