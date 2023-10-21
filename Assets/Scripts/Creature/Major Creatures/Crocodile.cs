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
    private bool underground = false;

    private bool performingDigAttack = false;

    
    [SerializeField] private float digCooldown = 20f; //Cooldown between uses of dig
    private float remainingDigCooldown = 0f; //Actual value that track remaining dig cooldown
    [SerializeField] private MeshCollider attackCollider;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        rb = GetComponent<Rigidbody>();
        agent.destination = player.transform.position;
    }

    public override IEnumerator Attack()
    {
        if (remainingDigCooldown < 0f)
        {
            Debug.Log("Dig attack");
            StartCoroutine(Dig());
        }
        else
        {
            Debug.Log("Regular attack");
            StartCoroutine(RegularAttack());
        }
        yield return null;
    }

    protected IEnumerator RegularAttack()
    {
        Debug.Log("Just walking at the player");
        agent.stoppingDistance = 6;
        while (agent.remainingDistance > agent.stoppingDistance)
        {
            agent.destination = player.transform.position;
            FaceTarget(agent.destination);
            yield return null;
        }


        Debug.Log("Attack collider enabled");
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Attack collider disabled");
        attackCollider.enabled = false;
        yield return new WaitForSeconds(1f);

        Debug.Log("Movement resumed");
        attacking = false;
        yield return null;
    }
    protected IEnumerator Dig()
    {
        agent.stoppingDistance = 2;
        performingDigAttack = true;
        Debug.Log("Digging...");
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);

        Debug.Log("Underground");
        agent.speed = agent.speed * 2;
        underground = true;
        agent.isStopped = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false; 
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => agent.remainingDistance < 1f);

        Debug.Log("Appear near the player");
        underground = false;
        agent.isStopped = true;
        agent.updateRotation = false;
        gameObject.transform.LookAt(player.transform.position);
        GetComponentInChildren<Canvas>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Charge the player");
        gameObject.transform.LookAt(player.transform.position);
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(0.75f);

        Debug.Log("Stop moving, reset behavior");
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


        // if (alive == true && underground == false)
        // {
        //     agent.destination = player.transform.position;
        //     if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask) && attacking == false)
        //     {
        //         //Player is in range
        //         //Perform attack coroutine
        //         StartCoroutine(Attack());
        //         attacking = true;
        //     }
        // }

        // if(underground == true && alive == true)
        // {
        //     agent.destination = player.transform.position + (player.transform.forward * -7f);
        // }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
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
