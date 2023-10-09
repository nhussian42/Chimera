using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Limb : MonoBehaviour
{
    [SerializeField] protected Classification classification;
    //public bool isPickup {get; private set;}
    //private Collider collider;
    
    // [SerializeField] private bool defaultLimb;
    // [SerializeField] private Classification classification;
    // public bool isPickup {get; private set;}

    [SerializeField] private float startingHealth = 100f;

    public float Health { get { return currentHealth; }}
    protected float currentHealth;
    protected float minHealth = 0f;
    protected float maxHealth = 100f;

    private void Awake()
    {
        currentHealth = Mathf.Clamp(startingHealth, minHealth, maxHealth);
    }

    private void OnEnable()
    {
        //collider = GetComponent<Collider>();
        //collider.enabled = true;
        //isPickup = true;

    }

    public void RemoveHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, minHealth, maxHealth);
    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, minHealth, maxHealth);
    }

    //public void SwitchGameState()
    //{
    //    if (isPickup == true)
    //    {
    //        isPickup = false;
    //        collider.enabled = false;
    //    }
    //    else
    //    {
    //        isPickup = true;
    //        collider.enabled = true;
    //    }        
    //}
}
