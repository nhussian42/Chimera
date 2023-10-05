using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorManager : Singleton<FloorManager>
{
    public static Action LeaveRoom;
    public static Action LoadNextRoom;
    public static Action NextRoomLoaded;
    public static Action EnableFloor;

    [Header("Floor")]
    [SerializeField] private int _currentFloor;  // unserialize after debug
    [SerializeField] private Room _currentRoom; // unserialize after debug
    [SerializeField] private int _totalRooms;
    [SerializeField] private int _currentRoomIndex; // unserialize after debug
    
    public static ExitRoomSide lastExitRoomSide;

    private Transform startTransform;
    public Transform StartTransform
    {
        get { return startTransform; }
    }

    protected override void Init()
    {
        _currentRoomIndex = 1;
    }

    private void OnEnable()
    {
        LoadNextRoom += SetupNextRoom;
        NextRoomLoaded += DetermineEntrancePosition;
        //EnableFloor += EnablePlayerControls;
        
        NextRoomLoaded?.Invoke();
    }

    private void OnDisable()
    {
        LoadNextRoom -= SetupNextRoom;
    }

    private void SetupNextRoom()
    {
        PlayerController.Instance.gameObject.SetActive(false);

        UnitySceneManager.Instance.LoadScene(DetermineNextRoomIndex());
    }

    private int DetermineNextRoomIndex()
    {
        return _currentRoomIndex;
    }

    private void DetermineEntrancePosition()
    {
        int startTransformIndex = 0;

        if (lastExitRoomSide == ExitRoomSide.TopRight)
        {
            startTransformIndex = UnityEngine.Random.Range(0, _currentRoom.bottomLeftStartDoors.Count);
            startTransform = _currentRoom.bottomLeftStartDoors[startTransformIndex];

        }
        else if (lastExitRoomSide == ExitRoomSide.TopLeft)
        {
            startTransformIndex = UnityEngine.Random.Range(0, _currentRoom.bottomRightStartDoors.Count);
            startTransform = _currentRoom.bottomRightStartDoors[startTransformIndex];
        }
    }
}
