using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegDrop : MonoBehaviour
{
    [SerializeField] Classification classification;
    [SerializeField] Weight weight;

    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public float instanceCurrentHealth { get; private set; }
    public void OverwriteHealth(float newHealth)
    {
        instanceCurrentHealth = newHealth;
    }
}
