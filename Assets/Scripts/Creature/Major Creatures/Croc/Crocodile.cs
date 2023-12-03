using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;
using UnityEngine.VFX;

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
    [SerializeField] private GameObject burrowVFX;
    private GameObject particle;
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
        agent.stoppingDistance = 8;
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

        animator.SetBool("Charge", true);
        float timer = 0;
        Vector3 endPos = player.transform.position;
        transform.LookAt(endPos);
        if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
        {
            while (timer < chargeTime)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, endPos, chargeMultiplier * Time.deltaTime);
                if (Vector3.Distance(transform.position, player.transform.position) < 2f)
                {
                    break;
                }
                yield return null;
            }
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
        CameraShake.Instance.CreatureBurrowShake(true);
        particle = Instantiate(burrowVFX, transform.position + (transform.forward * 2), Quaternion.identity, this.transform);
        //Destroy(particle, 2f);
        animator.SetBool("Burrow", true);
        burrowing = true;
        crocBodyCollider.enabled = false;
        agent.stoppingDistance = 4;
        agent.isStopped = true;
        agent.speed = burrowSpeed;
        GetComponentInChildren<Canvas>().enabled = false;
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCrocBurrow, transform.position);
        yield return new WaitForSeconds(2f);

        //Allows the croc to start chasing until within range
        agent.isStopped = false;
        yield return new WaitUntil(() => agent.remainingDistance < 6f);

        //When in range, unburrows and resets speed
        CameraShake.Instance.CreatureBurrowShake(false);
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
        Destroy(particle);
        animator.SetBool("Idle", true);
        crocBodyCollider.enabled = true;
        attackDamage = regularAttackDamage;
        GetComponentInChildren<Canvas>().enabled = true;
        burrowing = false;
        agent.updateRotation = true;
        remainingDigCooldown = digCooldown;
        yield return new WaitForSeconds(1f);

        animator.SetBool("Idle", false);
        agent.isStopped = false;
        attacking = false;
        yield return null;
    }

    protected override void Update()
    {
        if (alive == true && stunned == false)
        {
            agent.destination = player.transform.position;

            if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask) && attacking == false)
            {
                StartCoroutine(Attack());
                attacking = true;
            }
            if (particle != null)
                particle.GetComponentInChildren<VisualEffect>().SetVector3("Position", transform.position + (transform.forward * 2));
        }

        remainingDigCooldown -= Time.deltaTime;
    }

    public override void ResetAttackBooleans()
    {
        base.ResetAttackBooleans();
        animator.SetBool("Idle", false);
        if (particle != null)
            Destroy(particle);
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCrocDeath, transform.position);
    }

    public override void TakeDamage(int damage)
    {
        if (burrowing == false)
        {
            base.TakeDamage(damage);
        }
    }

}
