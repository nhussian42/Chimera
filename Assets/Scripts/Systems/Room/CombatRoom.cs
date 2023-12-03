using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : Room
{
    [HideInInspector] public Creature currentMajorCreature;
    [HideInInspector] public Creature currentMinorCreature;
    public Transform majorCreatureSpawnsParent;
    public Transform minorCreatureSpawnsParent;


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
}
