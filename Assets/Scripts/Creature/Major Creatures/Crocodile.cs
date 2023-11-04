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
    private float baseSpeed;
    [SerializeField] private float burrowSpeed;
    private float remainingDigCooldown = 0f; //Actual value that track remaining dig cooldown
    [SerializeField] private MeshCollider attackCollider;
    [SerializeField] private float burrowAttackDamage;
    private float regularAttackDamage;
    MeshCollider meshCollider;
    private bool burrowing;

    protected override void InitializeStats(float percentDamageIncrease, float percentHealthIncrease)
    {
        base.InitializeStats(percentDamageIncrease, percentHealthIncrease);
        // burrowAttackDamage += attackDamage * percentDamageIncrease * 0.01f + burrowFlatDamageIncrease;
        // * burrowPercentDamageIncrease * 0.01f; or something
    }

    protected void Start()
    {
        regularAttackDamage = attackDamage;
        meshCollider = GetComponentInChildren<MeshCollider>();
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
        agent.stoppingDistance = 7;
        yield return new WaitUntil(() => agent.remainingDistance < 10f);

        rb.AddForce(gameObject.transform.forward * agent.remainingDistance, ForceMode.Impulse);
        animator.SetBool("Charge", true);
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector3.zero;
        attackCollider.enabled = false;
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
        //Dig animation goes here
        animator.SetBool("Burrow", true);
        burrowing = true;
        meshCollider.enabled = false;
        agent.stoppingDistance = 2;
        agent.isStopped = true;
        agent.speed = burrowSpeed;
        yield return new WaitForSeconds(1f);

        //Enemy is invisible moving behind player
        //Maybe put particle effect here?
        GetComponentInChildren<Canvas>().enabled = false;
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        yield return new WaitUntil(() => agent.remainingDistance < 4f);

        //Enemy appears behind the player
        //Surfacing animation goes here
        animator.SetBool("BurrowResurface", true);
        animator.SetBool("Burrow", false);
        agent.isStopped = true;
        agent.updateRotation = false;
        agent.speed = baseSpeed;
        gameObject.transform.LookAt(player.transform.position);
        yield return new WaitForSeconds(0.5f);

        //Charges forward
        //Charging animation goes here
        animator.SetBool("BurrowResurface", false);
        gameObject.transform.LookAt(player.transform.position);
        meshCollider.enabled = true;
        yield return new WaitForSeconds(0.75f);

        //Resets speed, puts dig on cooldown
        attackDamage = regularAttackDamage;
        GetComponentInChildren<Canvas>().enabled = true;
        burrowing = false;
        agent.isStopped = false;
        agent.updateRotation = true;
        remainingDigCooldown = digCooldown;
        yield return new WaitForSeconds(1f);
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
