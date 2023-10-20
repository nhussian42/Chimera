using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Limb : MonoBehaviour
{
    [SerializeField] protected Classification classification;
    [SerializeField] protected Weight weight;
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private Limb limbDropPrefab;

    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public float Health { get { return currentHealth; } set { currentHealth = Mathf.Clamp(value, minHealth, maxHealth); }}
    protected float currentHealth;
    protected float minHealth = 0f;
    protected float maxHealth = 100f;

    public float MaxHealth { get { return maxHealth; } }

    private void Awake()
    {
        currentHealth = Mathf.Clamp(startingHealth, minHealth, maxHealth);
    }
    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, minHealth, maxHealth);
    }
}
