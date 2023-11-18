using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class LimbSwapMenu : MonoBehaviour
{
    private PlayerController playerController;

    [Header("UI Elements")]
    [SerializeField] private Button equipButton;
    [SerializeField] private TextMeshProUGUI currentLimbName;
    [SerializeField] private TextMeshProUGUI proposedLimbName;

    [SerializeField] private Image currentHeadIcon;
    [SerializeField] private Image currentLeftArmIcon;
    [SerializeField] private Image currentRightArmIcon;
    [SerializeField] private Image currentLegsIcon;
    [SerializeField] private Image proposedHeadIcon;
    [SerializeField] private Image proposedLeftArmIcon;
    [SerializeField] private Image proposedRightArmIcon;
    [SerializeField] private Image proposedLegsIcon;

    [SerializeField] private TextMeshProUGUI statText1; // 1-3 reserved for current limb stats
    [SerializeField] private TextMeshProUGUI statText2;
    [SerializeField] private TextMeshProUGUI statText3;
    [SerializeField] private TextMeshProUGUI statText4; // 4-6 reserved for proposed limb stats
    [SerializeField] private TextMeshProUGUI statText5;
    [SerializeField] private TextMeshProUGUI statText6;

    [SerializeField] private Color upgradeColor;
    [SerializeField] private Color downgradeColor;

    [SerializeField] private TextMeshProUGUI scrapText;
    [SerializeField] private TextMeshProUGUI switchArmText;

    public LimbType proposedLimbType { get; private set; }   
    private LimbDrop proposedLimbDrop;
    private SideOfPlayer displayedArm;

    private void Start()
    {
        playerController = PlayerController.Instance;
        playerController.limbSwapMenu = this;
        gameObject.SetActive(false);
        //Debug.Log("Called Start() on LimbSwapMenu");
    }

    public void Enable(LimbDrop proposedLimb)
    {
        proposedLimbDrop = proposedLimb;
        SetMenu();
        scrapText.text = "Scrap(50)";
        gameObject.SetActive(true);
        playerController.DisableAllDefaultControls();
        playerController.EnableAllUIControls();
        EventSystem.current.SetSelectedGameObject(equipButton.gameObject);
    }

    // Determines which limb swap menu variant will display
    private void SetMenu()
    {
        switch (proposedLimbDrop.LimbType)
        {
            case LimbType.Head:
                {
                    proposedLimbType = LimbType.Head;
                    switchArmText.gameObject.SetActive(false);
                    SetToHeads();
                    break;
                }
            case LimbType.Arm:
                {
                    proposedLimbType = LimbType.Arm;
                    switchArmText.gameObject.SetActive(true);
                    SetToLeftArm();
                    break;
                }
            case LimbType.Legs:
                {
                    proposedLimbType = LimbType.Legs;
                    switchArmText.gameObject.SetActive(false);
                    SetToLegs();
                    break;
                }
        }
    }

    // Heads variant
    private void SetToHeads()
    {
        foreach (Head head in playerController.allHeads)
        {
            if (head.Classification == proposedLimbDrop.Classification && head.Weight == proposedLimbDrop.Weight)
            {
                currentLimbName.text = playerController.currentHead.StringName;
                proposedLimbName.text = head.StringName;

                // Check if the health value on the limb has been altered, if not use the default max health value on the limb
                float headHealth = proposedLimbDrop.LimbHealth;
                if (proposedLimbDrop.LimbHealth <= 0)
                    headHealth = head.DefaultMaxHealth;

                currentHeadIcon.gameObject.SetActive(true);
                currentHeadIcon.sprite = playerController.currentHead.LimbSprite;
                currentLeftArmIcon.gameObject.SetActive(false);
                currentRightArmIcon.gameObject.SetActive(false);
                currentLegsIcon.gameObject.SetActive(false);
                proposedHeadIcon.gameObject.SetActive(true);
                proposedHeadIcon.sprite = head.LimbSprite;
                proposedLeftArmIcon.gameObject.SetActive(false);
                proposedRightArmIcon.gameObject.SetActive(false);
                proposedLegsIcon.gameObject.SetActive(false);

                // POSSIBLE BUG HERE: if trinkets buff max health and current health is > the default max health, the fraction will be wrong for this stat
                statText1.text = "HP: " + "(" + playerController.currentHead.Health + "/" + playerController.currentHead.DefaultMaxHealth + ")"; // current head's HP (current/maximum)
                statText2.text = "";
                statText3.text = "";
                statText4.text = "HP: " + "(" + headHealth + "/" + head.DefaultMaxHealth + ")"; // proposed head's HP
                statText5.text = "";
                statText6.text = "";

                // set scrap text here later with the bone value attached to the instance of the proposed limb
                break;
            }
        }
    }

    // Left Arm variant
    public void SetToLeftArm()
    {

        displayedArm = SideOfPlayer.Left;

        foreach (Arm arm in playerController.allArms)
        {
            if (arm.Classification == proposedLimbDrop.Classification && arm.Weight == proposedLimbDrop.Weight && arm.Side == SideOfPlayer.Left)
            {
                // Set these to the exposed Name attribute of the limbs later
                currentLimbName.text = playerController.currentLeftArm.StringName + "(L)";
                proposedLimbName.text = arm.StringName;

                // Check if the health value on the limb has been altered, if not use the default max health value on the limb
                float armHealth = proposedLimbDrop.LimbHealth;
                if (proposedLimbDrop.LimbHealth <= 0)
                    armHealth = arm.DefaultMaxHealth;

                currentHeadIcon.gameObject.SetActive(false);
                currentLeftArmIcon.gameObject.SetActive(true);
                currentLeftArmIcon.sprite = playerController.currentLeftArm.LimbSprite;
                currentRightArmIcon.gameObject.SetActive(false);
                currentLegsIcon.gameObject.SetActive(false);
                proposedHeadIcon.gameObject.SetActive(false);
                proposedLeftArmIcon.gameObject.SetActive(true);
                proposedLeftArmIcon.sprite = arm.LimbSprite;
                proposedRightArmIcon.gameObject.SetActive(false);
                proposedLegsIcon.gameObject.SetActive(false);

                // POSSIBLE BUG HERE: if trinkets buff max health and current health is > the default max health, the fraction will be wrong for this stat
                statText1.text = "HP: " + "(" + playerController.currentLeftArm.Health + "/" + playerController.currentLeftArm.DefaultMaxHealth + ")";
                statText2.text = "ATK: " + playerController.currentLeftArm.DefaultAttackDamage;
                statText3.text = "SPD: " + playerController.currentLeftArm.DefaultAttackSpeed;
                statText4.text = "HP: " + "(" + armHealth + "/" + arm.DefaultMaxHealth + ")";
                statText5.text = "ATK: " + arm.DefaultAttackDamage;
                statText6.text = "SPD: " + arm.DefaultAttackSpeed;

                // set scrap text here later with the bone value attached to the instance of the proposed limb
                break;
            }
        }


    }

    // Right Arm variant
    public void SetToRightArm()
    {

        displayedArm = SideOfPlayer.Right;

        foreach (Arm arm in playerController.allArms)
        {
            if (arm.Classification == proposedLimbDrop.Classification && arm.Weight == proposedLimbDrop.Weight && arm.Side == SideOfPlayer.Right)
            {
                // Set these to the exposed Name attribute of the limbs later
                currentLimbName.text = playerController.currentRightArm.StringName + "(R)";
                proposedLimbName.text = arm.StringName;

                // Check if the health value on the limb has been altered, if not use the default max health value on the limb
                float armHealth = proposedLimbDrop.LimbHealth;
                if (proposedLimbDrop.LimbHealth <= 0)
                    armHealth = arm.DefaultMaxHealth;

                currentHeadIcon.gameObject.SetActive(false);
                currentLeftArmIcon.gameObject.SetActive(false);
                currentRightArmIcon.gameObject.SetActive(true);
                currentRightArmIcon.sprite = playerController.currentRightArm.LimbSprite;
                currentLegsIcon.gameObject.SetActive(false);
                proposedHeadIcon.gameObject.SetActive(false);
                proposedLeftArmIcon.gameObject.SetActive(false);
                proposedRightArmIcon.gameObject.SetActive(true);
                proposedRightArmIcon.sprite = arm.LimbSprite;
                proposedLegsIcon.gameObject.SetActive(false);

                // POSSIBLE BUG HERE: if trinkets buff max health and current health is > the default max health, the fraction will be wrong for this stat
                statText1.text = "HP: " + "(" + playerController.currentRightArm.Health + "/" + playerController.currentRightArm.DefaultMaxHealth + ")";
                statText2.text = "ATK: " + playerController.currentRightArm.DefaultAttackDamage;
                statText3.text = "SPD: " + playerController.currentRightArm.DefaultAttackSpeed;
                statText4.text = "HP: " + "(" + armHealth + "/" + arm.DefaultMaxHealth + ")";
                statText5.text = "ATK: " + arm.DefaultAttackDamage;
                statText6.text = "SPD: " + arm.DefaultAttackSpeed;

                // set scrap text here later with the bone value attached to the instance of the proposed limb
                break;
            }
        }

    }

    // Called to switch which arm is displayed (Toggle Function)
    //public void SwitchArms()
    //{
    //    if(displayedArm == SideOfPlayer.Left)
    //    {
    //        SetToRightArm();
    //    }
    //    else
    //    {
    //        SetToLeftArm();
    //    }
    //}

    // Leg variant

    private void SetToLegs()
    {
        foreach (Legs legs in playerController.allLegs)
        {
            if (legs.Classification == proposedLimbDrop.Classification && legs.Weight == proposedLimbDrop.Weight)
            {
                // Set these to the exposed Name attribute of the limbs later
                currentLimbName.text = playerController.currentLegs.StringName;
                proposedLimbName.text = legs.StringName;

                // Check if the health value on the limb has been altered, if not use the default max health value on the limb
                float legsHealth = proposedLimbDrop.LimbHealth;
                if (proposedLimbDrop.LimbHealth <= 0)
                    legsHealth = legs.DefaultMaxHealth;

                currentHeadIcon.gameObject.SetActive(false);
                currentLeftArmIcon.gameObject.SetActive(false);
                currentRightArmIcon.gameObject.SetActive(false);
                currentLegsIcon.gameObject.SetActive(true);
                currentLegsIcon.sprite = playerController.currentLegs.LimbSprite;
                proposedHeadIcon.gameObject.SetActive(false);
                proposedLeftArmIcon.gameObject.SetActive(false);
                proposedRightArmIcon.gameObject.SetActive(false);
                proposedLegsIcon.gameObject.SetActive(true);
                proposedLegsIcon.sprite = legs.LimbSprite;

                // POSSIBLE BUG HERE: if trinkets buff max health and current health is > the default max health, the fraction will be wrong for this stat
                statText1.text = "HP: " + "(" + playerController.currentLegs.Health + "/" + playerController.currentLegs.DefaultMaxHealth + ")";
                statText2.text = "SPD: " + playerController.currentLegs.DefaultMovementSpeed;
                statText3.text = "CD: " + playerController.currentLegs.DefaultCooldownTime + " sec";
                statText4.text = "HP: " + "(" + legsHealth + "/" + legs.DefaultMaxHealth + ")";
                statText5.text = "SPD: " + legs.DefaultMovementSpeed;
                statText6.text = "CD: " + legs.DefaultCooldownTime + " sec";

                // set scrap text here later with the bone value attached to the instance of the proposed limb
                break;
            }
        }
    }

    // Called by button to equip the proposed limb
    public void EquipLimb()
    {
        switch (proposedLimbType)
        {
            case LimbType.Head:
                {
                    playerController.SwapLimb(playerController.currentHead, proposedLimbDrop);
                    //PlayerController.OnHeadSwapped?.Invoke(); (anticipated event)
                    break;
                }
            case LimbType.Arm:
                { 
                    if(displayedArm == SideOfPlayer.Right)
                    {
                        playerController.SwapLimb(playerController.currentRightArm, proposedLimbDrop);
                    }
                    else if(displayedArm == SideOfPlayer.Left)
                    {
                        playerController.SwapLimb(playerController.currentLeftArm, proposedLimbDrop);
                    }
                    PlayerController.OnArmSwapped?.Invoke();
                    break;
                }
            case LimbType.Legs:
                {
                    playerController.SwapLimb(playerController.currentLegs, proposedLimbDrop);
                    //PlayerController.OnLegsSwapped?.Invoke(); (anticipated event)
                    break;
                }
        }
        Destroy(proposedLimbDrop.gameObject);
        playerController.EnableAllDefaultControls();
    }

    // Called by button to scrap the proposed limb
    public void ScrapLimb()
    {
        playerController.AddBones(50); //replace with the exposed amount on the proposed limb
        Destroy(proposedLimbDrop.gameObject);
        playerController.EnableAllDefaultControls();
    }

    private float Truncate(float value)
    {
        value = Mathf.RoundToInt(value * 10);
        value /= 10;
        return value;

    }

    public void Exit()
    {
        playerController.EnableAllDefaultControls();
        playerController.DisableAllUIControls();
    }
}
