using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BodyShopMenu : MonoBehaviour
{

    [SerializeField] private List<Button> ItemButtonList;
    [SerializeField] private Button HealItemButton;
    [SerializeField] private Button ExitButton;

    [SerializeField] private List<float> ItemCostList;
    [SerializeField] private float HealCost;

    [SerializeField] private List<Image> ItemImageList;

    [SerializeField] private List<TextMeshProUGUI> ItemTextList;
    [SerializeField] private TextMeshProUGUI HealItemText;

    [SerializeField] private List<TextMeshProUGUI> descList;
    [SerializeField] private List<GameObject> descObjList;



    [SerializeField] private Button firstSelect;

    [SerializeField] private TextMeshProUGUI CurrentBones;

    [SerializeField] private GameObject self;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetupPrices();
        SetupSprites();
        SetupDescriptions();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
        if (!firstSelect.gameObject.activeSelf)
        {
            for(int i = 1; i < ItemButtonList.Count; i++)
            {
                EventSystem.current.SetSelectedGameObject(ItemButtonList[i].gameObject);
                if (ItemButtonList[i].gameObject.activeSelf)
                {
                    break;
                }                              
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBoneCount();
    }

    public void ExitMenu()
    {
        self.SetActive(false);
    }

    private void UpdateBoneCount()
    {
        CurrentBones.text = $"Bones: {PlayerController.Instance.totalBones.ToString("F0")}";
    }

    private void SetupPrices()
    {
        ItemCostList[0] = BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().limbCost;
        ItemCostList[1] = BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>().limbCost;
        //ItemCostList[2] = BodyShop.Instance.SpawnedHead.GetComponent<LimbDrop>().limbCost;
     
        for (int i = 0; i < (ItemTextList.Count); i++) 
        {
            ItemTextList[i].text = $"Cost: {ItemCostList[i].ToString()}";
        }

        HealItemText.text = $"Cost: {HealCost.ToString()}";
    }

    private void SetupSprites()
    {
        ItemImageList[0].sprite = BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().limbSprite;
        ItemImageList[1].sprite = BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>().limbSprite;
    }

    private void SetupDescriptions()
    {
        descList[0].text = null;
        descList[1].text = null;
        descList[2].text = null;
        descList[3].text = null;
    }

    public void PurchaseOption(int i)
    {
        if (ItemCostList[i] < PlayerController.Instance.totalBones)
        {
            PlayerController.Instance.totalBones -= ItemCostList[i];
            BodyShop.Instance.DestroyOption(i); 
            EventSystem.current.SetSelectedGameObject(null);
            if (ItemButtonList[i] != ItemButtonList[2])
            {
                if (ItemButtonList[i + 1].gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(ItemButtonList[i+1].gameObject);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(ExitButton.gameObject);
                }
            }
            else if (ItemButtonList[i] == ItemButtonList[2])
            {
                EventSystem.current.SetSelectedGameObject(HealItemButton.gameObject);
            }
            if(!HealItemButton.gameObject.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(ExitButton.gameObject);
            }

            DestroyButton(i);
        }
    }

    public void PurchaseHeal()
    {
        if(HealCost < PlayerController.Instance.totalBones)
        {
            PlayerController.Instance.totalBones -= HealCost;
            PlayerController.Instance.currentLeftArm.UpdateCurrentHealth(5);
            PlayerController.Instance.currentRightArm.UpdateCurrentHealth(5);
            PlayerController.Instance.UpdateCoreHealth(5);

            HealItemButton.gameObject.SetActive(false);
            ItemImageList[3].gameObject.SetActive(false);
            descObjList[3].gameObject.SetActive(false);
            BodyShop.Instance.DestroyOption(3);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(ExitButton.gameObject);
        }
    }

    private void DestroyButton(int i)
    {
        descObjList[i].gameObject.SetActive(false);
        ItemButtonList[i].gameObject.SetActive(false);
        ItemImageList[i].gameObject.SetActive(false);
    }

}
