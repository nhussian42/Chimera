using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomTrigger : MonoBehaviour
{
    private bool _triggered;
    [SerializeField] private RoomSide _exitRoomSide;
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            FloorManager.lastExitRoomSide = _exitRoomSide;
            FloorManager.LeaveRoom?.Invoke();
        }
    }
}
