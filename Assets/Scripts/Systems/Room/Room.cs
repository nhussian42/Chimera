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
        CreatureManager.AnyCreatureDied += DefeatCreature;
        DebugControls.SpawnRandomCreature += SpawnRandomCreature;
    }

    protected virtual void OnDisable()
    {
        CreatureManager.AnyCreatureDied -= DefeatCreature;
        DebugControls.SpawnRandomCreature += SpawnRandomCreature;
    }

    private void DefeatCreature()
    {
        if (--_numCreaturesAlive <= 0)
        {
            FloorManager.AllCreaturesDefeated?.Invoke();
        }
    }

    private void SpawnRandomCreature()
    {
        List<Creature> creatures = FloorManager.Instance.currentFloor.spawnableMajorCreatures;
        int index = UnityEngine.Random.Range(0, creatures.Count);
        Instantiate(creatures[index], Vector3.zero, Quaternion.identity);
        _numCreaturesAlive++;
    }

    public void SpawnPlaqueIcon(GameObject plaqueIcon, int doorIndex)
    {
        Transform plaqueIconParent = exitDoors[doorIndex].Find("PlaqueIconHolder").transform;
        GameObject spawnedPlaqueIcon = Instantiate(plaqueIcon, plaqueIconParent);
        spawnedPlaqueIcon.transform.Rotate(0, -90, 0);
    }
}
