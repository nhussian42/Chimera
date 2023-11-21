using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostRunSummaryController : MonoBehaviour
{
    // Private
    private PostRunSummaryManager pRSManager;
    private int roomsCount;
    private int timerCount;
    private int snapshotIndex;

    // Exposed
    [SerializeField] MasterTrinketList masterTrinketList;
    [SerializeField] List<Image> trinketSlots;

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
    }

    private void OnEnable()
    {
        // subscribe to C# events here
    }

    private void OnDisable()
    {
        // unsubscribe to C# events here
    }

    private void Initialize()
    {
        roomsCount = 0;
        timerCount = 0;
        snapshotIndex = 0;
    }

    private void Display()
    {
        // Uses coroutines to display information in order
    }

    private IEnumerator PlayTimerText()
    {
        // displays and formats timer text
        yield return null;
    }

    private IEnumerator PlayRoomsClearedText()
    {
        // displays and formats 'rooms cleared' text
        roomsClearedText.text = "Rooms: " + roomsCount.ToString();
        yield return new WaitForSeconds(0.25f);
        if (roomsCount < pRSManager.roomsCleared)
            roomsCount++;
            StartCoroutine(PlayRoomsClearedText());
    }

    private void ReadPlayerTrinketInventory()
    {
        // uses the master trinket list to display player's trinket inventory
    }

    private IEnumerator StartLimbsTimelapse()
    {
        // gets lists from PostRunSummaryManager to display a snapshot of player's limb build after each room in sequential order
        if (snapshotIndex <= pRSManager.headsInRun.Count)
            head.sprite = pRSManager.headsInRun[snapshotIndex];
        if (snapshotIndex <= pRSManager.leftArmsInRun.Count)
            leftArm.sprite = pRSManager.leftArmsInRun[snapshotIndex];
        if (snapshotIndex <= pRSManager.rightArmsInRun.Count)
            rightArm.sprite = pRSManager.rightArmsInRun[snapshotIndex];
        if (snapshotIndex <= pRSManager.leftArmsInRun.Count)
            legs.sprite = pRSManager.legsInRun[snapshotIndex];
        yield return new WaitForSeconds(0.25f);
        if (snapshotIndex < pRSManager.headsInRun.Count 
            || snapshotIndex < pRSManager.leftArmsInRun.Count 
            || snapshotIndex < pRSManager.rightArmsInRun.Count 
            || snapshotIndex < pRSManager.leftArmsInRun.Count)
        {
            snapshotIndex++;
            StartCoroutine(StartLimbsTimelapse());
        }
    }

    //Retry function?

    //Main Menu function?
}
