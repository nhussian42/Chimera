using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : Room
{
    public Creature currentMajorCreature;
    public Creature currentMinorCreature;
    public Transform majorCreatureSpawnsParent;
    public Transform minorCreatureSpawnsParent;
    
    private int _numCreaturesAlive;

    private void OnEnable()
    {
        CreatureManager.AnyCreatureDied += SubtractCreature;
    }

    private void OnDisable()
    {
        CreatureManager.AnyCreatureDied -= SubtractCreature;
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
}
