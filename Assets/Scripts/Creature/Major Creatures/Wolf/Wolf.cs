using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class Wolf : NotBossAI
{
    [SerializeField, Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 0.75f;
    [SerializeField, Tooltip("How long it charges for")] private float chargeTime = 0.5f;
    [SerializeField, Tooltip("Multiplier to how fast the wolf charges")] private float chargeMultiplier;
    [SerializeField] private float pounceKnockback;


    private bool inAttackRange = false; //Keeps track of if the wolf is in attack range
    private Vector3 dir;    //Stores direction towards the player
    private float angle;    //Stores the angle around the player
    private bool randomPositiveNegative; //Determines if the wolf runs clockwise or counterclockwise
    bool agentNearEdge = false; //Keeps track of if the wolf is near the edge of the navmesh
    float attackResetTime = 2f; //How long the wolf runs away before engaging again
    bool changeDirectionCooldown = false;
    public bool attackCompleted = false;
    private bool circling = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        attackResetTime -= Time.deltaTime;
        if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask))
        {
            //Player is in range
            inAttackRange = true;
        }

        if (alive == true)
        {
            if (inAttackRange == false && circling == false)
            {
                agent.destination = player.transform.position;
            }
            else if (inAttackRange == true)
            {
                if (circling == false)
                {
                    StartCoroutine(Attack());
                    circling = true;
                }
            }

            agent.FindClosestEdge(out NavMeshHit hit);
            if (Vector3.Distance(transform.position, hit.position) < 1.5f)
            {
                agentNearEdge = true;
            }
            else
            {
                agentNearEdge = false;
            }

            if (agentNearEdge == true && changeDirectionCooldown == false)
            {
                changeDirectionCooldown = true;
                Invoke("ResetCooldown", 1f);
                randomPositiveNegative = !randomPositiveNegative;
                if (randomPositiveNegative == true)
                {
                    angle += 30;
                }
                else
                {
                    angle -= 30;
                }
                agent.destination = PointOnXZCircle(player.transform.position, attackRange, angle);
            }
        }
    }

    private void ResetCooldown()
    {
        changeDirectionCooldown = !changeDirectionCooldown;
    }



    public override IEnumerator Attack()
    {
        //Circle the player for a set period of time
        randomPositiveNegative = Random.value > 0.5f;
        int randomValue = Random.Range(10, 21);

        dir = (player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180;

        for (int i = 0; i < randomValue; i++)
        {
            if (randomPositiveNegative == true)
            {
                angle += 30;
            }
            else
            {
                angle -= 30;
            }

            agent.destination = PointOnXZCircle(player.transform.position, attackRange, angle);

            if (Vector3.Distance(player.transform.position, transform.position) < 4f)
            {
                StartCoroutine(Pounce());
                yield break;
            }

            yield return new WaitUntil(() => agent.remainingDistance < 2f);
        }

        StartCoroutine(Pounce());

        yield return null;
    }

    public IEnumerator Pounce()
    {
        knockbackForce = pounceKnockback;
        animator.SetBool("Attack", true);
        AudioManager.Instance.PlayMajEnemySFX("WolfBark");
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        StartCoroutine(RotateTowardsTarget(player.transform.position, chargeDelay));
        yield return new WaitForSeconds(chargeDelay);

        attacking = true;
        float timer = 0;
        Vector3 endPos = player.transform.position;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, endPos, chargeMultiplier * Time.deltaTime);
            yield return null;
        }

        yield return new WaitUntil(() => attackCompleted == true);
        attackCompleted = false;
        animator.SetBool("Attack", false);
        agent.isStopped = false;
        attacking = false;

        agent.destination = PointOnXZCircle(transform.position, attackRange * 2, Random.Range(-45, 46));
        attackResetTime = 3f;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, player.transform.position) > attackRange * 1.5f || attackResetTime < 0f);

        circling = false;
        yield return null;
    }

    private Vector3 PointOnXZCircle(Vector3 center, float radius, float angle)
    {
        float a = angle * Mathf.PI / 180f;
        return center + new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * radius;
    }

    private IEnumerator RotateTowardsTarget(Vector3 position, float turnTime)
    {
        float timer = 0;
        while (timer < turnTime)
        {
            timer += Time.deltaTime;
            Vector3 direction = (position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, timer / turnTime);
            yield return null;
        }
        yield return null;
    }
}
