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
        DebugControls.SpawnRandomCreature += SpawnRandomCreature;
    }

    protected virtual void OnDisable()
    {
        CreatureManager.AnyCreatureDied -= SubtractCreature;
        DebugControls.SpawnRandomCreature += SpawnRandomCreature;
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

    private void SpawnRandomCreature()
    {
        List<Creature> creatures = FloorManager.Instance.currentFloor.spawnableMajorCreatures;
        int index = UnityEngine.Random.Range(0, creatures.Count);
        Instantiate(creatures[index], Vector3.zero, Quaternion.identity);
        _numCreaturesAlive++;
    }
}
