using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<int> validFloors;

    public List<Transform> bottomLeftStartDoors;
    public List<Transform> bottomRightStartDoors;
    
    private int _numCreaturesAlive;
}
