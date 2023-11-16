using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class PlayerInvSlot : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject armL;
    [SerializeField] private GameObject armR;
    [SerializeField] private GameObject core;
    [SerializeField] private GameObject legs;
    [SerializeField] private GameObject relic;


    [SerializeField] private TMP_Text NameBox;
    [SerializeField] private TMP_Text DescBox;
    [SerializeField] private Image ImgBox;

    [SerializeField] private GameObject LimbButtonGO;
    [SerializeField] private List<Button> LimbButtons;
    [SerializeField] private List<Button> LimbButtonHLs;


    //public event Action<PlayerInvSlot> OnLimbClicked;
    public void OnSelected(BaseEventData data)
    {
        UnityEngine.Debug.Log(data.selectedObject.name);

        switch (data.selectedObject.name)
        {
            case "Arm_L":
                NameBox.text = (PlayerController.Instance.currentLeftArm.Name.ToString() + " Arm");
                DescBox.text = (PlayerController.Instance.currentLeftArm.Health.ToString("F0") + " / " 
                    + PlayerController.Instance.currentLeftArm.MaxHealth.ToString() + " Health\v" 
                    + PlayerController.Instance.currentLeftArm.AttackDamage.ToString() + " Damage\v" 
                    + PlayerController.Instance.currentLeftArm.AttackSpeed.ToString("F2") + " ATK Speed");

                SetInfoSprite(PlayerController.Instance.currentLeftArm);
                break;

            case "Arm_R":
                NameBox.text = (PlayerController.Instance.currentRightArm.Name.ToString() + " Arm");
                DescBox.text = (PlayerController.Instance.currentRightArm.Health.ToString("F0") + " / "
                    + PlayerController.Instance.currentRightArm.MaxHealth.ToString() + " Health\v"
                    + PlayerController.Instance.currentRightArm.AttackDamage.ToString() + " Damage\v"
                    + PlayerController.Instance.currentRightArm.AttackSpeed.ToString("F2") + " ATK Speed");
                SetInfoSprite(PlayerController.Instance.currentRightArm);
                break;

            case "Head":
                NameBox.text = (PlayerController.Instance.currentHead.Name.ToString() + " Head");
                DescBox.text = (PlayerController.Instance.currentHead.Health.ToString("F0") + " / "
                    + PlayerController.Instance.currentHead.MaxHealth.ToString() + " Health\v");
                SetInfoSprite(PlayerController.Instance.currentHead);
                break;
            case "Core":
                NameBox.text = "Chimera Core";
                DescBox.text = (PlayerController.Instance.Core.Health.ToString("F0") + " / "
                    + PlayerController.Instance.Core.MaxHealth.ToString() + " Health\v");
                SetInfoSprite(PlayerController.Instance.Core);
                break;
            case "Legs":
                NameBox.text = (PlayerController.Instance.currentLegs.Name.ToString() + " Legs");
                DescBox.text = (PlayerController.Instance.currentLegs.Health.ToString("F0") + " / "
                + PlayerController.Instance.currentLegs.MaxHealth.ToString() + " Health\v" 
                + PlayerController.Instance.currentLegs.CooldownTime.ToString("F1") + " Dash Cooldown");
                SetInfoSprite(PlayerController.Instance.currentLegs);
                break;
            case "Relic":
                UnityEngine.Debug.Log("Relic");
                NameBox.text = "Relic Name";
                DescBox.text = "Relic Desc";
                break;
        }

    }

    public void SetLimbSprites()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(LimbButtonGO);
        

        for (int i = 0; i < LimbButtons.Count; i++)
        {
            SpriteState ss = new SpriteState();
           
            switch (LimbButtons[i].gameObject.name.ToString())
            {
                case "Arm_L":
                    LimbButtons[2].gameObject.GetComponent<Image>().sprite = PlayerController.Instance.currentLeftArm.limbSprite;
                    ss.highlightedSprite = PlayerController.Instance.currentLeftArm.selectedSprite;
                    ss.selectedSprite = PlayerController.Instance.currentLeftArm.selectedSprite;
                    LimbButtons[2].gameObject.GetComponent<Button>().spriteState = ss;
                    LimbButtonHLs[2].gameObject.GetComponent<Image>().sprite = PlayerController.Instance.currentLeftArm.limbSprite;
                    break;

                case "Arm_R":
                    LimbButtons[3].gameObject.GetComponent<Image>().sprite = PlayerController.Instance.currentRightArm.limbSprite;
                    ss.highlightedSprite = PlayerController.Instance.currentRightArm.selectedSprite;
                    ss.selectedSprite = PlayerController.Instance.currentRightArm.selectedSprite;
                    LimbButtons[3].gameObject.GetComponent<Button>().spriteState = ss;
                    LimbButtonHLs[3].gameObject.GetComponent<Image>().sprite = PlayerController.Instance.currentRightArm.limbSprite;
                    break;

                case "Head":
                    LimbButtons[1].GetComponent<Image>().sprite = PlayerController.Instance.currentHead.limbSprite;
                    ss.highlightedSprite = PlayerController.Instance.currentHead.selectedSprite;
                    ss.selectedSprite = PlayerController.Instance.currentHead.selectedSprite;
                    LimbButtons[1].gameObject.GetComponent<Button>().spriteState = ss;
                    LimbButtonHLs[1].GetComponent<Image>().sprite = PlayerController.Instance.currentHead.limbSprite;
                    break;

                case "Core":
                    LimbButtons[0].GetComponent<Image>().sprite = PlayerController.Instance.Core.limbSprite;
                    ss.highlightedSprite = PlayerController.Instance.Core.selectedSprite;
                    ss.selectedSprite = PlayerController.Instance.Core.selectedSprite;
                    LimbButtons[0].gameObject.GetComponent<Button>().spriteState = ss;
                    LimbButtonHLs[0].GetComponent<Image>().sprite = PlayerController.Instance.Core.limbSprite;
                    break;

                case "Legs":
                    LimbButtons[4].GetComponent<Image>().sprite = PlayerController.Instance.currentLegs.limbSprite;
                    ss.highlightedSprite = PlayerController.Instance.currentLegs.selectedSprite;
                    ss.selectedSprite = PlayerController.Instance.currentLegs.selectedSprite;
                    LimbButtons[4].gameObject.GetComponent<Button>().spriteState = ss;
                    LimbButtonHLs[4].GetComponent<Image>().sprite = PlayerController.Instance.currentLegs.limbSprite;
                    break;

            }
        }
    }

    public void SetInfoSprite(Limb limb)
    {
        ImgBox.gameObject.SetActive(true);
        var limbSprite = limb.limbSprite;

        ImgBox.sprite = limbSprite;
    }


}
