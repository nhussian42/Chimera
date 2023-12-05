using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BodyShopMenu : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private List<Button> ItemButtonList;
    [SerializeField] private Button HealItemButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private GameObject EnterExit;

    [SerializeField] private List<float> ItemCostList;
    [SerializeField] private float HealCost;

    [SerializeField] private List<Image> ItemImageList;
    [SerializeField] private List<TextMeshProUGUI> ItemTextList;

    [SerializeField] private TextMeshProUGUI HealItemText;
    [SerializeField] private float HealAmount;

    [SerializeField] private TextMeshProUGUI limbText;

    [SerializeField] private List<TextMeshProUGUI> descList;
    [SerializeField] private List<GameObject> descObjList;

    [SerializeField] private GameObject confirmMenu;

    [SerializeField] private Button firstSelect;

    [SerializeField] private GameObject self;

    private LimbDrop shopLimbDrop;
    private Limb shopLimb;

    private int playerChoice;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.Instance;
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

    public void ExitMenu()
    {
        self.SetActive(false);
        playerController.EnableAllDefaultControls();
        playerController.DisableAllUIControls();
    }
    private void SetupPrices()
    {
        ItemCostList[0] = BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().limbCost;
        ItemCostList[1] = BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>().limbCost;
        //ItemCostList[2] = BodyShop.Instance.SpawnedHead.GetComponent<LimbDrop>().limbCost;
     
        for (int i = 0; i < (ItemTextList.Count); i++) 
        {
            ItemTextList[i].text = $"{ItemCostList[i].ToString()}";
        }

        HealItemText.text = $"{HealCost.ToString()}";
    }

    private void SetupSprites()
    {
        ItemImageList[0].sprite = BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().limbSprite;
        ItemImageList[1].sprite = BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>().limbSprite;
    }

    private void SetupDescriptions()
    {
        SetArmDesc();
        SetLegDesc();
        //SetHeadDesc();
    }

    public void PurchaseOption(int i)
    {
        if(i == 4)
        {
            PurchaseHeal();
        }
        else if (ItemCostList[i] < CurrencyManager.Instance.currentBones)
        {
            CurrencyManager.Instance.RemoveBones((int)ItemCostList[i]) ;
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
            AudioManager.PlaySound2D(AudioEvents.Instance.OnPurchaseItems);
            DestroyButton(i);
        }
    }

    public void PurchaseHeal()
    {
        if(HealCost < PlayerController.Instance.totalBones)
        {
            PlayerController.Instance.totalBones -= HealCost;
            PlayerController.Instance.currentLeftArm.UpdateCurrentHealth(HealAmount);
            PlayerController.Instance.currentRightArm.UpdateCurrentHealth(HealAmount);
            PlayerController.Instance.UpdateCoreHealth(HealAmount);

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

    public void ConfirmPrompt(int i)
    {
        confirmMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ConfirmButton.gameObject);
        EnterMenu(false);
        playerChoice = i;
        switch (i)
        {
            case 0:
                { 
                    limbText.text = $"{BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().Name.ToString() + " " + BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().LimbType.ToString()}"+" ?";
                    break; 
                }
            case 1:
                {
                    limbText.text = $"{BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>().Name.ToString() + " " + BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>().LimbType.ToString()}"+" ?";
                    break; 
                }
            case 2:
                {
                    limbText.text = $"{BodyShop.Instance.SpawnedHead.GetComponent<LimbDrop>().Name.ToString() + " " + BodyShop.Instance.SpawnedHead.GetComponent<LimbDrop>().LimbType.ToString()}"+" ?";
                    break; 
                }
            case 3:
                {
                    limbText.text = $"Heal Grub";
                    break; 
                }
        }
        
    }

    public void ConfirmYes()
    {
        PurchaseOption(playerChoice);
        ReSelectMenu();
        EnterMenu(true);
        confirmMenu.SetActive(false);
    }

    public void ConfirmNo()
    {
        ReSelectMenu();
        confirmMenu.SetActive(false);
        EnterMenu(true);
    }

    public void ReSelectMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        for(int i = 0; i < 3; i++)
        {
            if (ItemButtonList[i].gameObject.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(ItemButtonList[i].gameObject);
                break;
            }    
            else if(HealItemButton.gameObject.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(HealItemButton.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(ExitButton.gameObject);
            }
        }
    }

    // private void Update()
    // {
    //     Debug.Log(EventSystem.current.currentSelectedGameObject);
    // }

    private void EnterMenu(bool tf)
    {
        EnterExit.gameObject.SetActive(tf);
    }

    private void SetArmDesc()
    {
        shopLimbDrop = BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>();
        foreach (Arm arm in playerController.allArms)
        {
            if(arm.Classification == shopLimbDrop.Classification && arm.Weight == shopLimbDrop.Weight)
            {
                shopLimb = arm;
                descList[0].text = $"{"HP:        " + arm.DefaultMaxHealth.ToString()}";
                descList[1].text = $"{"ATK:      " + arm.DefaultAttackDamage.ToString()}";
                descList[2].text = $"{"SPD:      " + arm.DefaultAttackSpeed.ToString("F1")}";

                break;
            }
        }    
    }

    private void SetLegDesc()
    {
        shopLimbDrop = BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>();
        foreach (Legs legs in playerController.allLegs)
        {
            if (legs.Classification == shopLimbDrop.Classification && legs.Weight == shopLimbDrop.Weight)
            {
                shopLimb = legs;
                descList[3].text = $"{"HP:          " + legs.DefaultMaxHealth.ToString()}";
                descList[4].text = $"{"SPD:       " + legs.DefaultMovementSpeed.ToString()}";
                descList[5].text = $"{"CD:          " + legs.DefaultCooldownTime.ToString()}";

                break;
            }
        }
    }

    private void SetHeadDesc()
    {
        shopLimbDrop = BodyShop.Instance.SpawnedHead.GetComponent<LimbDrop>();
        foreach (Head head in playerController.allHeads)
        {
            if (head.Classification == shopLimbDrop.Classification && head.Weight == shopLimbDrop.Weight)
            {
                shopLimb = head;
                descList[6].text = $"{"HP:        " + head.DefaultMaxHealth.ToString()}";
                //descList[1].text = $"{"ATK: " + head..ToString()}";
                //descList[2].text = $"{"SPD: " + legs.DefaultCooldownTime.ToString()}";

                break;
            }
        }
    }

}
