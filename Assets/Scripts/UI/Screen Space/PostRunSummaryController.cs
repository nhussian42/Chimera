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
public class PostRunSummaryController : Singleton<PostRunSummaryController>
{
    // Private
    private bool timerStarted = false;
    public float currentTime { get; private set; } = 0;
    public int roomsCleared { get; private set; } = 0;

    // Events
    public static Action OnTimerUpdate;

    private void OnEnable()
    {
        // subscribe to C# events here
        DebugControls.TestTimerStart += StartTimer; // Debug Timer
        DebugControls.TestTimerStop += StopTimer;
        FloorManager.AllCreaturesDefeated += AddClearedRoom;
        FloorManager.LeaveRoom += StopTimer;
        FloorManager.NextRoomLoaded += StartTimer;
    }

    private void OnDisable()
    {
        // unsubscribe from C# events here
        DebugControls.TestTimerStart -= StartTimer; // Debug Timer
        DebugControls.TestTimerStop -= StopTimer;
        FloorManager.AllCreaturesDefeated -= AddClearedRoom;
        FloorManager.LeaveRoom -= StopTimer;
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
            currentTime += Time.deltaTime * 20;
            OnTimerUpdate?.Invoke();
        }        
    }

    private void Initialize() // Called to reset the post run summary controller
    {
        currentTime = 0;
        roomsCleared = 0;
        timerStarted = false;
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

    private void ReadPlayerLimbs() // Called at end of run to read and display the player's equipped limbs
    {

    }

    private void ReadPlayerTrinkets() // Called at end of run to read and display the player's trinket inventory
    {

    }

}
