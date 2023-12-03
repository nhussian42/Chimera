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
    public static Action AllCreaturesDefeated;

    // statics
    public static RoomSide lastExitRoomSide;
    public static Room StoredNextRoom;
    private static Creature previouslyGeneratedMajorCreature;
    private static int _currentRoomIndex;

    [field: Header("Floor Info")]
    [field: SerializeField] public FloorSO currentFloor { get; private set; }
    [SerializeField] private int combatRoomBuildIndex;
    [SerializeField] private TutorialRoom tutorialRoom;

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
        AllCreaturesDefeated += () => AudioManager.PlaySound2D(AudioEvents.Instance.OnRoomCleared);
        PostRunSummaryController.OnPressedMainMenu += LoadMainMenu;
    }

    private void OnDisable()
    {
        LoadNextRoom -= LoadNextRoomIndex;
        AllCreaturesDefeated -= GenerateNewCombatRooms;
        AllCreaturesDefeated -= () => AudioManager.PlaySound2D(AudioEvents.Instance.OnRoomCleared);
        PostRunSummaryController.OnPressedMainMenu -= LoadMainMenu;
    }

    private void Start()
    {
        _currentRoomIndex++;
        DetermineNextRoom();
    }

    private void LoadNextRoomIndex()
    {
        if (_currentRoomIndex <= currentFloor.numCombatRooms)
            ChimeraSceneManager.Instance.LoadScene(combatRoomBuildIndex);
        else
        {
            LoadMainMenu(); // load main menu after boss room
        }
    }

    // refactor later
    public void LoadMainMenu()
    {
        _currentRoomIndex = 0;
        StoredNextRoom = null;
        previouslyGeneratedMajorCreature = null;
        ChimeraSceneManager.Instance.LoadScene(0);
    }

    private void DetermineNextRoom()
    {
        // Tutorial Room
        if (_currentRoomIndex == 1)
        {
            // Spawn the tutorial room only on the first floor
            if (currentFloor.index == 1)
            {
                lastExitRoomSide = tutorialRoom.OppositeEnterSide;
                SpawnRoom(tutorialRoom);
            }
            else
            {
                _currentRoomIndex++;
                DetermineNextRoom();
            }

            GenerateNewCombatRooms();
        }
        // Boss room
        else if (_currentRoomIndex > currentFloor.numCombatRooms)
        {
            SpawnRoom(currentFloor.bossRoom);
        }
        // Any other combat room after the first
        else
        {
            if (StoredNextRoom)
                SpawnRoom(StoredNextRoom);
            else
                Debug.LogError("Stored next room has not been set to any room.");
        }
    }

    // Attempts several times to generate new combat rooms until all spawnable conditions are met
    private CombatRoom DetermineNextCombatRoom()
    {
        int attemptsMax = 25;
        int attempts = 0;
        CombatRoom combatRoom = new CombatRoom();

        do
        {
            if (++attempts > attemptsMax)
            {
                Debug.LogWarning("Hit max attempts to generate room");
                combatRoom = (CombatRoom)StoredNextRoom;
                break;
            }
            combatRoom = GenerateNewCombatRoom();
            combatRoom.DetermineCreatures(currentFloor);
        } 
        while (
            combatRoom == StoredNextRoom ||
            (lastExitRoomSide == RoomSide.Left && combatRoom.bottomRightStartDoors.Count == 0) ||
            (lastExitRoomSide == RoomSide.Right && combatRoom.bottomLeftStartDoors.Count == 0) ||
            combatRoom.currentMajorCreature == previouslyGeneratedMajorCreature
            );

        previouslyGeneratedMajorCreature = combatRoom.currentMajorCreature;
        
        return combatRoom;
    }

    private CombatRoom GenerateNewCombatRoom()
    {
        int totalRooms = currentFloor.spawnableCombatRooms.Count;
        int nextRoomIndex = UnityEngine.Random.Range(0, totalRooms);
        CombatRoom combatRoom = currentFloor.spawnableCombatRooms[nextRoomIndex];

        return combatRoom;
    }

    private void GenerateNewCombatRooms()
    {
        if (_currentRoom is BossRoom) return;

        for (int i = 0; i < _currentRoom.exitDoors.Count; i++)
        {
            if (_currentRoomIndex == currentFloor.numCombatRooms && _currentRoom is CombatRoom)
            {
                CombatRoom _currentCombatRoom = (CombatRoom)_currentRoom;
                _currentCombatRoom.SpawnPlaqueIcon(currentFloor.bossPlaque, i);
                continue;
            }

            CombatRoom newRoom = DetermineNextCombatRoom();
            _currentRoom.exitDoors[i].GetComponentInChildren<LeaveRoomTrigger>()._nextRoom = newRoom;
            _currentRoom.SpawnPlaqueIcon(newRoom.currentMajorCreature.CreatureInfo.plaqueIcon, i);

            // debugging room creature / plaque mismatch
            // CombatRoom c = (CombatRoom)_currentRoom.exitDoors[i].GetComponentInChildren<LeaveRoomTrigger>()._nextRoom;
            // print(c.currentMajorCreature.CreatureInfo.plaqueIcon.gameObject.name);
            // print(newRoom.currentMajorCreature.CreatureInfo.plaqueIcon.gameObject.name);
        }
    }

    private void SpawnRoom(Room room)
    {
        GameObject environmentParent = new GameObject("==== ENVIRONMENT ====");
        _currentRoom = Instantiate(room, environmentParent.transform);
        
        if (_currentRoom is CombatRoom)
        {
            CombatRoom _currentCombatRoom = (CombatRoom)_currentRoom;
            _currentCombatRoom.SpawnCreatures(environmentParent.transform);
        }
        else if (_currentRoom is BossRoom)
        {
            BossRoom _currentBossRoom = (BossRoom)_currentRoom;
            //_currentBossRoom.SpawnBoss(currentFloor, environmentParent.transform);
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
            if (_currentRoom.bottomLeftStartDoors.Count <= 0) { EntrancePositionNotFound(); return; }

            startTransformIndex = UnityEngine.Random.Range(0, _currentRoom.bottomLeftStartDoors.Count);
            startTransform = _currentRoom.bottomLeftStartDoors[startTransformIndex];

        }
        else if (lastExitRoomSide == RoomSide.Left)
        {
            if (_currentRoom.bottomRightStartDoors.Count <= 0) { EntrancePositionNotFound(); return; }

            startTransformIndex = UnityEngine.Random.Range(0, _currentRoom.bottomRightStartDoors.Count);
            startTransform = _currentRoom.bottomRightStartDoors[startTransformIndex];
        }
    }

    private void EntrancePositionNotFound()
    {
        Debug.LogWarning("Last exit room side not found. Start transform set to 0,0,0 ");
        startTransform = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity).transform;
    }
}
