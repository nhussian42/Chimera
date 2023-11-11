using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class EMScript : MonoBehaviour
{
    [SerializeField] private TrinketInvSlot trinketSlot;

    [SerializeField] private RectTransform contentPanel;

    [SerializeField] private SelectedDescription selectedDesc;

    [SerializeField] private PlayerInvSlot playerInv;


    List<TrinketInvSlot> listOfTrinkets = new List<TrinketInvSlot>();
 
    private void Awake()
    {
        Hide();
        selectedDesc.ResetDescription();
    }


    public void InitializeTrinketInv (int invSize)
    {
        for (int i = 0; i < invSize; i++)
        {
            TrinketInvSlot trinkety = Instantiate(trinketSlot, Vector3.zero, Quaternion.identity) as TrinketInvSlot;
            trinkety.transform.SetParent(contentPanel, false);
            listOfTrinkets.Add(trinkety);

            trinketSlot.OnTrinketClicked += HandleTrinketSelection;
        }
    }

    private void HandleTrinketSelection(TrinketInvSlot obj)
    {
        UnityEngine.Debug.Log(obj.name);
        listOfTrinkets[0].Select();
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
