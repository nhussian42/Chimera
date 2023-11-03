using System;
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
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float attackDamage = 5f;
    [SerializeField] protected float iFrameDuration = 1f; //iFrame for creatures ONLY controls animations
    private bool iFrame = false;

    [field: SerializeField] public CreatureSO CreatureInfo { get; private set; }

    protected virtual void InitializeStats(float percentDamageIncrease, float percentHealthIncrease)
    {
        attackDamage += attackDamage * percentDamageIncrease * 0.01f;
        currentHealth += currentHealth * percentHealthIncrease * 0.01f;
    }

    [field: SerializeField] public Classification classification { get; private set; }

    public enum CreatureType
    {
        Minor,
        Major
    }

    [SerializeField] private CreatureType creatureType;

    //[SerializeField] List<GameObject> drops;

    protected Animator animator;
    protected NavMeshAgent agent;
    protected bool alive = true;
    [SerializeField] protected EnemyHealthBar healthbar;

    // private void Awake()
    // {
    //     //Sets current room
    // }

    public void OnEnable()
    {
        DebugControls.DamageAllCreatures += TakeDamage;
    }

    public void OnDisable()
    {
        DebugControls.DamageAllCreatures -= TakeDamage;
    }

    private void SpawnDrop(GameObject objToSpawn)
    {
        //Drops the specified object
    }

    private void CalculateStats()
    {
        //Pulls stats from FloorManager based on CreatureType enum
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.UpdateHealthBar(currentHealth, health);
        if (currentHealth <= 0 && alive == true)
        {
            Die();
            TrinketManager.Instance.StartKillSkills();

        }
        else if (alive == true && iFrame == false)
        {
            iFrame = true;
            Invoke("IFrame", iFrameDuration);
            animator.SetTrigger("TakeDamage");
        }
    }

    private void IFrame()
    {
        animator.ResetTrigger("TakeDamage");
        iFrame = false;
    }

    protected virtual void Die()
    {
        // SpawnDrop();
        animator.Play("Death");
        agent.isStopped = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        alive = false;
        CreatureManager.AnyCreatureDied?.Invoke();
        Destroy(this.gameObject, 1.5f);
        StopAllCoroutines();
        //Something happens
        //Death
    }

    public void Knockback(Vector3 knockbackDir, float knockbackForce, float knockbackDuration)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        Debug.Log("Knocking back");
        if (this.GetComponent<Rigidbody>() != null)
        {
            rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
        }

        float timer = knockbackDuration;
        agent.isStopped = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0 && rb != null)
        {
            Debug.Log("Knockback stopped");
            rb.velocity = Vector3.zero;
            agent.isStopped = false;
        }
    }

    public IEnumerator PlayerKnockback(Vector3 knockbackDir, float knockbackForce, float knockbackDuration)
    {
        float timer = knockbackDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float knockbackDistance = Mathf.Lerp(0, knockbackForce / 2, timer);
            PlayerController.Instance._controller.Move(knockbackDir * knockbackDistance * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    // protected void SpawnDrop()
    // {
    //    foreach(GameObject drop in drops)
    //     {
    //         Instantiate(drop, transform.position, Quaternion.identity);
    //     }
    // }
}
