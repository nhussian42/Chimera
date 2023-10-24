using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorSO", menuName = "ScriptableObjects/FloorSO", order = 1)]
public class FloorSO : ScriptableObject
{
    [Header("Room Information")]
    [Tooltip("Which floor # is it?")]
    public int index;
    [Tooltip("How many combat rooms will the player pass through before the boss?")]
    public int numCombatRooms;
    [Tooltip("Which creature classifications will spawn on this floor?")]
    public List<Classification> spawnableClassifications;
    [Tooltip("Which room prefabs will spawn on this floor?")]
    public List<Room> spawnableRooms;
    [Tooltip("What is the boss room prefab for this floor?")]
    public Room bossRoom;

    [Header("Major Creature Scaling")]
    public int majorHealthGainedPerRoom;
    public int majorDamageGainedPerRoom;

    [Header("Minor Creature Scaling")]
    public int minorHealthGainedPerRoom;
    public int minorDamageGainedPerRoom;
}
