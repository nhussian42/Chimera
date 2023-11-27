using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BodyShopMenu : MonoBehaviour
{

    public List<Button> ItemButtonList;
    public List<float> ItemCostList;
    public Button HealItemButton;
    public float HealCost;
    public Button ExitButton;
    public GameObject self;


    public List<TextMeshProUGUI> ItemTextList;
    public TextMeshProUGUI HealItemText;
    public TextMeshProUGUI CurrentBones;

    
    // Start is called before the first frame update
    void Start()
    {
        SetupPrices();

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
