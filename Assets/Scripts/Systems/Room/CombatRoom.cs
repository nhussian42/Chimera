using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : Room
{
    [HideInInspector] public Creature currentMajorCreature;
    [HideInInspector] public Creature currentMinorCreature;
    public Transform majorCreatureSpawnsParent;
    public Transform minorCreatureSpawnsParent;
    int exitDoorIndex;

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

    public void DetermineCreatures(FloorSO floorInfo)
    {
        List<Creature> majorCreaturePool = floorInfo.spawnableMajorCreatures;
        int majorCreatureIndex = UnityEngine.Random.Range(0, majorCreaturePool.Count);
        currentMajorCreature = majorCreaturePool[majorCreatureIndex];
        DetermineMinorCreature(floorInfo, currentMajorCreature.classification);
    }

    private void DetermineMinorCreature(FloorSO floorInfo, Classification classification)
    {
        switch (classification)
        {
            case Classification.Mammalian:
                currentMinorCreature = floorInfo.mammalianMinorCreature;
                break;
            case Classification.Reptilian:
                currentMinorCreature = floorInfo.reptilianMinorCreature;
                break;
            case Classification.Aquatic:
                currentMinorCreature = floorInfo.aquaticMinorCreature;
                break;
            default:
                Debug.LogError("Classification was not accounted for when spawning a minor creature.");
                break;
        }
    }

    public void SpawnCreatures(Transform parent)
    {
        foreach (Transform creatureSpawn in majorCreatureSpawnsParent)
        {
            Instantiate(currentMajorCreature, creatureSpawn.position, Quaternion.identity, parent);
            _numCreaturesAlive++;
        }

        foreach (Transform creatureSpawn in minorCreatureSpawnsParent)
        {
            Instantiate(currentMinorCreature, creatureSpawn.position, Quaternion.identity, parent);
            _numCreaturesAlive++;
        }
    }

    private void SpawnDebugCreature()
    {
        Instantiate(currentMajorCreature, Vector3.zero, Quaternion.identity);
        _numCreaturesAlive++;
    }

    public void SpawnPlaqueIcon(GameObject plaqueIcon)
    {
        // cursed
        Transform plaqueIconParent = exitDoors[exitDoorIndex].Find("PlaqueIconHolder").transform;
        
        GameObject spawnedPlaqueIcon = Instantiate(plaqueIcon, plaqueIconParent);
        spawnedPlaqueIcon.transform.Rotate(0, -90, 0);

        // DEBUG: adding a manual quaternion rotation for different room sides
        // LeaveRoomTrigger exitDoorTrigger = exitDoors[exitDoorIndex].GetComponentInChildren<LeaveRoomTrigger>();

        // if (exitDoorTrigger == null)
        // {
        //     Debug.LogError("Exit Door Trigger not found to instantiate plaque icon");
        //     return;
        // }

        // if (exitDoorTrigger._exitRoomSide == RoomSide.Left)
        //     Instantiate(plaqueIcon, plaqueIconParent.transform.position, Quaternion.identity, plaqueIconParent);
        // else if (exitDoorTrigger._exitRoomSide == RoomSide.Right)
        //     Instantiate(plaqueIcon, plaqueIconParent.transform.position, Quaternion.identity * Quaternion.Euler(0, 90, 0), plaqueIconParent);
        // else
        // {
        //     Debug.LogError("Side unexpected when instantiating plaque icon.");
        //     return;
        // }

        exitDoorIndex++;

    }
}
