using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderHighlight : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private GameObject highlight;
    [SerializeField] private Color baseColor;
    public void OnSelected(BaseEventData data)
    {
        Debug.Log("selected");
        var highlightColor = highlight.GetComponent<Image>().color;
        highlightColor = Color.white;
        highlight.GetComponent<Image>().color = highlightColor;
    }

    public void OnDeselect(BaseEventData data)
    {
        Debug.Log("Deselected");
        var highlightColor = highlight.GetComponent<Image>().color;
        highlightColor = baseColor;
        highlight.GetComponent<Image>().color = highlightColor;
    }

    
}

