using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLArmStatsSO : ScriptableObject
{
    // Default stats for the current left arm
    private float baseHealth;
    private float baseMaxHealth;
    private float baseAttackSpeed;
    private float baseAttackDamage;

    // Trinkets subtract their buffed value from the base value locally and add that value to these values
    private float healthAddValue;
    private float maxHealthAddValue;
    private float attackSpeedAddValue;
    private float attackDamageAddValue;

    // Stats for the left arm after trinket buffs
    private float currentHealth;
    private float currentMaxHealth;
    private float currentAttackSpeed;
    private float currentAttackDamage; 

    //Called by the current left arm when it is swapped/initialized so this SO has updated base values
    public void LoadBaseStats(float health, float maxHealth, float attackSpeed, float attackDamage)
    {
        baseHealth = health;
        baseMaxHealth = maxHealth;
        baseAttackSpeed = attackSpeed;
        baseAttackDamage = attackDamage;
    }
}
