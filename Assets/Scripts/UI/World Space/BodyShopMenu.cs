using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BodyShopMenu : MonoBehaviour
{

    [SerializeField] private List<Button> ItemButtonList;
    [SerializeField] private List<float> ItemCostList;
    [SerializeField] private List<Image> ItemImageList;
    [SerializeField] private List<TextMeshProUGUI> ItemTextList;
    [SerializeField] private List<TextMeshProUGUI> descList;

    [SerializeField] private Button HealItemButton;
    [SerializeField] private TextMeshProUGUI HealItemText;
    [SerializeField] private float HealCost;

    [SerializeField] private Button ExitButton;
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

    }

    public void PurchaseOption(int i)
    {
        Debug.Log("bought?!?!");
        if (ItemCostList[i] < PlayerController.Instance.totalBones)
        {
            Debug.Log("bought");
            PlayerController.Instance.totalBones -= ItemCostList[i];
            BodyShop.Instance.DestroyOption(i);
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
            BodyShop.Instance.DestroyOption(3);
        }
    }

    private void DestroyButton(int i)
    {
        ItemButtonList[i].gameObject.SetActive(false);

    }

}
