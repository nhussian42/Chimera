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


    [SerializeField] private TMP_Text limbName;
    [SerializeField] private TMP_Text limbDesc;


    public event Action<PlayerInvSlot> OnLimbClicked;

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            OnLimbClicked?.Invoke(this);
        }

        switch (pointerData.pointerCurrentRaycast.gameObject.name)
        {
            case "Arm_L":
                UnityEngine.Debug.Log("ARM_L");
                limbName.text = "Left Arm Name";
                limbDesc.text = "Left Arm Desc";
                break;
            case "Arm_R":
                UnityEngine.Debug.Log("ARM_R");
                limbName.text = "Right Arm Name";
                limbDesc.text = "Right Arm Desc";
                break;
            case "Head":
                UnityEngine.Debug.Log("Head");
                limbName.text = "Head Name";
                limbDesc.text = "Head Desc";
                break;
            case "Core":
                UnityEngine.Debug.Log("Core");
                limbName.text = "Core Name";
                limbDesc.text = "Core Desc";
                break;
            case "Legs":
                UnityEngine.Debug.Log("Legs");
                limbName.text = "Legs Name";
                limbDesc.text = "Legs Desc";
                break;
            case "Relic":
                UnityEngine.Debug.Log("Relic");
                limbName.text = "Relic Name";
                limbDesc.text = "Relic Desc";
                break;
        }
    }

}
