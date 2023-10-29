using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EMScript : MonoBehaviour
{
    [SerializeField] private TrinketInvSlot trinketSlot;

    [SerializeField] private RectTransform contentPanel;

    List<TrinketInvSlot> listOfTrinkets = new List<TrinketInvSlot>();
 
    public void InitializeTrinketInv (int invSize)
    {
        for (int i = 0; i < invSize; i++)
        {
            TrinketInvSlot trinkety = Instantiate(trinketSlot, Vector3.zero, Quaternion.identity) as TrinketInvSlot;
            trinketSlot.transform.SetParent(contentPanel);
            listOfTrinkets.Add(trinkety);
        }
    }
}
