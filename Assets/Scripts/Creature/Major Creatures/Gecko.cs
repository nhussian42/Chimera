using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gecko : NotBossAI
{
    [SerializeField, Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 0.5f;
    [SerializeField, Tooltip("How fast it charges")] private float chargeSpeed = 15f;
    [SerializeField, Tooltip("How long it charges for")] private float chargeTime = 0.5f;
    [SerializeField, Tooltip("How long after a charge until it can charge again")] private float attackCooldown = 1f;

    [SerializeField] private float dashAttackCooldown = 3f;
    private float remainingDashAttackCooldown = 0f;
    private bool fleeing = false;
    public override IEnumerator Attack()
    {
        if (remainingDashAttackCooldown < 0)
        {
            StartCoroutine(DashAttack());
        }
        else
        {
            StartCoroutine(Flee());
        }
        yield return null;
    }

    private IEnumerator DashAttack()
    {
        //Stops the movement
        attacking = true;
        agent.velocity = Vector3.zero;
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        agent.isStopped = true;
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        int random = Random.Range(1, 3);
        if (random == 1)
        {
            rb.AddForce(gameObject.transform.right * chargeSpeed/2, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(-gameObject.transform.right * chargeSpeed/2, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(chargeTime/2);

        rb.velocity = Vector3.zero;
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        if (random == 2)
        {
            rb.AddForce(gameObject.transform.right * chargeSpeed/2, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(-gameObject.transform.right * chargeSpeed/2, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(chargeTime/2);        
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(chargeTime);

        rb.AddForce(-gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        if (random == 1)
        {
            rb.AddForce(gameObject.transform.right * chargeSpeed/2, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(-gameObject.transform.right * chargeSpeed/2, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(chargeTime*2);

        //Sets velocity to 0 and resumes movement
        rb.velocity = Vector3.zero;
        agent.isStopped = false;
        remainingDashAttackCooldown = dashAttackCooldown;
        yield return new WaitForSeconds(attackCooldown);

        //Resets attack cooldown
        attacking = false;
        yield return null;
    }

    private IEnumerator Flee()
    {
        agent.speed = agent.speed * 2;
        yield return new WaitUntil(() => remainingDashAttackCooldown < 0);
        agent.speed = agent.speed / 2;
        fleeing = false;
        attacking = false;
        yield return null;
    }

    protected override void Update()
    {
        if (alive == true && remainingDashAttackCooldown < 0)
        {
            FaceTarget(agent.destination);
            agent.destination = player.transform.position;

            if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask) && attacking == false)
            {
                StartCoroutine(DashAttack());
                attacking = true;
            }
        }
        else if (alive == true)
        {
            Vector3 dirToPlayer = transform.position - player.transform.position;
            agent.destination = transform.position + dirToPlayer;

            if (fleeing == false)
            {
                StartCoroutine(Flee());
                fleeing = true;
            }
  
        }

        remainingDashAttackCooldown -= Time.deltaTime;
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);  
    }
}
