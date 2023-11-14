using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Variables/Float Variable", order = 1)]
public class FloatVar : ScriptableObject
{
    public float value { get; private set; }

    public void Write(float val)
    {
        value = val;
    }
}
