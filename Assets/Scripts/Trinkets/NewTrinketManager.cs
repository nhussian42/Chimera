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

        optionOne.image.sprite = trinketOne.Icon;
        optionOne.GetComponentInChildren<TextMeshProUGUI>().text = trinketOne.TrinketName;
        optionTwo.image.sprite = trinketTwo.Icon;
        optionTwo.GetComponentInChildren<TextMeshProUGUI>().text = trinketTwo.TrinketName;
        optionThree.image.sprite = trinketThree.Icon;
        optionThree.GetComponentInChildren<TextMeshProUGUI>().text = trinketThree.TrinketName;

        optionOne.GetComponent<Image>().sprite = buttonSprite;
        optionTwo.GetComponent<Image>().sprite = buttonSprite;
        optionThree.GetComponent<Image>().sprite = buttonSprite;
    }

    public void PickupOptionOne()
    {
        masterTrinketList.Pickup(trinketOne, amountPerPickup);
        gameObject.SetActive(false);
    }
    public void PickupOptionTwo()
    {
        masterTrinketList.Pickup(trinketTwo, amountPerPickup);
        gameObject.SetActive(false);
    }
    public void PickupOptionThree()
    {
        masterTrinketList.Pickup(trinketThree, amountPerPickup);
        gameObject.SetActive(false);
    }
}
