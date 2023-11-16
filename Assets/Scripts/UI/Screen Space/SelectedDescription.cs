using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedDescription : MonoBehaviour
{
    [SerializeField] private Image selectedImg;
    [SerializeField] private TMP_Text selectedName;
    [SerializeField] private TMP_Text selectedDesc;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.selectedImg.gameObject.SetActive(false);
        this.selectedName.text = "";
        this.selectedDesc.text = "";
    }

    public void SetSelected(Sprite sprite, string name, string desc)
    {
        this.selectedImg.gameObject.SetActive(true);
        this.selectedImg.sprite = sprite;
        this.selectedName.text = name;
        this.selectedDesc.text = desc;
    }
}
