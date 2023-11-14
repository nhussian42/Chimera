using System.Collections;
using System.Collections.Generic;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Callbacks;
using UnityEditor.ShortcutManagement;
#endif
using UnityEngine;
using UnityEngine.AI;

public class Rhino : NotBossAI
{
    private float initialTurnSpeed;
    private float initialMovementSpeed;
    private float initialAcceleration;

    [SerializeField, Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 1f;
    [SerializeField, Tooltip("Acceleration during the charge")] private float chargeAcceleration = 4f;
    [SerializeField, Tooltip("Turn speed turning the charge, measured in degrees/second")] private float chargingTurnSpeed = 10f;
    [SerializeField, Tooltip("Maximum movement speed during charge")] private float chargeSpeed = 15f;

    [SerializeField, Tooltip("Range it begins slam attack")] private float slamAttackRange = 2f;

    [SerializeField, Tooltip("How long it takes for the slam attack to deal damage")] private float slamDelay = 1f;
    [SerializeField] private float chargeKnockback;
    [SerializeField] private float slamKnockback;

    [SerializeField] private CapsuleCollider attackCollider;
    [SerializeField] private BoxCollider chargeAttackCollider;

    private Rigidbody rb;
    private float baseAttackDamage;

    private void Start()
    {
        baseAttackDamage = attackDamage;
        initialTurnSpeed = agent.angularSpeed;
        initialMovementSpeed = agent.speed;
        initialAcceleration = agent.acceleration;

        rb = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {

        if (alive == true)
        {
            if (attacking == false)
            {
                agent.destination = player.transform.position;
                if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask))
                {
                    //Player is in range
                    //Perform attack coroutine
                    StartCoroutine(Attack());
                    attacking = true;
                }
            }
        }
    }

    public override IEnumerator Attack()
    {
        //Rhino stops
        StartCoroutine(FacePlayer());
        yield return new WaitForSeconds(1f);

        agent.isStopped = true;
        agent.speed = chargeSpeed;
        agent.angularSpeed = chargingTurnSpeed;
        agent.acceleration = chargeAcceleration;
        animator.SetBool("Charge", true);
        yield return new WaitForSeconds(chargeDelay);

        //Rhino runs towards player until within slam attack range
        chargeAttackCollider.enabled = true;
        agent.isStopped = false;
        float timer = 1f;
        while (Physics.CheckSphere(transform.position, slamAttackRange, playerLayerMask) == false)
        {
            agent.destination = player.transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(agent.steeringTarget - transform.position), Time.deltaTime);
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                agent.speed += 1;
                agent.angularSpeed += 1;
                agent.acceleration += 1;
                timer = 2f;
                attackDamage = Mathf.RoundToInt(agent.velocity.magnitude);
                knockbackForce = Mathf.Clamp(knockbackForce, Mathf.RoundToInt(agent.velocity.magnitude), 50f);
            }
            yield return null;
        }

        attackDamage = baseAttackDamage;
        //Starts slam attack
        animator.SetBool("Charge", false);
        StartCoroutine(SlamAttack());
        yield return new WaitForSeconds(0.5f);
        chargeAttackCollider.enabled = false;
        yield break;
    }

    public IEnumerator SlamAttack()
    {
        //Stops the rhino
        knockbackForce = slamKnockback;
        agent.isStopped = true;
        animator.SetBool("Slam", true);
        yield return new WaitForSeconds(slamDelay);

        //Attack is performed
        agent.velocity = Vector3.zero;
        attackCollider.enabled = true;
        agent.isStopped = false;
        yield return new WaitForSeconds(0.5f);

        //Attack ends, resets rhino to normal movement
        attackCollider.enabled = false;
        agent.angularSpeed = initialTurnSpeed;
        agent.speed = initialMovementSpeed;
        agent.acceleration = initialAcceleration;
        animator.SetBool("Slam", false);

        //Resets attack cooldown
        Vector3 targetPos;
        if (RandomPoint(player.transform.position, attackRange, out targetPos))
        {
            //Debug.Log(targetPos);
            agent.destination = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            yield return new WaitForSeconds(1);
            yield return null;
        }

        yield return new WaitUntil(() => agent.remainingDistance < 4f);
        attacking = false;

        yield return null;
    }

    private IEnumerator FacePlayer()
    {
        float timer = 3f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            Vector3 targetDirection = player.transform.position - transform.position;
            float singleStep = 2.5f * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = center + Random.onUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    protected override void Die()
    {
        animator.Play("Death");
        agent.isStopped = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        alive = false;
        CreatureManager.AnyCreatureDied?.Invoke();
        Destroy(this.gameObject, 3.5f);
        StopAllCoroutines();
    }

}
