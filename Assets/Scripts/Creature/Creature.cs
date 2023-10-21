using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public abstract class Creature : MonoBehaviour
{
    [Header("Creature Stats")]
    [SerializeField] protected float health;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float attackDamage = 5f;

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
    [SerializeField] private EnemyHealthBar healthbar;
    
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
        currentHealth -= damage;
        healthbar.UpdateHealthBar(currentHealth, health);
        if (currentHealth <= 0 && alive == true)
        {
            Die();
        }
        else if (alive == true)
        {
            animator.Play("Take Damage");
        }
    }

    protected virtual void Die()
    {
        SpawnDrop();
        AudioManager.Instance.PlayMinEnemySFX("HedgehogDie");
        animator.Play("Death");
        agent.isStopped = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        alive = false;
        Destroy(this.gameObject, 1f);
        StopAllCoroutines();
        //Something happens
        //Death
    }

    protected void SpawnDrop()
    {
       foreach(GameObject drop in drops)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }
}
