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
                if (PlayerController.Instance.currentLeftArm.Classification.ToString() != "Core")
                {
                    NameBox.text = PlayerController.Instance.currentLeftArm.Classification.ToString();
                    //DescBox.text = ;
                    break;
                }
                else
                {
                    NameBox.text = "Left Arm Name";
                    DescBox.text = "Left Arm Desc";
                    break;
                }
                
            case "Arm_R":
                UnityEngine.Debug.Log("ARM_R");
                if (PlayerController.Instance.currentRightArm.Classification.ToString() != "Core")
                {
                    NameBox.text = PlayerController.Instance.currentRightArm.Classification.ToString();
                    //DescBox.text = ;
                    break;
                }
                else
                {
                    NameBox.text = "Left Arm Name";
                    DescBox.text = "Left Arm Desc";
                }
                break;
            case "Head":
                UnityEngine.Debug.Log("Head");
                NameBox.text = "Head Name";
                DescBox.text = "Head Desc";
                break;
            case "Core":
                UnityEngine.Debug.Log("Core");
                NameBox.text = "Core Name";
                DescBox.text = "Core Desc";
                break;
            case "Legs":
                UnityEngine.Debug.Log("Legs");
                NameBox.text = "Legs Name";
                DescBox.text = "Legs Desc";
                break;
            case "Relic":
                UnityEngine.Debug.Log("Relic");
                NameBox.text = "Relic Name";
                DescBox.text = "Relic Desc";
                break;
        }
    }

}
