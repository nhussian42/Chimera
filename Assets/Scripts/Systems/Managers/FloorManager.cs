using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : Singleton<FloorManager>
{
    public static Action LeaveRoom;
    public static Action LoadNextRoom;
    public static Action NextRoomLoaded;
    public static Action EnableFloor;
    public static RoomSide lastExitRoomSide;
    private static int _currentRoomIndex;

    [Header("Floor Info")]
    [SerializeField] private FloorSO currentFloor;  // unserialize after debug
    [SerializeField] private int combatRoomBuildIndex;

    [Header("DEBUG READ ONLY")]
    [SerializeField] private Room _currentRoom; // unserialize after debug

    private Transform startTransform;
    public Transform StartTransform
    {
        get { return startTransform; }
    }

    private void OnEnable()
    {
        LoadNextRoom += LoadNextRoomIndex;
    }

    private void Start()
    {
        DetermineNextRoom();
    }

    private void OnDisable()
    {
        LoadNextRoom -= LoadNextRoomIndex;
    }

    private void LoadNextRoomIndex()
    {
        ChimeraSceneManager.Instance.LoadScene(combatRoomBuildIndex);
    }

    // Write this function if we end up using different scenes for different rooms
    // private int DetermineNextRoomIndex()
    // {
    //     print(_currentRoomIndex);
    //     return _currentRoomIndex;
    // }

    private void DetermineNextRoom()
    {
        if (++_currentRoomIndex > currentFloor.numCombatRooms)
        {
            SpawnRoom(currentFloor.bossRoom);
        }
        else
        {
            SpawnRoom(DetermineNextCombatRoom());
        }
    }

    private Room DetermineNextCombatRoom()
    {        
        int totalRooms = currentFloor.spawnableRooms.Count;
        int nextRoomIndex = UnityEngine.Random.Range(0, totalRooms);
        Room combatRoom = currentFloor.spawnableRooms[nextRoomIndex];

        // Keep generating new rooms until one has an entrance to spawn from (inefficient)
        if (lastExitRoomSide == RoomSide.Left && combatRoom.bottomRightStartDoors.Count == 0)
        {
            combatRoom = DetermineNextCombatRoom();
        }
        else if (lastExitRoomSide == RoomSide.Right && combatRoom.bottomLeftStartDoors.Count == 0)
        {
            combatRoom = DetermineNextCombatRoom();
        }
        
        return combatRoom;
    }

    private void SpawnRoom(Room room)
    {
        GameObject environmentParent = Instantiate(new GameObject());
        environmentParent.name = "==== ENVIRONMENT ====";
        _currentRoom = Instantiate(room, environmentParent.transform);
        
        //StartCoroutine(WaitForRoomLoad(room, 10f));
        DetermineEntrancePosition();
        NextRoomLoaded?.Invoke();
    }

    private IEnumerator WaitForRoomLoad(Room room, float maxTimeToWait)
    {
        float timeWaited = 0;

        while (!room.RoomLoaded && timeWaited < maxTimeToWait)
        {
            timeWaited += Time.deltaTime;
            yield return null;
        }

        if (room.RoomLoaded)
        {
            DetermineEntrancePosition();
            NextRoomLoaded?.Invoke();
        }
        else
        {
            Debug.LogError("Max time waiting for next room exceeded.");
        }
    }

    private void DetermineEntrancePosition()
    {
        int startTransformIndex = 0;

        if (lastExitRoomSide == RoomSide.Right)
        {
            startTransformIndex = UnityEngine.Random.Range(0, _currentRoom.bottomLeftStartDoors.Count);
            startTransform = _currentRoom.bottomLeftStartDoors[startTransformIndex];

        }
        else if (lastExitRoomSide == RoomSide.Left)
        {
            startTransformIndex = UnityEngine.Random.Range(0, _currentRoom.bottomRightStartDoors.Count);
            startTransform = _currentRoom.bottomRightStartDoors[startTransformIndex];
        }
    }
}
