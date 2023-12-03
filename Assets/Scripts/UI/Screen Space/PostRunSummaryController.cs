using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PostRunSummaryController : MonoBehaviour
{
    // Private
    private PostRunSummaryManager pRSManager;
    private int roomsCount;
    private int timerCount;
    private int secondsCount;
    private int minutesCount;
    private int hoursCount;
    private int snapshotIndex;

    // Exposed
    [SerializeField] MasterTrinketList masterTrinketList;
    [SerializeField] GameObject trinketInventoryGrid;
    [SerializeField] TrinketCellSlot trinketCellPrefab;
    [SerializeField] Button mainMenuButton;

    // Events
    public static Action OnPressedMainMenu;
    public static Action OnPressedRetry;

    // Exposed UI
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI roomsClearedText;
    [SerializeField] Image head;
    [SerializeField] Image leftArm;
    [SerializeField] Image rightArm;
    [SerializeField] Image legs;

    // Start is called before the first frame update
    void Start()
    {
        pRSManager = PostRunSummaryManager.Instance;
        PlayerController.OnDie += Display;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerController.OnDie -= Display;
    }

    private void Initialize()
    {
        roomsCount = 0;
        timerCount = 0;
        secondsCount = 0;
        minutesCount = 0;
        hoursCount = 0;
        snapshotIndex = 0;
    }

    private void Display()
    {
        // Uses coroutines to display information in order
        gameObject.SetActive(true);
        PlayerController.Instance.DisableAllDefaultControls();
        PlayerController.Instance.EnableAllUIControls();
        EventSystem.current.SetSelectedGameObject(mainMenuButton.gameObject);
        Initialize();
        ReadPlayerTrinketInventory();
        StartCoroutine(PlayTimerText());
        StartCoroutine(PlayRoomsClearedText());
        StartCoroutine(PlayLimbsTimelapse());
    }

    private IEnumerator PlayTimerText()
    {
        // displays and formats timer text
        string hoursText = hoursCount.ToString();
        string minutesText = minutesCount.ToString();
        string secondsText = secondsCount.ToString();

        if (hoursText.Length < 2)
            hoursText = "0" + hoursCount.ToString();
        if (minutesText.Length < 2)
            minutesText = "0" + minutesCount.ToString();
        if (secondsText.Length < 2)
            secondsText = "0" + secondsCount.ToString();

        timerText.text = hoursText + ":" + minutesText + ":" + secondsText;

        yield return new WaitForSeconds(0.01f);
        if (timerCount < pRSManager.currentTime)
        {
            timerCount++;
            secondsCount++;
            if (secondsCount >= 60)
            {
                secondsCount = 0;
                minutesCount++;
            }
            if (minutesCount >= 60)
            {
                minutesCount = 0;
                hoursCount++;
            }              

            StartCoroutine(PlayTimerText());
        }
    }

    private IEnumerator PlayRoomsClearedText()
    {
        // displays and formats 'rooms cleared' text
        roomsClearedText.text = roomsCount.ToString();
        yield return new WaitForSeconds(0.25f);
        if (roomsCount < pRSManager.roomsCleared)
            roomsCount++;
            StartCoroutine(PlayRoomsClearedText());
    }

    private void ReadPlayerTrinketInventory()
    {
        // uses the master trinket list to display player's trinket inventory
        foreach(Trinket trinket in masterTrinketList.playerInventory)
        {
            TrinketCellSlot cell = Instantiate(trinketCellPrefab.gameObject, trinketInventoryGrid.transform).GetComponent<TrinketCellSlot>();
            cell.SetSprite(trinket.Icon);
            cell.WriteQuantityText(trinket.Amount.ToString());
        }
    }

    private IEnumerator PlayLimbsTimelapse()
    {
        // gets lists from PostRunSummaryManager to display a snapshot of player's limb build after each room in sequential order
        if (snapshotIndex <= pRSManager.headsInRun.Count)
        head.sprite = pRSManager.headsInRun[snapshotIndex];
        else
            head.sprite = pRSManager.headsInRun[pRSManager.headsInRun.Count];
        if (snapshotIndex <= pRSManager.leftArmsInRun.Count)
            leftArm.sprite = pRSManager.leftArmsInRun[snapshotIndex];
        else
            leftArm.sprite = pRSManager.leftArmsInRun[pRSManager.leftArmsInRun.Count];
        if (snapshotIndex <= pRSManager.rightArmsInRun.Count)
            rightArm.sprite = pRSManager.rightArmsInRun[snapshotIndex];
        else
            rightArm.sprite = pRSManager.rightArmsInRun[pRSManager.rightArmsInRun.Count];
        if (snapshotIndex <= pRSManager.leftArmsInRun.Count)
            legs.sprite = pRSManager.legsInRun[snapshotIndex];
        else
            legs.sprite = pRSManager.legsInRun[pRSManager.leftArmsInRun.Count];
        yield return new WaitForSeconds(0.5f);
        if (snapshotIndex < pRSManager.headsInRun.Count 
            || snapshotIndex < pRSManager.leftArmsInRun.Count 
            || snapshotIndex < pRSManager.rightArmsInRun.Count 
            || snapshotIndex < pRSManager.leftArmsInRun.Count)
        {
            snapshotIndex++;
            StartCoroutine(PlayLimbsTimelapse());
        }
    }

    public void PressedMainMenu()
    {
        OnPressedMainMenu?.Invoke();
    }

    //public void PressedRetry()
    //{
    //    MainMenu.LoadFirstRoom?.Invoke();
    //}
}
