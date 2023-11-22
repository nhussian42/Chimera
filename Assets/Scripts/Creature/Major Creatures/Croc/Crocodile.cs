using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Crocodile : NotBossAI
{
    private Rigidbody rb;

    [SerializeField] private float digCooldown = 20f; //Cooldown between uses of dig
    [SerializeField] private float chargeTime = 0.5f;
    [SerializeField] private float chargeMultiplier = 2f;
    private float baseSpeed;
    [SerializeField] private float burrowSpeed;
    private float remainingDigCooldown = 0f; //Actual value that track remaining dig cooldown
    [SerializeField] private float burrowAttackDamage;
    [SerializeField] private float regularAttackDamage;
    [SerializeField] private BoxCollider crocBodyCollider;
    [SerializeField] private float regularAttackKnockback;
    [SerializeField] private float burrowAttackKnockback;
    private bool burrowing;

    protected override void InitializeStats(float percentDamageIncrease, float percentHealthIncrease)
    {
        base.InitializeStats(percentDamageIncrease, percentHealthIncrease);
        // burrowAttackDamage += attackDamage * percentDamageIncrease * 0.01f + burrowFlatDamageIncrease;
        // * burrowPercentDamageIncrease * 0.01f; or something
    }

    protected void Start()
    {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        rb = GetComponent<Rigidbody>();
        agent.destination = player.transform.position;
        animator = GetComponentInChildren<Animator>();
        baseSpeed = agent.speed;
    }

    public override IEnumerator Attack()
    {
        //Begins dig attack if off cooldown, otherwise perform regular attack
        if (remainingDigCooldown < 0f)
        {
            attackDamage = burrowAttackDamage;
            StartCoroutine(Dig());
        }
        else
        {
            attackDamage = regularAttackDamage;
            StartCoroutine(RegularAttack());
        }
        yield return null;
    }

    protected IEnumerator RegularAttack()
    {
        //Walks up to the player and attacks in a small cone
        knockbackForce = regularAttackKnockback;
        agent.stoppingDistance = 6;
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

        animator.SetBool("Charge", true);
        float timer = 0;
        Vector3 endPos = player.transform.position;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, endPos, chargeMultiplier * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.transform.position) < 2f)
            {
                break;
            }
            yield return null;
        }
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCrocAttack, transform.position);
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Charge", false);
        agent.isStopped = true;
        animator.SetBool("Idle", true);
        yield return new WaitForSeconds(2f);

        agent.isStopped = false;
        animator.SetBool("Idle", false);
        attacking = false;
        yield return null;
    }
    protected IEnumerator Dig()
    {
        //Disables collider, increases speed, makes the croc burrow
        knockbackForce = burrowAttackKnockback;
        animator.SetBool("Burrow", true);
        burrowing = true;
        crocBodyCollider.enabled = false;
        agent.stoppingDistance = 2;
        agent.isStopped = true;
        agent.speed = burrowSpeed;
        GetComponentInChildren<Canvas>().enabled = false;
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCrocBurrow, transform.position);
        yield return new WaitForSeconds(2f);

        //Allows the croc to start chasing until within range
        agent.isStopped = false;
        yield return new WaitUntil(() => agent.remainingDistance < 4f);

        //When in range, unburrows and resets speed
        animator.SetBool("BurrowResurface", true);
        animator.SetBool("Burrow", false);
        agent.isStopped = true;
        agent.speed = baseSpeed;
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCrocResurface, transform.position);
        yield return new WaitForSeconds(0.5f);

        //Damage gets dealt here
        animator.SetBool("BurrowResurface", false);
        yield return new WaitForSeconds(0.75f);

        //Burrow attack collider disabled, croc can take damage again, attack damage reset, goes back to chasing
        crocBodyCollider.enabled = true;
        attackDamage = regularAttackDamage;
        GetComponentInChildren<Canvas>().enabled = true;
        burrowing = false;
        agent.isStopped = false;
        agent.updateRotation = true;
        remainingDigCooldown = digCooldown;
        attacking = false;
        yield return null;
    }

    protected override void Update()
    {
        if (alive == true)
        {
            FaceTarget(agent.destination);
            agent.destination = player.transform.position;

            if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask) && attacking == false)
            {
                StartCoroutine(Attack());
                attacking = true;
            }
        }

        remainingDigCooldown -= Time.deltaTime;
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCrocDeath, transform.position);
        animator.Play("Death");
    }

    public override void TakeDamage(int damage)
    {
        if (burrowing == false)
        {
            base.TakeDamage(damage);
        }
    }

}
