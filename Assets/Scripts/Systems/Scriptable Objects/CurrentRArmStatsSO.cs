using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRArmStatsSO : ScriptableObject
{
    // Default stats for the current right arm
    public float baseHealth { get; private set; }
    public float baseMaxHealth { get; private set; }
    public float baseAttackSpeed { get; private set; }
    public float baseAttackDamage { get; private set; }

    // Trinkets subtract the base value from their calculated buffed value locally and then add that value to these values
    public float healthAddValue;
    public float maxHealthAddValue;
    public float attackSpeedAddValue;
    public float attackDamageAddValue;

    // Exposed stats for the right arm after trinket buffs
    public float currentHealth { get; private set; }
    public float currentMaxHealth { get; private set; }
    public float currentAttackSpeed { get; private set; }
    public float currentAttackDamage { get; private set; }

    // Called by the current right arm when it is swapped/initialized for updated base values
    public void LoadBaseStats(float health, float maxHealth, float attackSpeed, float attackDamage)
    {
        baseHealth = health;
        baseMaxHealth = maxHealth;
        baseAttackSpeed = attackSpeed;
        baseAttackDamage = attackDamage;
    }

    // Called whenever a SO game event is called to reset the add values before calculating new ones
    public void ResetAddValues()
    {
        healthAddValue = 0;
        maxHealthAddValue = 0;
        attackSpeedAddValue = 0;
        attackDamageAddValue = 0;
    }

    // Called after all trinkets are done adding their 'add values' to this script
    public void CalculateCurrentStats()
    {
        currentHealth = baseHealth + healthAddValue;
        currentMaxHealth = baseMaxHealth + maxHealthAddValue;
        currentAttackSpeed = baseAttackSpeed + attackSpeedAddValue;
        currentAttackDamage = baseAttackDamage + attackDamageAddValue;
    }
}
