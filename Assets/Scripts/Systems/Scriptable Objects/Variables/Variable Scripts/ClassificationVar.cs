using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Variables/Classification Variable", order = 1)]
public class ClassificationVar : ScriptableObject
{
    public Classification value { get; private set; }

    public void Write(Classification classification)
    {
        value = classification;
    }
}
