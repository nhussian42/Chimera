using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomTrigger : MonoBehaviour
{
    private bool _triggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            FloorManager.LeaveRoom?.Invoke();
        }
    }
}
