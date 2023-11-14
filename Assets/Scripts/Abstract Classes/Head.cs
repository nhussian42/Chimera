using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : Limb
{
    // Called to update stats after applying a trinket buff, loading a new scene, etc.
    public virtual void LoadStats(float maxHP, float currentHP)
    {
        maxHealth = maxHP;
        Health = currentHP;
    }
}
