using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostRunSummaryController : MonoBehaviour
{
    // Private
    private PostRunSummaryManager pRSManager;

    // Exposed
    [SerializeField] MasterTrinketList masterTrinketList;

    // Exposed UI
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI roomsClearedText;

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
        // displays and formats rooms cleared text
        yield return null;
    }

    private void ReadPlayerTrinketInventory()
    {
        // uses the master trinket list to display player's trinket inventory
    }

    private IEnumerator StartLimbsTimelapse()
    {
        // gets lists from PostRunSummaryManager to display a snapshot of player's limb build after each room in sequential order
        yield return null;
        
    }

    //Retry function?

    //Main Menu function?
}
