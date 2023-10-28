using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomTrigger : MonoBehaviour
{
    private bool _triggered;
    [SerializeField] private RoomSide _exitRoomSide;

    [HideInInspector] public Room _nextRoom;

    private Collider roomTrigger;

    private void OnAwake()
    {
        roomTrigger = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
       FloorManager.AllCreaturesDefeated += EnableRoomTrigger;
    }

    private void OnDisable()
    {
        FloorManager.AllCreaturesDefeated -= EnableRoomTrigger;
    }

    private void EnableRoomTrigger()
    {
        roomTrigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            FloorManager.lastExitRoomSide = _exitRoomSide;
            FloorManager.StoredNextRoom = _nextRoom;
            FloorManager.LeaveRoom?.Invoke();
        }
    }
}
