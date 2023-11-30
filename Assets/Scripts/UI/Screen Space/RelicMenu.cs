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

    private PlayerController playerController;

    [SerializeField] private GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(clickYes.gameObject);
        playerController = PlayerController.Instance;
    }

    public void Yes()
    {
        masterTrinketList.AddRelic(relic);
        
        RelicInteract.instance.pickedYes = true;
        playerController.EnableAllDefaultControls();
        playerController.DisableAllUIControls();
        self.SetActive(false);
    }

    public void No()
    {
        self.SetActive(false);
        playerController.EnableAllDefaultControls();
        playerController.DisableAllUIControls();
    }
}
