using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExitRoomSide
{
    TopRight,
    TopLeft
}

public class LeaveRoomTrigger : MonoBehaviour
{
    private bool _triggered;
    [SerializeField] private ExitRoomSide _exitRoomSide;
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            FloorManager.Instance.lastExitRoomSide = _exitRoomSide;
            FloorManager.LeaveRoom?.Invoke();
        }
    }
}
