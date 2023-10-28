using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Transform> bottomLeftStartDoors;
    public List<Transform> bottomRightStartDoors;
    public List<Transform> exitDoors;

    protected int _numCreaturesAlive;

    protected virtual void OnEnable()
    {
        CreatureManager.AnyCreatureDied += SubtractCreature;
    }

    protected virtual void OnDisable()
    {
        CreatureManager.AnyCreatureDied -= SubtractCreature;
    }

    private void SubtractCreature()
    {
        if (--_numCreaturesAlive <= 0)
        {
            print("All creatures defeated in room!");
            FloorManager.AllCreaturesDefeated?.Invoke();
            // spawn limb at currentmajorcreature transform?????
        }
    } 
}
