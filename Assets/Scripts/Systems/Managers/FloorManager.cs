using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : Singleton<FloorManager>
{
    public static Action LeaveRoom;
    public static Action<Room> LeaveRoomRoom;
    public static Action LoadNextRoom;
    public static Action NextRoomLoaded;
    public static Action EnableFloor;
    public static Action AllCreaturesDefeated;
    public static RoomSide lastExitRoomSide;
    public static Room StoredNextRoom;
    private static int _currentRoomIndex;

    [field: Header("Floor Info")]
    [field: SerializeField] public FloorSO currentFloor { get; private set; }
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
        AllCreaturesDefeated += GenerateNewCombatRooms;
    }

    private void Start()
    {
        if (_currentRoomIndex == 0 || _currentRoomIndex > currentFloor.numCombatRooms)
            DetermineNextRoom();
        else
            SpawnRoom(StoredNextRoom);
    }

    private void OnDisable()
    {
        LoadNextRoom -= LoadNextRoomIndex;
        AllCreaturesDefeated -= GenerateNewCombatRooms;
    }

    private void LoadNextRoomIndex()
    {
        if (_currentRoomIndex + 2 < currentFloor.numCombatRooms)
            ChimeraSceneManager.Instance.LoadScene(combatRoomBuildIndex);
        else
            ChimeraSceneManager.Instance.LoadScene(0); // Loads main menu after boss room
    }

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

    private CombatRoom DetermineNextCombatRoom()
    {        
        int totalRooms = currentFloor.spawnableCombatRooms.Count;
        int nextRoomIndex = UnityEngine.Random.Range(0, totalRooms);
        CombatRoom combatRoom = currentFloor.spawnableCombatRooms[nextRoomIndex];

        // Keep generating new rooms until one has an entrance to spawn from (inefficient)
        if (lastExitRoomSide == RoomSide.Left && combatRoom.bottomRightStartDoors.Count == 0)
        {
            combatRoom = DetermineNextCombatRoom();
        }
        else if (lastExitRoomSide == RoomSide.Right && combatRoom.bottomLeftStartDoors.Count == 0)
        {
            combatRoom = DetermineNextCombatRoom();
        }

        combatRoom.DetermineCreatures(currentFloor);
        
        return combatRoom;
    }

    private void SpawnRoom(Room room)
    {
        GameObject environmentParent = Instantiate(new GameObject());
        environmentParent.gameObject.name = "==== ENVIRONMENT ====";
        _currentRoom = Instantiate(room, environmentParent.transform);
        
        if (_currentRoom is CombatRoom)
        {
            CombatRoom _currentCombatRoom = (CombatRoom)_currentRoom;
            _currentCombatRoom.SpawnCreatures(environmentParent.transform);
        }
        
        //StartCoroutine(WaitForRoomLoad(room, 10f));
        DetermineEntrancePosition();
        NextRoomLoaded?.Invoke();
    }

    // Apparently useless because instantiation is run through the main thread
    // private IEnumerator WaitForRoomLoad(CombatRoom room, float maxTimeToWait)
    // {
    //     float timeWaited = 0;

    //     while (!room.RoomLoaded && timeWaited < maxTimeToWait)
    //     {
    //         timeWaited += Time.deltaTime;
    //         yield return null;
    //     }

    //     if (room.RoomLoaded)
    //     {
    //         DetermineEntrancePosition();
    //         NextRoomLoaded?.Invoke();
    //     }
    //     else
    //     {
    //         Debug.LogError("Max time waiting for next room exceeded.");
    //     }
    // }

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

    private void GenerateNewCombatRooms()
    {
        for (int i = 0; i < _currentRoom.exitDoors.Count; i++)
        {
            CombatRoom newRoom = DetermineNextCombatRoom();
            _currentRoom.exitDoors[i].GetComponentInChildren<LeaveRoomTrigger>()._nextRoom = newRoom;

            if (_currentRoom is CombatRoom)
            {
                CombatRoom _currentCombatRoom = (CombatRoom)_currentRoom;
                Creature currentMajorCreature = newRoom.currentMajorCreature;
                _currentCombatRoom.SpawnPlaqueIcon(currentMajorCreature.CreatureInfo.plaqueIcon);
            }
            else
            {
                Debug.LogWarning("CombatRoom expected, another room type detected.");
            }
        }
    }
}
