using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public override IEnumerator Attack()
    {
        if (remainingDashAttackCooldown < 0)
        {
            StartCoroutine(DashAttack());
        }
        yield return null;
    }

    private IEnumerator DashAttack()
    {
        //Stops the movement
        attacking = true;
        agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        agent.isStopped = true;
        Vector3 endPos = Vector3.Lerp(transform.position, player.transform.position, 0.7f);
        Vector3 middlePos = Vector3.Lerp(transform.position, endPos, 0.3f);
        animator.SetBool("Dash", true);
        float timer = 0;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.LookAt(endPos);
            transform.position = Vector3.MoveTowards(transform.position, middlePos + (transform.right * 3), chargeSpeed * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("Dash", false);
        animator.SetBool("DashAttack", true);
        endPos = Vector3.Lerp(transform.position, player.transform.position, 0.5f);
        transform.LookAt(endPos);
        timer = 0;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, chargeSpeed * Time.deltaTime);
            if (Vector3.Distance(endPos, transform.position) < 0.1f)
            {
                timer = chargeTime;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("DashAttack", false);
        animator.SetBool("DashBack", true);
        Vector3 fleePos = PointOnXZCircle(endPos, 7f, Random.Range(-30, 31));
        timer = 0;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.LookAt(endPos);
            transform.position = Vector3.MoveTowards(transform.position, fleePos, chargeSpeed * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("DashBack", false);
        agent.isStopped = false;
        remainingDashAttackCooldown = dashAttackCooldown;
        StartCoroutine(Flee());
        fleeing = true;
        yield return new WaitForSeconds(attackCooldown);

        //Resets attack cooldown
        attacking = false;
        yield return null;
    }

    private IEnumerator Flee()
    {
        agent.destination = PointOnXZCircle(player.transform.position, 15f, Random.Range(-45, 46));
        while (remainingDashAttackCooldown > 0)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 1f)
            {
                agent.destination = PointOnXZCircle(player.transform.position, 15f, Random.Range(-45, 46));
                Debug.Log("New flee destination");
            }
            yield return new WaitForSeconds(2f);
        }
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

    private Vector3 PointOnXZCircle(Vector3 center, float radius, float angle)
    {
        float a = angle * Mathf.PI / 180f;
        return center + new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * radius;
    }
}
