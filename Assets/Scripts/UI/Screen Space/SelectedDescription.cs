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

    public static SelectedDescription instance;

    public void Awake()
    { 
        instance = this;
    }


    public void SetSelected(Sprite sprite, string name, string desc)
    {
        selectedImg.gameObject.SetActive(true);
        selectedImg.sprite = sprite;
        selectedName.text = name;
        selectedDesc.text = desc;
        
    }
}
