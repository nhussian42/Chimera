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
    [SerializeField] private GameObject attackCollider;
    [SerializeField] private float pounceKnockback;

    private float circleTimer;
    private bool inAttackRange = false; //Keeps track of if the wolf is in attack range
    private Vector3 dir;    //Stores direction towards the player
    private float angle;    //Stores the angle around the player
    private bool randomPositiveNegative; //Determines if the wolf runs clockwise or counterclockwise
    float attackResetTime = 2f; //How long the wolf runs away before engaging again
    bool changeDirectionCooldown = false;
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
            if ((Vector3.Distance(agent.destination, hit.position) < 2f) && changeDirectionCooldown == false)
            {
                changeDirectionCooldown = true;
                Invoke("ResetCooldown", 1f);
                randomPositiveNegative = !randomPositiveNegative;
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
        circleTimer = Random.Range(3, 6);

        dir = (player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180;
        agent.destination = PointOnXZCircle(player.transform.position, attackRange, angle);
        yield return new WaitUntil(() => agent.remainingDistance < 1f);

        float timer = 0;
        while (timer < circleTimer)
        {
            timer += Time.deltaTime;
            agent.destination = PointOnXZCircle(player.transform.position, attackRange, angle);
            if (randomPositiveNegative)
            {
                angle += 4;
            }
            else
            {
                angle -= 4;
            }

            if (Vector3.Distance(transform.position, player.transform.position) < 5)
            {
                break;
            }

            yield return new WaitForSeconds(0.001f);
            yield return null;
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
        attackCollider.SetActive(true);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(gameObject.transform.forward * (Vector3.Distance(transform.position, player.transform.position) * chargeMultiplier), ForceMode.Impulse);
        yield return new WaitForSeconds(chargeTime);

        attackCollider.SetActive(false);
        animator.SetBool("Attack", false);
        rb.velocity = Vector3.zero;
        agent.isStopped = false;
        attacking = false;

        agent.destination = PointOnXZCircle(transform.position, attackRange * 2, Random.Range(-45, 46));
        attackResetTime = 2f;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, agent.destination) < 1f || attackResetTime < 0f);

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
