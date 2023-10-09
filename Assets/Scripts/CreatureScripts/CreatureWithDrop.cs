using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureWithDrop : MonoBehaviour
{
    [Header("Creature Stats")]
    [SerializeField] private float health;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange = 5f;

    public enum Classification
    {
        Mammal,
        Reptile,
        Aquatic
    }

    [SerializeField] private Classification classification;

    public enum CreatureType
    {
        Minor,
        Major
    }

    [SerializeField] private CreatureType creatureType;

    [SerializeField] List<GameObject> drops;

    
    

    private void Awake()
    {
        //Sets current room
    }

    private void SpawnDrop()
    {
       foreach(GameObject drop in drops)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }

    private void CalculateStats()
    {
        //Pulls stats from FloorManager based on CreatureType enum
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        SpawnDrop();
    }
}
