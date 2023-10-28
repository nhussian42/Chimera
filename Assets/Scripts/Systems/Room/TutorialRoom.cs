using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : Room
{
    [field: SerializeField] public RoomSide OppositeEnterSide { get; private set; }
    protected override void OnEnable()
    {
        base.OnEnable();
        DebugControls.SpawnDebugCreature += SpawnDebugCreature;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DebugControls.SpawnDebugCreature -= SpawnDebugCreature;
    }

    private void SpawnDebugCreature()
    {
        Instantiate(FloorManager.Instance.currentFloor.spawnableMajorCreatures[0], Vector3.zero, Quaternion.identity);
        _numCreaturesAlive++;
    }
}
