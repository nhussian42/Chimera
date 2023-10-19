using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TrinketManager : Singleton<TrinketManager>
{
    public PlayerController PlayerController;
    public TrinketMenu TrinketMenu;

    private bool hyenaJaw = false;
    private bool feedingFrenzy = false;
    private bool bearClaw = false;
    private float killSkillDuration = 2;
    

    public Arm LeftArm { get; private set; }
    public Arm RightArm { get; private set; }

    private float coreHealth;
    public bool canFrenzy = false;
    private bool hasFrenzied = false;
    private bool HJTier1 = false;
    private bool HJTier2 = false;

    // Start is called before the first frame update

    public void Start()
    {
        LeftArm = PlayerController.Instance.currentLeftArm;
        RightArm = PlayerController.Instance.currentRightArm;
    }

    public void Update()
    { 
        coreHealth = PlayerController.Instance.CoreHealth;

        if (hyenaJaw) HyenaJaw();



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

    public void EnableFeedingFrenzy()
    {
        canFrenzy = true;
        Debug.Log("frenzy enabled");
    }

    public void FeedingFrenzy(bool Frenzy) //Increases damage after a kill (WIP)
    {
        TrinketManager.Instance.canFrenzy = canFrenzy;
        Debug.Log(canFrenzy);
        if(canFrenzy == true) {

            if (Frenzy == true)
            {
                PlayerController.Instance.currentLeftArm.UpdateAttackDamage(5);
                PlayerController.Instance.currentRightArm.UpdateAttackDamage(5);
                StartCoroutine(KillSkillDuration());
                hasFrenzied = true;
            }

            if (Frenzy == false && hasFrenzied == true)
            {
                PlayerController.Instance.currentLeftArm.UpdateAttackDamage(-5);
                PlayerController.Instance.currentRightArm.UpdateAttackDamage(-5);
            }
        }
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

        if ((coreHealth < 50) && (coreHealth > 25) && HJTier1 == false)
        {
            //LeftArm.UpdateAttackDamage(10);
            //RightArm.UpdateAttackDamage(10);
            PlayerController.Instance.currentLeftArm.UpdateAttackDamage(10);
            PlayerController.Instance.currentRightArm.UpdateAttackDamage(10);
            HJTier1 = true;         

        }
        if (coreHealth < 25 && HJTier2 == false)
        {
            //LeftArm.UpdateAttackDamage(10);
            //RightArm.UpdateAttackDamage(10);
            PlayerController.Instance.currentLeftArm.UpdateAttackDamage(10);
            PlayerController.Instance.currentRightArm.UpdateAttackDamage(10);
            HJTier2 = true; 
  
        }

        if (coreHealth > 50 && HJTier1)
        {
            PlayerController.Instance.currentLeftArm.UpdateAttackDamage(-10);
            PlayerController.Instance.currentRightArm.UpdateAttackDamage(-10);
            HJTier1 = false;
        }

        if ((coreHealth < 50) && (coreHealth > 25) && HJTier2)
        {
            PlayerController.Instance.currentLeftArm.UpdateAttackDamage(-10);
            PlayerController.Instance.currentRightArm.UpdateAttackDamage(-10);
            HJTier2 = false;
        }

    }

    public void Scavenger() //Increases bone drops (WIP)
    {
        //PlayerController.Instance.BonesMultiplier = 1.05;
    }

    public void StartKillSkills()
    {
        FeedingFrenzy(Frenzy: true);
    }

    private IEnumerator KillSkillDuration()
    {
        yield return new WaitForSeconds(killSkillDuration);
        if (hasFrenzied)
        {
            FeedingFrenzy(Frenzy: false);
        }
    }

}
