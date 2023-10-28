using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomTrigger : MonoBehaviour
{
    private bool _triggered;
    [field: SerializeField] public RoomSide _exitRoomSide { get; private set; }

    [HideInInspector] public Room _nextRoom;

    private Collider roomTrigger;

    private void Awake()
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
