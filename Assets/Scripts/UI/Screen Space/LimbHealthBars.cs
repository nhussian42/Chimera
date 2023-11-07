using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LimbHealthBars : MonoBehaviour
{

    public Gradient gradient;
    public Image CoreImage;
    public Image LegsImage;
    public Image RightArmImage;
    public Image LeftArmImage;
    public Image HeadImage;

    private bool LArmEquipped;
    private bool RArmEquipped;

    private float CoreHealth;
    private float LegsHealth;
    private float RightArmHealth;
    private float LeftArmHealth;
    private float HeadHealth;

    public void SetHealth()
    {
        //gets max health of limbs and current health to set the gradient

        float CoreHealthMax = PlayerController.Instance.Core.MaxHealth;
        float LegsHealthMax = PlayerController.Instance.currentLegs.MaxHealth;
        
        
        //float HeadMaxHealth = PlayerController.Instance.currentHead.MaxHealth;
        CoreHealth = PlayerController.Instance.Core.Health / CoreHealthMax;
        LegsHealth = PlayerController.Instance.currentLegs.Health / LegsHealthMax;

        
        
        //CoreHealth = PlayerController.Instance.Head.Health;

        CoreImage.color = gradient.Evaluate(CoreHealth);
        //LegsImage.color = gradient.Evaluate(LegsHealth);
       
        
        //HeadImage.color = gradient.Evaluate();

        if (LArmEquipped)
        {
            float LeftArmHealthMax = PlayerController.Instance.currentLeftArm.MaxHealth;
            LeftArmHealth = PlayerController.Instance.currentLeftArm.Health / LeftArmHealthMax;
            LeftArmImage.color = gradient.Evaluate(LeftArmHealth);
        }
        if (RArmEquipped)
        {
            float RightArmHealthMax = PlayerController.Instance.currentRightArm.MaxHealth;
            RightArmHealth = PlayerController.Instance.currentRightArm.Health / RightArmHealthMax;
            RightArmImage.color = gradient.Evaluate(RightArmHealth);
        }
    }

    void Update()
    {
        SetHealth();
        OnLimbEquipped();
        CheckLimbSwap();
    }


    void OnLimbEquipped()
    {

        if (PlayerController.Instance.currentLeftArm.Classification.ToString() != "Core")
        {
            LArmEquipped = true;
            //Checks for Both limbs just in case
            if (PlayerController.Instance.currentRightArm.Classification.ToString() != "Core")
            {
                RArmEquipped = true;
            }
        }

        else if (PlayerController.Instance.currentRightArm.Classification.ToString() != "Core")
        {
            RArmEquipped = true;
            if (PlayerController.Instance.currentLeftArm.Classification.ToString() != "Core")
            {
                LArmEquipped = true;
            }
        }
        
        else return;
    }

    void CheckLimbSwap()
    {
        if (PlayerController.Instance.currentLeftArm.Classification.ToString() == "Core")
        {
            LArmEquipped = false;
            LeftArmImage.color = Color.white;
        }
        else if (PlayerController.Instance.currentRightArm.Classification.ToString() == "Core")
        {
            RArmEquipped = false;
            RightArmImage.color = Color.white;
        }
     }
}
