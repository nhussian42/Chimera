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
    protected float knockbackForce = 5;
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

    // [SerializeField] List<GameObject> drops;

    protected Animator animator;
    protected NavMeshAgent agent;
    protected bool alive = true;
    [SerializeField] protected EnemyHealthBar healthbar;

    // private void Awake()
    // {
    //     //Sets current room
    // }

    protected virtual void OnEnable()
    {
        DebugControls.DamageAllCreatures += TakeDamage;
        FloorManager.AllCreaturesDefeated += DestroyCreature;
    }

    protected virtual void OnDisable()
    {
        DebugControls.DamageAllCreatures -= TakeDamage;
        FloorManager.AllCreaturesDefeated -= DestroyCreature;
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
            //TrinketManager.Instance.StartKillSkills();  - commented this out temporarily - Amon

        }
        else if (alive == true && iFrame == false)
        {
            iFrame = true;
            Invoke("IFrame", iFrameDuration);
            animator.SetTrigger("TakeDamage");
            
            // Blanket audio event for all creatures taking damage, may be replaced by individual creature sounds
            AudioManager.PlaySound3D(AudioEvents.Instance.OnCreatureDamaged, transform.position);
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
        alive = false;
        GetComponent<BoxCollider>().enabled = false;

        CreatureManager.AnyCreatureDied?.Invoke();
        if (creatureType == CreatureType.Minor)
            DestroyCreature();

        //Destroy(this.gameObject, 1.5f);
        StopAllCoroutines();
        //Something happens
        //Death
    }

    private void DestroyCreature()
    {
        // play the dissolve shader
        Destroy(this.gameObject, 1.5f);
    }

    public void Knockback(Vector3 knockbackDir, float knockbackForce, float knockbackDuration)
    {
        if (alive == true)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (GetComponent<Rigidbody>() != null)
            {
                float timer = 0;
                while (timer < knockbackDuration)
                {
                    timer += Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, knockbackDir * knockbackForce, Time.deltaTime);
                }
            }

        }

    }

    public IEnumerator PlayerKnockback(Vector3 knockbackDir, float knockbackForce, float knockbackDuration)
    {
        float timer = knockbackDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            knockbackDir = new Vector3(knockbackDir.x, 0, knockbackDir.z);
            float knockbackDistance = Mathf.Lerp(0, knockbackForce * 2, timer);
            PlayerController.Instance._controller.Move(knockbackDir.normalized * knockbackDistance * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
}
