using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenTrigger : MonoBehaviour
{
    private bool _triggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            transform.parent.Find("ExitDoor").GetComponent<MoveDoor>().TriggerDoorOpen();
            transform.parent.GetComponent<LeaveRoomTrigger>().EnableRoomTrigger();
        }
    }
}
