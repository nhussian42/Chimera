using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Limb : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool defaultLimb;
    [SerializeField] private Classification classification;
    public bool isPickup {get; private set;}
    private Collider collider;

    private float currentHealth;
    private float minHealth;
    private float maxHealth;

    private void OnEnable()
    {
        collider = GetComponent<Collider>();
        collider.enabled = true;
        isPickup = true;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, minHealth, maxHealth);
    }

    public void SwitchGameState()
    {
        if (isPickup == true)
        {
            isPickup = false;
            collider.enabled = false;
        }
        else
        {
            isPickup = true;
            collider.enabled = true;
        }        
    }
}
