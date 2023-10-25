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
    private float chargeSpeed = 20f;

    private bool performingDigAttack = false;


    [SerializeField] private float digCooldown = 20f; //Cooldown between uses of dig
    private float remainingDigCooldown = 0f; //Actual value that track remaining dig cooldown
    [SerializeField] private MeshCollider attackCollider;
    BoxCollider boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        rb = GetComponent<Rigidbody>();
        agent.destination = player.transform.position;
    }

    public override IEnumerator Attack()
    {
        //Begins dig attack if off cooldown, otherwise perform regular attack
        if (remainingDigCooldown < 0f)
        {
            StartCoroutine(Dig());
        }
        else
        {
            StartCoroutine(RegularAttack());
        }
        yield return null;
    }

    protected IEnumerator RegularAttack()
    {
        //Walks up to the player and attacks in a small cone
        agent.stoppingDistance = 6;
        while (agent.remainingDistance > agent.stoppingDistance)
        {
            agent.destination = player.transform.position;
            FaceTarget(agent.destination);
            yield return null;
        }

        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);

        attackCollider.enabled = false;
        yield return new WaitForSeconds(1f);

        attacking = false;
        yield return null;
    }
    protected IEnumerator Dig()
    {
        //Dig animation goes here
        agent.stoppingDistance = 2;
        performingDigAttack = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);

        //Enemy is invisible moving behind player
        //Maybe put particle effect here?
        agent.speed = agent.speed * 2;
        agent.isStopped = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        boxCollider.enabled = false;
        GetComponentInChildren<Canvas>().enabled = false; 
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => agent.remainingDistance < 1f);

        //Enemy appears behind the player
        //Surfacing animation goes here
        agent.isStopped = true;
        agent.updateRotation = false;
        gameObject.transform.LookAt(player.transform.position);
        boxCollider.enabled = true;
        GetComponentInChildren<Canvas>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);

        //Charges forward
        //Charging animation goes here
        gameObject.transform.LookAt(player.transform.position);
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(0.75f);

        //Resets speed, puts dig on cooldown
        agent.speed = agent.speed / 2;
        agent.isStopped = false;
        agent.updateRotation = true;
        rb.velocity = Vector3.zero;
        remainingDigCooldown = digCooldown;
        performingDigAttack = false;
        attacking = false;
        yield return null;
    }

    protected override void Update()
    {

        if (alive == true)
        {
            if (performingDigAttack == false)
            {
                FaceTarget(agent.destination);
                agent.destination = player.transform.position;

                if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask) && attacking == false)
                {
                    StartCoroutine(Attack());
                    attacking = true;
                }   
            }

            if (performingDigAttack == true)
            {
                agent.destination = player.transform.position + (player.transform.forward * -7f);
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
}
