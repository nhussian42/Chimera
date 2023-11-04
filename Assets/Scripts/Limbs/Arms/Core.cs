using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Limb
{
    public void LoadStats(float maxHP, float currentHP)
    {
        maxHealth = maxHP;
        currentHealth = currentHP;
    }
}
