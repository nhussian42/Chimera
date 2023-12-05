using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomTrigger : MonoBehaviour
{
    private bool _triggered;
    [field: SerializeField] public RoomSide ExitRoomSide { get; private set; }

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

    public void EnableRoomTrigger()
    {
        roomTrigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            FloorManager.lastExitRoomSide = ExitRoomSide;
            FloorManager.StoredNextRoom = _nextRoom;
            FloorManager.LeaveRoom?.Invoke();

            // debugging room creature / plaque mismatch
            // CombatRoom c = (CombatRoom)FloorManager.StoredNextRoom;
            // CombatRoom n = (CombatRoom)_nextRoom;
            // print(n.currentMajorCreature);
            // print(c.currentMajorCreature);
        }
    }
}
