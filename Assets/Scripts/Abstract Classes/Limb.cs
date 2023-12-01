using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Limb : MonoBehaviour
{
    // Exposed
    [SerializeField] protected Classification classification;
    [SerializeField] protected Weight weight;
    [SerializeField] public Name limbName;
    [SerializeField] private string stringName;
    [SerializeField] private float defaultHealth;
    [SerializeField] private float defaultMaxHealth;
    [SerializeField] public Sprite limbSprite;
    [SerializeField] public Sprite selectedSprite;

    // Protected
    protected float currentHealth;
    protected float minHealth = 0;
    protected float maxHealth;

    protected bool dissolving;

    // Public getters
    public Classification Classification { get { return classification; } }
    public Name Name { get { return limbName; } }
    public string StringName { get { return stringName; } }
    public Weight Weight { get { return weight; } }
    public Sprite LimbSprite { get { return limbSprite; } }
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
        if (dissolving) return;
        dissolving = true;

        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerLimbLost);

        foreach (DissolveObject limb in GetComponentsInChildren<DissolveObject>())
        {
            limb.Dissolve(false);
        }
    }

    private void OnDisable()
    {
        dissolving = false;
    }
}
