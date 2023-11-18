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
    private bool HeadEquipped;
    private bool LegsEquipped;

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
        float HeadMaxHealth = PlayerController.Instance.currentHead.MaxHealth;

        CoreHealth = PlayerController.Instance.Core.Health / CoreHealthMax;
        LegsHealth = PlayerController.Instance.currentLegs.Health / LegsHealthMax;
        //HeadHealth = PlayerController.Instance.Head.Health;

        CoreImage.color = gradient.Evaluate(CoreHealth);

        if (LegsEquipped)
        {
            if(LegsHealth > 0)
            {
                LegsImage.color = gradient.Evaluate(LegsHealth);
            }

            if(LegsHealth == 0)
            {
                LegsImage.color = Color.white;
                LegsEquipped = false;
            }
        }
        



        //HeadImage.color = gradient.Evaluate();

        if (LArmEquipped)
        {
            float LeftArmHealthMax = PlayerController.Instance.currentLeftArm.MaxHealth;
            LeftArmHealth = PlayerController.Instance.currentLeftArm.Health / LeftArmHealthMax;
            LeftArmImage.color = gradient.Evaluate(LeftArmHealth);

            if(LeftArmHealth == 0) 
            {
                LeftArmImage.color = Color.white;
                LArmEquipped = false;
            }
        }
        if (RArmEquipped)
        {
            float RightArmHealthMax = PlayerController.Instance.currentRightArm.MaxHealth;
            RightArmHealth = PlayerController.Instance.currentRightArm.Health / RightArmHealthMax;
            RightArmImage.color = gradient.Evaluate(RightArmHealth);

            if (RightArmHealth == 0)
            {
                RightArmImage.color = Color.white;
                RArmEquipped = false;
            }
        }
    }

    void Update()
    {
        SetHealth();
        OnLimbEquipped();
        CheckLimbSwap();

        if(Input.GetKeyDown(KeyCode.I)) 
        {
            PlayerController.Instance.currentLeftArm.Health -= 10;
            PlayerController.Instance.currentRightArm.Health -= 10;
            PlayerController.Instance.currentLegs.Health -= 10;
        }
    }


    void OnLimbEquipped()
    {

        if (PlayerController.Instance.currentLeftArm.Classification.ToString() != "Core")
        {
            LArmEquipped = true;
        }

        if (PlayerController.Instance.currentRightArm.Classification.ToString() != "Core")
        {
            RArmEquipped = true;
        }

        if (PlayerController.Instance.currentLegs.Classification.ToString() != "Core")
        {
            LegsEquipped = true;  
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
