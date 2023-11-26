using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegDrop : MonoBehaviour
{
    [SerializeField] Classification classification;
    [SerializeField] Weight weight;
    [SerializeField] Name limbName;

    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public Name Name {  get { return limbName; } }
    public float instanceCurrentHealth { get; private set; }
    public void OverwriteHealth(float newHealth)
    {
        instanceCurrentHealth = newHealth;
    }
}
