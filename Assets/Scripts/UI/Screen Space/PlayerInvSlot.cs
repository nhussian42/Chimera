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
    [SerializeField] private List<Image> limbImgList;

    [SerializeField] private GameObject LimbButtonGO;
    [SerializeField] private List<Button> LimbButtons;
    [SerializeField] private List<Button> LimbButtonHLs;
    [SerializeField] private Image RelicHighlight;
    [SerializeField] private Image ScrollHighlight;

    [SerializeField] private MasterTrinketList masterTrinketList;

    //public event Action<PlayerInvSlot> OnLimbClicked;

    private void Update()
    {
        UnityEngine.Debug.Log(EventSystem.current.currentSelectedGameObject);
        if(masterTrinketList.Relic != null)
        {
            relic.gameObject.SetActive(true);
        }

    }
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

                SetInfoSprite(PlayerController.Instance.currentLeftArm, 2);
                break;

            case "Arm_R":
                NameBox.text = (PlayerController.Instance.currentRightArm.Name.ToString() + " Arm");
                DescBox.text = (PlayerController.Instance.currentRightArm.Health.ToString("F0") + " / "
                    + PlayerController.Instance.currentRightArm.MaxHealth.ToString() + " Health\v"
                    + PlayerController.Instance.currentRightArm.AttackDamage.ToString() + " Damage\v"
                    + PlayerController.Instance.currentRightArm.AttackSpeed.ToString("F2") + " ATK Speed");
                SetInfoSprite(PlayerController.Instance.currentRightArm, 1);
                break;

            case "Head":
                NameBox.text = (PlayerController.Instance.currentHead.Name.ToString() + " Head");
                DescBox.text = (PlayerController.Instance.currentHead.Health.ToString("F0") + " / "
                    + PlayerController.Instance.currentHead.MaxHealth.ToString() + " Health\v");
                SetInfoSprite(PlayerController.Instance.currentHead, 3);
                break;
            case "Core":
                NameBox.text = "Chimera Core";
                DescBox.text = (PlayerController.Instance.Core.Health.ToString("F0") + " / "
                    + PlayerController.Instance.Core.MaxHealth.ToString() + " Health\v");
                SetInfoSprite(PlayerController.Instance.Core, 0);
                break;
            case "Legs":
                NameBox.text = (PlayerController.Instance.currentLegs.Name.ToString() + " Legs");
                DescBox.text = (PlayerController.Instance.currentLegs.Health.ToString("F0") + " / "
                + PlayerController.Instance.currentLegs.MaxHealth.ToString() + " Health\v" 
                + PlayerController.Instance.currentLegs.CooldownTime.ToString("F1") + " Dash Cooldown");
                SetInfoSprite(PlayerController.Instance.currentLegs, 4);
                break;
            case "Relic":
                ResetLimbImg();
                if(masterTrinketList.Relic == null)
                {
                    NameBox.text = "Relic Slot";
                    DescBox.text = "There is no Relic Currently Equipped";
                    ImgBox.gameObject.SetActive(false);
                    break;
                }
                else
                {
                    //ImgBox.gameObject.SetActive(true);
                    NameBox.text = masterTrinketList.Relic.TrinketName.ToString();
                    DescBox.text = masterTrinketList.Relic.Description.ToString();
                    SetRelicSprite(masterTrinketList.Relic);
                    break;
                }
                
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

                case "Relic":
                    if(masterTrinketList.Relic == null)
                    {                   
                        LimbButtons[5].GetComponent<Image>().sprite = null;
                        ss.selectedSprite = RelicHighlight.sprite;
                        LimbButtons[5].gameObject.GetComponent<Button>().spriteState = ss;
                        RelicHighlight.GetComponent<Image>().sprite = null;
                        break;
                    }
                    else
                    {
                        LimbButtons[5].GetComponent<Image>().sprite = masterTrinketList.Relic.Icon;
                        break;
                    }
                    
            }
        }
    }

    public void SetInfoSprite(Limb limb, int i)
    {
        var limbSprite = limb.limbSprite;
        ImgBox.gameObject.SetActive(false);
        switch (i)
        {
            
            case 0:
                {
                    ResetLimbImg();
                    limbImgList[i].gameObject.SetActive(true);
                    limbImgList[i].sprite = limbSprite;
                    break;
                }
            case 1:
                {
                    ResetLimbImg();
                    limbImgList[i].gameObject.SetActive(true);
                    limbImgList[i].sprite = limbSprite;
                    break;
                }
            case 2:
                {
                    ResetLimbImg();
                    limbImgList[i].gameObject.SetActive(true);
                    limbImgList[i].sprite = limbSprite;
                    break;
                }
            case 3:
                {
                    ResetLimbImg();
                    limbImgList[i].gameObject.SetActive(true);
                    limbImgList[i].sprite = limbSprite;
                    break;
                }
            case 4:
                {
                    ResetLimbImg();
                    limbImgList[i].gameObject.SetActive(true);
                    limbImgList[i].sprite = limbSprite;
                    break;
                }
        }

    }

    public void SetRelicSprite(Trinket trinket)
    {
        ResetLimbImg();
        ImgBox.gameObject.SetActive(true);
        var trinketIcon = trinket.Icon;

        ImgBox.sprite = trinketIcon;
    }

    public void ResetLimbImg()
    {
        for (int i = 0; i < limbImgList.Count; i++)
        {
            limbImgList[i].gameObject.SetActive(false);
        }
    }

    public void ToggleHighlightRelic()
    {
        RelicHighlight.gameObject.SetActive(!RelicHighlight.gameObject.activeSelf);
    }

    public void ToggleHighlightScroll()
    {
        ScrollHighlight.gameObject.SetActive(!ScrollHighlight.gameObject.activeSelf);
    }
}
