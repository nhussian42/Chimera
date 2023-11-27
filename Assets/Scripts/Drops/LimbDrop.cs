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
    [SerializeField] public float limbCost;

    private float limbHealth;

    public LimbType LimbType { get { return type; } }
    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public Name Name { get { return limbName; } } 
    public float LimbHealth { get { return limbHealth; } }
    public float LimbCost { get { return limbCost; } }

    private void OnEnable()
    {
        //if (limbHealth == 0)
        //{
        //    limbHealth = 100f;
        //}
        //Debug.Log("limbHealth: " + limbHealth);

    }

    private void Start()
    {
        Vector3 forceDirection = PointOnXZCircle(transform.position, 1f, Random.Range(0,360)).normalized * 20f + Vector3.up * 100f;
        GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);
    }

    public void OverwriteLimbHealth(float newValue)
    {
        limbHealth = newValue;
    }
}
