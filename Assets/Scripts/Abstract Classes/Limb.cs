using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Limb : MonoBehaviour
{
    // Exposed
    [SerializeField] protected Classification classification;
    [SerializeField] protected Weight weight;
    [SerializeField] private float defaultHealth;
    [SerializeField] private float defaultMaxHealth;

    [SerializeField] public Sprite limbSprite;
    [SerializeField] public Sprite selectedSprite;

    // Protected
    protected float currentHealth;
    protected float minHealth = 0;
    protected float maxHealth;

    // Public getters
    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public float Health { get { return currentHealth; } set { currentHealth = Mathf.Clamp(value, minHealth, maxHealth); }}
    public float MaxHealth { get { return maxHealth; } }

    public float DefaultHealth { get { return defaultHealth; } }
    public float DefaultMaxHealth { get { return defaultMaxHealth; } }

    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, minHealth, maxHealth);
        if(currentHealth <= minHealth) { Disintegrate(); }
    }

    // called by PlayerController at beginning of session for each limb
    public virtual void LoadDefaultStats()
    {
        maxHealth = defaultMaxHealth;
        currentHealth = Mathf.Clamp(defaultHealth, minHealth, maxHealth);
    }

    public virtual void Disintegrate()
    {
        // shader dissolve thing here maybe?
        
    }
}
