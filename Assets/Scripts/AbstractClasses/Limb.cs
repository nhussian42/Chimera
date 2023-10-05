using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Limb : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool defaultLimb;
    [SerializeField] private Classification classification;

    private float currentHealth;
    private float minHealth;
    private float maxHealth;

    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, minHealth, maxHealth);
    }
}
