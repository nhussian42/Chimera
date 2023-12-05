using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : Limb
{
    [SerializeField] private string description;
    public string Description { get { return description; } }

    // Called to update stats after applying a trinket buff, loading a new scene, etc.
    public virtual void LoadStats(float maxHP, float currentHP)
    {
        maxHealth = maxHP;
        Health = currentHP;
    }

    public override void Disintegrate()
    {
        base.Disintegrate();
        PlayerController.Instance.RevertToDefault(this);
    }
}
