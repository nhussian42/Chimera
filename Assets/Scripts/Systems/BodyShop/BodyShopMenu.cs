using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BodyShopMenu : MonoBehaviour
{

    public List<Button> ItemButtonList;
    public Button HealItem;
    public Button ExitButton;

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
        DestroyImmediate(gameObject, true);
        
    }

    private void UpdateBoneCount()
    {
        CurrentBones.text = $"Bones: {PlayerController.Instance.totalBones.ToString("F0")}";
    }

    public void SetupPrices()
    {
        for(int i = 0; i < (ItemTextList.Count); i++) 
        {
            ItemTextList[i].text = (Random.Range(0, 100).ToString());
        }
    }


}
