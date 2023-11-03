using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Variables/Bool Variable", order = 1)]
public class BoolVar : ScriptableObject
{
    public bool value { get; private set; }

    public void Write(bool val)
    {
        value = val;
    }
}
