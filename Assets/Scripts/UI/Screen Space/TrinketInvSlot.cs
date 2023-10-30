using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class TrinketInvSlot : MonoBehaviour
{
    [SerializeField] private Image trinketImg;
    [SerializeField] private TMP_Text qtyTxt;
    [SerializeField] private Image borderImage;

    public event Action<TrinketInvSlot> OnTrinketClicked;

    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.trinketImg.gameObject.SetActive(false);
        empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this.trinketImg.gameObject.SetActive(true);
        this.trinketImg.sprite = sprite;
        this.qtyTxt.text = quantity + "";
        empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            OnTrinketClicked?.Invoke(this);
        }

        if (empty)
        {
            UnityEngine.Debug.Log("Empty");
        }
    }
}
