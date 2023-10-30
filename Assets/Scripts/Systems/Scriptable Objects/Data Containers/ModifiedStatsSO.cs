using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Player Data/ModifiedStatsSO", order = 1)]
public class ModifiedStatsSO : ScriptableObject
{
    // THE PLAYER READS THESE STATS

    [SerializeField] bool debug;

    [Header("Reference to current base stats:")]
    public CurrentBaseStatsSO currentBaseStats;

    [Header("Buffed core stats:")]
    public FloatVar coreMaxHealth;
    public FloatVar coreHealth;

    [Header("Buffed left arm stats:")]
    public FloatVar leftArmMaxHealth;
    public FloatVar leftArmHealth;
    public FloatVar leftArmAttackDamage;
    public FloatVar leftArmAttackSpeed;

    [Header("Buffed right arm stats:")]
    public FloatVar rightArmMaxHealth;
    public FloatVar rightArmHealth;
    public FloatVar rightArmAttackDamage;
    public FloatVar rightArmAttackSpeed;

    [Header("Buffed legs stats:")]
    public FloatVar legsMaxHealth;
    public FloatVar legsHealth;
    public FloatVar legsMovementSpeed;
    public FloatVar legsCooldown;

    // Called whenever the player modifies their build (picks up a trinket, swaps a limb)
    public void ResetValues()
    {
        // Set all values to 0 so trinkets can write their values to them
        coreMaxHealth.Write(0);
        coreHealth.Write(0);
        leftArmMaxHealth.Write(0);
        leftArmHealth.Write(0);
        leftArmAttackDamage.Write(0);
        leftArmAttackSpeed.Write(0);
        rightArmMaxHealth.Write(0);
        rightArmHealth.Write(0);
        rightArmAttackDamage.Write(0);
        rightArmAttackSpeed.Write(0);
        legsMaxHealth.Write(0);
        legsHealth.Write(0);
        legsMovementSpeed.Write(0);
        legsCooldown.Write(0);
    }

    // Always called after ResetValues is called and after all trinkets are done activating
    public void CalculateFinalValues()
    {
        // Adds buff value calculate by trinkets to the current base value of respective limb to produce final value
        coreMaxHealth.Write(coreMaxHealth.value + currentBaseStats.coreMaxHealth.value);
        coreHealth.Write(coreHealth.value + currentBaseStats.coreHealth.value);

        leftArmMaxHealth.Write(leftArmMaxHealth.value + currentBaseStats.leftArmMaxHealth.value);
        leftArmHealth.Write(leftArmHealth.value + currentBaseStats.leftArmHealth.value);
        leftArmAttackDamage.Write(leftArmAttackDamage.value + currentBaseStats.leftArmAttackDamage.value);
        leftArmAttackSpeed.Write(leftArmAttackSpeed.value + currentBaseStats.leftArmAttackSpeed.value);

        rightArmMaxHealth.Write(rightArmMaxHealth.value + currentBaseStats.rightArmMaxHealth.value);
        rightArmHealth.Write(rightArmHealth.value + currentBaseStats.rightArmHealth.value);
        //Debug.Log("rightArmAttackDamage.value = " + rightArmAttackDamage.value + " currentBaseStats.rightArmAttackDamage.value = " + currentBaseStats.rightArmAttackDamage.value);
        rightArmAttackDamage.Write(rightArmAttackDamage.value + currentBaseStats.rightArmAttackDamage.value);
        rightArmAttackSpeed.Write(rightArmAttackSpeed.value + currentBaseStats.rightArmAttackSpeed.value);

        legsMaxHealth.Write(legsMaxHealth.value + currentBaseStats.legsMaxHealth.value);
        legsHealth.Write(legsHealth.value + currentBaseStats.legsHealth.value);
        legsMovementSpeed.Write(legsMovementSpeed.value + currentBaseStats.legsMovementSpeed.value);
        legsCooldown.Write(legsCooldown.value + currentBaseStats.legsCooldown.value);

        if(debug == true)
        {
            Debug.Log("////////// BUFFED STATS ////////// ");
            Debug.Log("coreMaxHealth: " + coreMaxHealth.value);
            Debug.Log("coreHealth: " + coreHealth.value);
            
            Debug.Log("leftArmMaxHealth: " + leftArmMaxHealth.value);
            Debug.Log("leftArmHealth: " + leftArmHealth.value);
            Debug.Log("leftArmAttackDamage: " + leftArmAttackDamage.value);
            Debug.Log("leftArmAttackSpeed: " + leftArmAttackSpeed.value);
            
            Debug.Log("rightArmMaxHealth: " + rightArmMaxHealth.value);
            Debug.Log("rightArmHealth: " + rightArmHealth.value);
            Debug.Log("rightArmAttackDamage: " + rightArmAttackDamage.value);
            Debug.Log("rightArmAttackSpeed: " + rightArmAttackSpeed.value);
          
            Debug.Log("legsMaxHealth: " + legsMaxHealth.value);
            Debug.Log("legsHealth: " + legsHealth.value);
            Debug.Log("legsMovementSpeed: " + legsMovementSpeed.value);
            Debug.Log("legsCooldown: " + legsCooldown.value);
            Debug.Log("////////// ////////// ////////// ");
        }//debugging
    }

}
