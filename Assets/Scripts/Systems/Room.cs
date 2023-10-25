using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Transform> bottomLeftStartDoors;
    public List<Transform> bottomRightStartDoors;
    
    private int _numCreaturesAlive;

    public bool RoomLoaded { get; private set; }

    private void OnEnable()
    {
        RoomLoaded = true;
    }

    private void OnDisable()
    {
        RoomLoaded = false;
    }
}
