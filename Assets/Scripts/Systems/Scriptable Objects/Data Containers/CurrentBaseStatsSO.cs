using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Player Data/CurrentBaseStatsSO", order = 1)]
public class CurrentBaseStatsSO : ScriptableObject
{
    // THE PLAYER WRITES TO THESE STATS

    //private Head head;
    private Core core;
    private Arm leftArm;
    private Arm rightArm;
    private Legs legs;


    // SO References updated by the references above

    //Add head SOs here

    [Header("Current base core stats:")]
    public FloatVar coreMaxHealth;
    public FloatVar coreHealth;

    [Header("Current base left arm stats:")]
    public ClassificationVar leftArmClass;
    public FloatVar leftArmMaxHealth;
    public FloatVar leftArmHealth;
    public FloatVar leftArmAttackDamage;
    public FloatVar leftArmAttackSpeed;

    [Header("Current base right arm stats:")]
    public ClassificationVar rightArmClass;
    public FloatVar rightArmMaxHealth;
    public FloatVar rightArmHealth;
    public FloatVar rightArmAttackDamage;
    public FloatVar rightArmAttackSpeed;

    [Header("Current base legs stats:")]
    public ClassificationVar legsClass;
    public FloatVar legsMaxHealth;
    public FloatVar legsHealth;
    public FloatVar legsMovementSpeed;
    public FloatVar legsCooldown;

    // Called anytime the player swaps out a limb (including when switching L/R arms)
    public void UpdateCurrentBuild(Core currentCore, Arm currentLeftArm, Arm currentRightArm, Legs currentLegs)
    {
        core = currentCore;
        leftArm = currentLeftArm;
        rightArm = currentRightArm;
        legs = currentLegs;

        //UpdateHead();
        UpdateCore();
        UpdateLeftArm();
        UpdateRightArm();
        UpdateLegs();
    }

    private void UpdateCore()
    {
        coreMaxHealth.Write(core.MaxHealth);
        coreHealth.Write(core.Health);
    }

    private void UpdateLeftArm()
    {
        leftArmClass.Write(leftArm.Classification);
        leftArmMaxHealth.Write(leftArm.MaxHealth);
        leftArmHealth.Write(leftArm.Health);
        leftArmAttackDamage.Write(leftArm.AttackDamage);
        leftArmAttackSpeed.Write(leftArm.AttackSpeed);
    }

    private void UpdateRightArm()
    {
        rightArmClass.Write(rightArm.Classification);
        rightArmMaxHealth.Write(rightArm.MaxHealth);
        rightArmHealth.Write(rightArm.Health);
        rightArmAttackDamage.Write(rightArm.AttackDamage);
        rightArmAttackSpeed.Write(rightArm.AttackSpeed); 
    }

    private void UpdateLegs()
    {
        legsClass.Write(legs.Classification);
        legsMaxHealth.Write(legs.MaxHealth);
        legsHealth.Write(legs.Health);
        legsMovementSpeed.Write(legs.MovementSpeed);
        legsCooldown.Write(legs.CooldownTime);
    }

    //Update Conditionals function
}
