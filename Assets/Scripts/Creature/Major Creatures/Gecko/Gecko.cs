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
        Vector3 startPos = transform.position;
        Vector3 endPos = Vector3.Lerp(transform.position, player.transform.position, 0.5f);
        Vector3 middlePos = Vector3.Lerp(transform.position, endPos, 0.3f);
        animator.SetBool("Dash", true);
        float timer = 0;
        if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
        {
            AudioManager.PlaySound3D(AudioEvents.Instance.OnGeckoDash, transform.position);
            transform.LookAt(middlePos);
            while (timer < chargeTime)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, middlePos + (transform.right * 3), chargeSpeed * Time.deltaTime);
                yield return null;
            }
        }
        animator.SetBool("Dash", false);

        animator.SetBool("DashAttack", true);
        AudioManager.PlaySound3D(AudioEvents.Instance.OnGeckoAttack, transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
        {
            endPos = Vector3.Lerp(transform.position, player.transform.position, 0.5f);
            transform.LookAt(endPos);
            timer = 0;
            while (timer < chargeTime)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chargeSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, player.transform.position) < agent.stoppingDistance)
                {
                    break;
                }
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.25f);

        animator.SetBool("DashAttack", false);
        animator.SetBool("DashBack", true);
        AudioManager.PlaySound3D(AudioEvents.Instance.OnGeckoDash, transform.position);

        timer = 0;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, startPos, chargeSpeed * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("Fleeing", true);
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
        agent.destination = PointOnXZCircle(transform.position, 15f, Random.Range(0, 361));
        agent.speed = 24;
        agent.acceleration = 24;
        while (remainingDashAttackCooldown > 0)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 9f)
            {
                agent.destination = PointOnXZCircle(transform.position, 25f, Random.Range(0, 361));
                yield return new WaitForSeconds(1.5f);
            }
            yield return null;
        }
        animator.SetBool("Fleeing", false);
        animator.SetBool("Idle", false);
        agent.speed = 6;
        agent.acceleration = 8;
        fleeing = false;
        attacking = false;
        yield return null;
    }

    protected override void Update()
    {
        if (stunned != true)
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
                if (agent.remainingDistance < 0.1f && fleeing == true)
                {
                    animator.SetBool("Idle", true);
                }
                else
                {
                    animator.SetBool("Idle", false);
                }
            }

            remainingDashAttackCooldown -= Time.deltaTime;
        }
    }

    public override void ResetAttackBooleans()
    {
        base.ResetAttackBooleans();
        fleeing = false;
        animator.SetBool("Dash", false);
        animator.SetBool("DashAttack", false);
        animator.SetBool("DashBack", false);
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

    protected override void Die()
    {
        base.Die();
        AudioManager.PlaySound3D(AudioEvents.Instance.OnGeckoDeath, transform.position);
        Destroy(stunnedFX);
    }
}
