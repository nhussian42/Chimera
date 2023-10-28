using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Floor", menuName = "Scriptable Objects/Floor", order = 1)]
public class FloorSO : ScriptableObject
{
    [Header("Room Information")]
    [Tooltip("Which floor # is it?")]
    public int index;
    [Tooltip("How many combat rooms will the player pass through before the boss?")]
    public int numCombatRooms;
    [Tooltip("Which major creatures will spawn on this floor?")]
    public List<Creature> spawnableMajorCreatures;
    [Tooltip("Which minor creatures will spawn on this floor?")]
    public Creature mammalianMinorCreature;
    public Creature reptilianMinorCreature;
    public Creature aquaticMinorCreature;
    [Tooltip("Which coombat room prefabs will spawn on this floor?")]
    public List<CombatRoom> spawnableCombatRooms;
    [Tooltip("What is the boss room prefab for this floor?")]
    public BossRoom bossRoom;
    [Tooltip("What is the prefab for the boss?")]
    public Creature boss;
    [Tooltip("What is the prefab for the plaque before the boss room?")]
    public GameObject bossPlaque;

    [Header("Major Creature Scaling")]
    public float majorHealthPercentGainedPerRoom;
    public float majorDamagePercentGainedPerRoom;

    [Header("Minor Creature Scaling")]
    public float minorHealthPercentGainedPerRoom;
    public float minorDamagePercentGainedPerRoom;
}
