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
    [SerializeField] private GameObject attackCollider;


    private bool inAttackRange = false; //Keeps track of if the wolf is in attack range
    private Vector3 dir;    //Stores direction towards the player
    private float angle;    //Stores the angle around the player
    private int randomPositiveNegative; //Determines if the wolf runs clockwise or counterclockwise
    bool agentNearEdge = false; //Keeps track of if the wolf is near the edge of the navmesh
    float attackResetTime = 2f; //How long the wolf runs away before engaging again

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
            if (inAttackRange == false)
            {
                agent.destination = player.transform.position;
            }
            else if (inAttackRange == true)
            {
                if (attacking == false)
                {
                    StartCoroutine(Attack());
                    attacking = true;
                }

                agent.FindClosestEdge(out NavMeshHit hit);
                if (Vector3.Distance(transform.position, hit.position) < 0.75f)
                {
                    agentNearEdge = true;
                }
                else
                {
                    agentNearEdge = false;
                }
            }
        }
    }

    public override IEnumerator Attack()
    {
        //Circle the player for a set period of time
        int randomValue = Random.Range(6, 13);
        randomPositiveNegative = Random.Range(1, 3);
        dir = (player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180;

        for (int i = 0; i < randomValue; i++)
        {
            if (randomPositiveNegative == 0)
            {
                angle += 30;
            }
            else
            {
                angle -= 30;
            }
            agent.destination = PointOnXZCircle(player.transform.position, attackRange, angle);

            yield return new WaitUntil(() => agent.remainingDistance < 2.5f || (agentNearEdge == true && inAttackRange == true));
        }

        StartCoroutine(Pounce());

        yield return null;
    }

    public IEnumerator Pounce()
    {
        animator.SetBool("Attack", true);
        AudioManager.Instance.PlayMajEnemySFX("WolfBark");
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        StartCoroutine(RotateTowardsTarget(player.transform.position, chargeDelay));
        yield return new WaitForSeconds(chargeDelay);

        attackCollider.SetActive(true);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(gameObject.transform.forward * 15, ForceMode.Impulse);
        yield return new WaitForSeconds(chargeTime);

        attackCollider.SetActive(false);
        animator.SetBool("Attack", false);
        rb.velocity = Vector3.zero;
        agent.isStopped = false;

        agent.destination = PointOnXZCircle(transform.position, attackRange * 2, Random.Range(-45, 46));
        attackResetTime = 3f;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, player.transform.position) > attackRange * 1.5f || attackResetTime < 0f);

        attacking = false;
        yield return null;
    }

    private Vector3 PointOnXZCircle(Vector3 center, float radius, float angle)
    {
        float a = angle * Mathf.PI / 180f;
        return center + new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * radius;
    }

    private IEnumerator RotateTowardsTarget(Vector3 position, float turnTime)
    {
        float timer = turnTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            Vector3 direction = (position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 7);
            yield return null;
        }
        yield return null;
    }
}
