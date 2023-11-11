using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewTrinketManager : Singleton<NewTrinketManager>
{
    [SerializeField] private MasterTrinketList masterTrinketList;

    [SerializeField] private int amountPerPickup;

    [SerializeField] private GameObject OptionOneGO;
    [SerializeField] public Sprite buttonSprite;

    [SerializeField] private Button optionOne;
    [SerializeField] private Button optionTwo;
    [SerializeField] private Button optionThree;

    [SerializeField] public List<TextMeshProUGUI> trinketDesc;
    [SerializeField] public List<Image> trinketSprite;

    [HideInInspector] public Trinket trinketOne { get; private set; }
    [HideInInspector] public Trinket trinketTwo { get; private set; }
    [HideInInspector] public Trinket trinketThree { get; private set; }

    static bool called = false;

    protected override void Init()
    {
        if(called == false)
        {
            called = true;
            masterTrinketList.FullReset();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(OptionOneGO);

        

        trinketOne = masterTrinketList.GetRandomTrinket();
        trinketTwo = masterTrinketList.GetRandomTrinket();
        trinketThree = masterTrinketList.GetRandomTrinket();
  
        optionOne.GetComponentInChildren<TextMeshProUGUI>().text = trinketOne.TrinketName;  
        trinketDesc[0].text = trinketOne.Description.ToString();
        trinketSprite[0].sprite = trinketOne.Icon;
        

        optionTwo.GetComponentInChildren<TextMeshProUGUI>().text = trinketTwo.TrinketName;
        trinketDesc[1].text = trinketTwo.Description.ToString();
        trinketSprite[1].sprite = trinketTwo.Icon;
        

        optionThree.GetComponentInChildren<TextMeshProUGUI>().text = trinketThree.TrinketName;
        trinketDesc[2].text = trinketThree.Description.ToString();
        trinketSprite[2].sprite = trinketThree.Icon;
        

        optionOne.GetComponent<Image>().sprite = buttonSprite;
        optionTwo.GetComponent<Image>().sprite = buttonSprite;
        optionThree.GetComponent<Image>().sprite = buttonSprite;
    }

    public void PickupOptionOne()
    {
        masterTrinketList.Pickup(trinketOne, amountPerPickup);
        Debug.Log(trinketOne.TrinketName);
        gameObject.SetActive(false);
    }
    public void PickupOptionTwo()
    {
        masterTrinketList.Pickup(trinketTwo, amountPerPickup);
        Debug.Log(trinketOne.TrinketName);
        gameObject.SetActive(false);
    }
    public void PickupOptionThree()
    {
        masterTrinketList.Pickup(trinketThree, amountPerPickup);
        Debug.Log(trinketThree.TrinketName);
        gameObject.SetActive(false);
    }
}
