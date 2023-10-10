using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public abstract class Creature : MonoBehaviour
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

    protected Animator animator;
    protected NavMeshAgent agent;
    protected bool alive = true;
    
    private void Awake()
    {
        //Sets current room
    }

    private void SpawnDrop(GameObject objToSpawn)
    {
        //Drops the specified object
    }

    private void CalculateStats()
    {
        //Pulls stats from FloorManager based on CreatureType enum
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        animator.SetBool("Death", true);
        agent.isStopped = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        alive = false;
        Destroy(this.gameObject, 1f);
        //Something happens
        //Death
    }
}
