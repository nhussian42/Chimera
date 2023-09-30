using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [Header("Creature Stats")]
    [SerializeField] private float health;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange = 5f;

    public enum Classification
    {
        Mammal,
        Reptile,
        Aquatic
    }

    [SerializeField] private Classification classification;

    public enum CreatureType
    {
        Minor,
        Major
    }

    [SerializeField] private CreatureType creatureType;

    [SerializeField] List<GameObject> drops;

    
    

    private void Awake()
    {
        //Sets current room
    }

    private void SpawnDrop(GameObject objToSpawn)
    {
        //Drops the specified object
    }

    private void CalculateStats()
    {
        //Pulls stats from FloorManager based on CreatureType enum
    }
}
