using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "Scriptable Objects/Creature", order = 1)]
public class CreatureSO : ScriptableObject
{
    public string creatureName;
    //public List<LimbDrop> limbDrops;
    public GameObject plaqueIcon;
}
