using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public TrinketMenu TrinketMenu;

    private bool hyenaJaw = false;
    private bool feedingFrenzy = false;

    public Arm LeftArm { get; private set; }
    public Arm RightArm { get; private set; }

    private float coreHealth;
    private bool HJHasRun = false;

    // Start is called before the first frame update

    public void Start()
    {
        LeftArm = PlayerController.Instance.currentLeftArm;
        RightArm = PlayerController.Instance.currentRightArm;
    }

    public void Update()
    {



        coreHealth = PlayerController.Instance.CoreHealth;
        if (hyenaJaw) // Actively updates damage based on health (WIP) - Doesn't reset on healing
        {
            if((coreHealth < 50) && (coreHealth > 25) && HJHasRun == false)
            {
                //LeftArm.UpdateAttackDamage(10);
                //RightArm.UpdateAttackDamage(10);
                PlayerController.Instance.currentLeftArm.UpdateAttackDamage(10);
                PlayerController.Instance.currentRightArm.UpdateAttackDamage(10);
                HJHasRun = true;
            }
            if(coreHealth < 25 && HJHasRun == true) 
            {
                //LeftArm.UpdateAttackDamage(10);
                //RightArm.UpdateAttackDamage(10);
                PlayerController.Instance.currentLeftArm.UpdateAttackDamage(10);
                PlayerController.Instance.currentRightArm.UpdateAttackDamage(10);
                HJHasRun = false;
            }
        }
    }

    public void LizardClaw() //Increases Attack Damage
    {
        //LeftArm.UpdateAttackDamage(5); 
        //RightArm.UpdateAttackDamage(5);
        PlayerController.Instance.currentLeftArm.UpdateAttackDamage(5);
        PlayerController.Instance.currentRightArm.UpdateAttackDamage(5);
    }

    public void BirdTalon() //Increases Attack Speed
    {
        //LeftArm.UpdateAttackSpeed(0.05f);
        //RightArm.UpdateAttackSpeed(0.05f);
        PlayerController.Instance.currentLeftArm.UpdateAttackSpeed(0.05f);
        PlayerController.Instance.currentRightArm.UpdateAttackSpeed(0.05f);
    }

    public void TuftOfFur() //Increases Max Health (WIP for core)
    {

        //LeftArm.UpdateMaxHealth(5);
        //RightArm.UpdateMaxHealth(5);
        PlayerController.Instance.currentLeftArm.UpdateMaxHealth(5);
        PlayerController.Instance.currentRightArm.UpdateMaxHealth(5);
        PlayerController.Instance.UpdateCoreHealth(5);
        //RightArm.DebugLog();
    }

    public void BearClaw() //Increases damage on every 3rd hit (WIP)
    {

    }

    public void FeedingFrenzy() //Increases damage after a kill (WIP)
    {
        feedingFrenzy = true;
    }

    public void PlumpMushroom() //Heals Player after room clear (WIP)
    {
        //LeftArm.UpdateCurrentHealth(5);
        //RightArm.UpdateCurrentHealth(5);
        PlayerController.Instance.currentLeftArm.UpdateCurrentHealth(5);
        PlayerController.Instance.currentRightArm.UpdateCurrentHealth(5);
        PlayerController.Instance.UpdateCoreHealth(5);

    }

    public void MulesKick() //Increases Player Move Speed 
    {
        PlayerController.Instance.MulesKick(1);
    }

    public void HyenaJaw() //Increases damage when core gets low (WIP)
    {
        hyenaJaw = true;
    }

    public void Scavenger() //Increases bone drops (WIP)
    {
        //PlayerController.Instance.BonesMultiplier = 1.05;
    }

}
