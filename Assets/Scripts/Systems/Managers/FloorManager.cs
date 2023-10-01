using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : Singleton<FloorManager>
{
    public static Action LeaveRoom;

    public static Action TransitionPlayer;

    private void OnEnable()
    {
        TransitionPlayer += MovePlayerToNextRoom;
    }

    private void OnDisable()
    {
        TransitionPlayer -= MovePlayerToNextRoom;
    }

    private static void MovePlayerToNextRoom()
    {
        print(PlayerController.Instance);
        PlayerController.Instance.gameObject.SetActive(false);
    }
}
