using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EMScript : MonoBehaviour
{
    [SerializeField] private TrinketInvSlot trinketSlot;

    [SerializeField] private RectTransform contentPanel;

    [SerializeField] private PlayerInvSlot playerInv;

    [SerializeField] private List<Trinket> trinketList;
    [SerializeField] private MasterTrinketList masterTrinketList;

    [SerializeField] public Transform trinketCont;
    [SerializeField] public GameObject trinketInInv;

    [SerializeField] private Button selectedButton;
    [SerializeField] public Image selectedImg;
    [SerializeField] private TMP_Text selectedName;
    [SerializeField] private TMP_Text selectedDescription;


    List<TrinketInvSlot> listOfTrinkets = new List<TrinketInvSlot>();

    public static EMScript Instance;

    private void Awake()
    {      
        Instance = this;
        Hide();
        ResetDescription();

        foreach(Trinket trinket in masterTrinketList.playerInventory)
        {
            trinketList.Add(trinket);
        }

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void AddTrinket(Trinket trinket)
    {
        if (trinketList.Contains(trinket) && (trinket.IsRelic == false))
        {
            return;
        }
        else
        {
            trinketList.Add(trinket);
        }
    }

    public void ListTrinkets()
    {
        foreach(Transform trinketInInv in trinketCont)
        {
            Destroy(trinketInInv.gameObject);
        }

        foreach (var Trinket in trinketList)
        {
            GameObject obj = Instantiate(trinketInInv, trinketCont);
            var trinketName = obj.transform.Find("TrinketName").GetComponent<TextMeshProUGUI>();
            var trinketImage = obj.transform.Find("TrinketIcon").GetComponent<Image>();
            var trinketDesc = obj.transform.Find("TrinketDescription").GetComponent<TextMeshProUGUI>();
            var trinketQuantity = obj.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

            trinketName.text = Trinket.name;
            trinketImage.sprite = Trinket.Icon;
            trinketDesc.text = Trinket.Description;
            trinketQuantity.text = Trinket.amount.ToString();

            if (Trinket.TrinketType.ToString() == "OneTime")
            {
                trinketQuantity.text = "U";
            }


        }

        playerInv.SetLimbSprites();

        
    }

    public void ChangeDescriptionTrinket(Button button)
    {
        selectedImg.gameObject.SetActive(true);

        selectedName.text = button.transform.Find("TrinketName").GetComponent<TMP_Text>().text;
        selectedImg.sprite = button.transform.Find("TrinketIcon").GetComponent<Image>().sprite;
        selectedDescription.text = button.transform.Find("TrinketDescription").GetComponent<TMP_Text>().text;
        
        //UnityEngine.Debug.Log(selectedImg.sprite.ToString()); 
        //UnityEngine.Debug.Log(selectedName.text);
        //UnityEngine.Debug.Log(selectedDescription.text);  
        SelectedDescription.instance.SetSelected(selectedImg.sprite, selectedName.text, selectedDescription.text);
        
  
    }

    public void ResetDescription()
    {
        selectedImg.gameObject.SetActive(false);
        selectedName.text = "";
        selectedDescription.text = "";
    }

}
