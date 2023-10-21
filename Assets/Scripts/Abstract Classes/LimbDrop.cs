using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDrop : MonoBehaviour
{
    
    [SerializeField] protected Weight weight;
    [SerializeField] protected Classification classification;
    [SerializeField] protected LimbType type;

    private float limbHealth;

    public LimbType LimbType { get { return type; } }
    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public float LimbHealth { get { return limbHealth; } }

    private void OnEnable()
    {
        if (limbHealth == 0)
        {
            limbHealth = 100f;
        }

    }

    public void OverwriteLimbHealth(float newValue)
    {
        limbHealth = newValue;
    }

    
}
