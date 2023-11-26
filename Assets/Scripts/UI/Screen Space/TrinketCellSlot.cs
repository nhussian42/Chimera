using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrinketCellSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image trinketSprite;

    public void SetSprite(Sprite sprite)
    {
        trinketSprite.sprite = sprite;
    }

    public void WriteQuantityText(string text)
    {
        quantityText.text = text;
    }
}
