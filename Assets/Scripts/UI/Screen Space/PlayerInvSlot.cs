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


    //public event Action<PlayerInvSlot> OnLimbClicked;

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            //OnLimbClicked?.Invoke(this);
        }

        switch (pointerData.pointerCurrentRaycast.gameObject.name)
        {
            case "Arm_L":
                UnityEngine.Debug.Log("ARM_L");    
                NameBox.text = (PlayerController.Instance.currentLeftArm.Name.ToString() + " Arm");
                DescBox.text = "Left Arm Desc";
                SetSprite(PlayerController.Instance.currentLeftArm.Name, LimbType.Arm, PlayerController.Instance.currentLeftArm);
                break;              
                
            case "Arm_R":
                UnityEngine.Debug.Log("ARM_R");
                NameBox.text = (PlayerController.Instance.currentRightArm.Name.ToString() + " Arm");
                DescBox.text = "Right Arm Desc";
                SetSprite(PlayerController.Instance.currentRightArm.Name, LimbType.Arm, PlayerController.Instance.currentRightArm);
                break;

            case "Head":
                NameBox.text = "Head Name";
                //NameBox.text = (PlayerController.Instance.currentHead.Name.ToString() + " Arm");
                DescBox.text = "Head Desc";
                break;
            case "Core":
                NameBox.text = "Chimera Core";
                DescBox.text = "Core Desc";
                break;
            case "Legs":
                NameBox.text = (PlayerController.Instance.currentLegs.Name.ToString() + " Legs");
                DescBox.text = "Legs Desc";
                break;
            case "Relic":
                UnityEngine.Debug.Log("Relic");
                NameBox.text = "Relic Name";
                DescBox.text = "Relic Desc";
                break;
                //if (PlayerController.Instance.currentRightArm.Classification.ToString() != "Core")
                //{
                //    NameBox.text = PlayerController.Instance.currentRightArm.Name.ToString();
                //    //DescBox.text = ;
                //    break;
                //}
                //else
                //{
                //    NameBox.text = "Left Arm Name";
                //    DescBox.text = "Left Arm Desc";
                //}
        }

    }

    public void SetSprite(Name name, LimbType type, Limb limb)
    {
        ImgBox.gameObject.SetActive(true);
        var limbSprite = limb.limbSprite;

        if(name.ToString() == "Core")
        {
            if (type.ToString() == "Arm")
            {
                ImgBox.sprite = limbSprite;
            };
        }

        if(name.ToString() == "Wolf")
        {
            if (type.ToString() == "Arm")
            {
                ImgBox.sprite = limbSprite;
            }
        }

    }


}
