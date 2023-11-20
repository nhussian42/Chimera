using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Things that need to be tracked:
/*
 * Run time
 * Rooms cleared
 * Trinkets acquired
 * Limbs equipped
 */
public class PostRunSummaryManager : Singleton<PostRunSummaryManager>
{
    // Private
    private bool timerStarted = false;
    public float currentTime { get; private set; } = 0;
    public int roomsCleared { get; private set; } = 0;
    public List<Sprite> headsInRun { get; private set; }
    public List<Sprite> leftArmsInRun { get; private set; }
    public List<Sprite> rightArmsInRun { get; private set; }
    public List<Sprite> legsInRun { get; private set; }

    // Events
    public static Action OnTimerUpdate;

    private void OnEnable()
    {
        // subscribe to C# events here
        DebugControls.TestTimerStart += StartTimer; // Debug Timer
        DebugControls.TestTimerStop += StopTimer;

        FloorManager.AllCreaturesDefeated += AddClearedRoom; 
        FloorManager.LeaveRoom += StopTimer;
        FloorManager.LeaveRoom += ReadPlayerLimbs;
        FloorManager.NextRoomLoaded += StartTimer;
    }

    private void OnDisable()
    {
        // unsubscribe from C# events here
        DebugControls.TestTimerStart -= StartTimer; // Debug Timer
        DebugControls.TestTimerStop -= StopTimer;

        FloorManager.AllCreaturesDefeated -= AddClearedRoom;
        FloorManager.LeaveRoom -= StopTimer;
        FloorManager.LeaveRoom -= ReadPlayerLimbs;
        FloorManager.NextRoomLoaded -= StartTimer;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // start and stop timer here
        if (timerStarted == true)
        {
            currentTime += Time.deltaTime;
            OnTimerUpdate?.Invoke();
        }        
    }

    private void Initialize() // Called to reset the post run summary controller
    {
        currentTime = 0;
        roomsCleared = 0;
        timerStarted = false;
        headsInRun.Clear();
        leftArmsInRun.Clear();
        rightArmsInRun.Clear();
        legsInRun.Clear();
    }

    private void StartTimer() // Called to start timer at beginning of new run
    {
        timerStarted = true;
    }

    private void StopTimer() // Called to stop timer when run ends or when paused
    {
        timerStarted = false;
    }

    private void AddClearedRoom() // Called when a player clears a room
    {
        roomsCleared++;
    }

    private void ReadPlayerLimbs() // Called when player exits a room to record the equipped limbs at that time
    {
        headsInRun.Add(PlayerController.Instance.currentHead.LimbSprite);
        leftArmsInRun.Add(PlayerController.Instance.currentLeftArm.LimbSprite);
        rightArmsInRun.Add(PlayerController.Instance.currentRightArm.LimbSprite);
        legsInRun.Add(PlayerController.Instance.currentLegs.LimbSprite);
    }
}
