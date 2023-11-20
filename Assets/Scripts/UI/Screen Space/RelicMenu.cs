using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelicMenu : MonoBehaviour
{
    [SerializeField] private Button clickYes;
    [SerializeField] private Button clickNo;
    [SerializeField] private Button clickCancel;

    [SerializeField] private Image relicImage;
    [SerializeField] private TextMeshProUGUI descText;

    [SerializeField] private MasterTrinketList masterTrinketList;
    [SerializeField] private Trinket relic;

    private GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(clickYes.gameObject);

        self = this.gameObject;
    }

    public void Yes()
    {
        masterTrinketList.AddRelic(relic);
        self.SetActive(false);
        RelicInteract.instance.pickedYes = true;
    }

    public void No()
    {
        self.SetActive(false);
    }
}
