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
    [SerializeField] private TextMeshProUGUI limbText;


    [SerializeField] private List<TextMeshProUGUI> descList;
    [SerializeField] private List<GameObject> descObjList;
    [SerializeField] private GameObject confirmMenu;

    [SerializeField] private Button firstSelect;

    [SerializeField] private GameObject self;

    private int playerChoice;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupPrices();
        SetupSprites();
        SetupDescriptions();
        playerController = PlayerController.Instance;
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
            ItemTextList[i].text = $"Buy: {ItemCostList[i].ToString()}";
        }

        HealItemText.text = $"Buy: {HealCost.ToString()}";
    }

    private void SetupSprites()
    {
        ItemImageList[0].sprite = BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().limbSprite;
        ItemImageList[1].sprite = BodyShop.Instance.SpawnedLeg.GetComponent<LimbDrop>().limbSprite;
    }

    private void SetupDescriptions()
    {
        descList[0].text = $"{BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().Name.ToString() + " " + BodyShop.Instance.SpawnedArm.GetComponent<LimbDrop>().LimbType}";
        descList[1].text = null;
        descList[2].text = null;
        descList[3].text = null;

        
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

    private void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    private void EnterMenu(bool tf)
    {
        EnterExit.gameObject.SetActive(tf);
    }
}
