using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Limb : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] protected Classification classification;
    //public bool isPickup {get; private set;}
    //private Collider collider;
    
    // [SerializeField] private bool defaultLimb;
    // [SerializeField] private Classification classification;
    // public bool isPickup {get; private set;}

    public float Health { get { return currentHealth; }}
    protected float currentHealth;
    protected float minHealth;
    protected float maxHealth;

    private void OnEnable()
    {
        //collider = GetComponent<Collider>();
        //collider.enabled = true;
        //isPickup = true;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, minHealth, maxHealth);

        PlayerController.OnDamageReceived?.Invoke();
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
