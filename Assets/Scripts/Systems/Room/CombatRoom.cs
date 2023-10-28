using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : Room
{
    public Creature currentMajorCreature;
    public Creature currentMinorCreature;
    public Transform majorCreatureSpawnsParent;
    public Transform minorCreatureSpawnsParent;
    public List<Transform> ExitDoors;
    int exitDoorIndex;
    
    private int _numCreaturesAlive;

    private void OnEnable()
    {
        CreatureManager.AnyCreatureDied += SubtractCreature;
    }

    private void OnDisable()
    {
        CreatureManager.AnyCreatureDied -= SubtractCreature;
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

    private void SubtractCreature()
    {
        if (--_numCreaturesAlive <= 0)
        {
            print("All creatures defeated!");
            FloorManager.AllCreaturesDefeated?.Invoke();
            // spawn limb at currentmajorcreature transform?????
        }
    }

    public void SpawnPlaqueIcon(GameObject plaqueIcon)
    {
        Transform plaqueIconParent = exitDoors[exitDoorIndex++].Find("PlaqueIconHolder").transform;

        Instantiate(plaqueIcon, plaqueIconParent.transform.position, Quaternion.identity, plaqueIconParent);
    }
}
