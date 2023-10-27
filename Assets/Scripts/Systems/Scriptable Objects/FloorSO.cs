using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Floor", menuName = "Scriptable Objects/FloorSO", order = 1)]
public class FloorSO : ScriptableObject
{
    [Header("Room Information")]
    [Tooltip("Which floor # is it?")]
    public int index;
    [Tooltip("How many combat rooms will the player pass through before the boss?")]
    public int numCombatRooms;
    [Tooltip("Which creature classifications will spawn on this floor?")]
    public List<Classification> spawnableClassifications;
    [Tooltip("Which coombat room prefabs will spawn on this floor?")]
    public List<CombatRoom> spawnableCombatRooms;
    [Tooltip("What is the boss room prefab for this floor?")]
    public BossRoom bossRoom;

    [Header("Major Creature Scaling")]
    public float majorHealthPercentGainedPerRoom;
    public float majorDamagePercentGainedPerRoom;

    [Header("Minor Creature Scaling")]
    public float minorHealthPercentGainedPerRoom;
    public float minorDamagePercentGainedPerRoom;
}
