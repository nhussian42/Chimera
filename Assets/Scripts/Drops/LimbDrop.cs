using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDrop : Drop
{
    [Header("Limb information")]
    [SerializeField] protected Weight weight;
    [SerializeField] protected Classification classification;
    [SerializeField] protected LimbType type;
    [SerializeField] protected Name limbName;

    private float limbHealth;

    public LimbType LimbType { get { return type; } }
    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public Name Name { get { return limbName; } } 
    public float LimbHealth { get { return limbHealth; } }

    public void OverwriteLimbHealth(float newValue)
    {
        limbHealth = newValue;
    }
}
