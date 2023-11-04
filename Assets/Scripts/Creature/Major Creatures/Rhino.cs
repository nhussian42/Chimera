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

    [SerializeField] private CapsuleCollider attackCollider;

    private Rigidbody rb;

    private void Start()
    {
        initialTurnSpeed = agent.angularSpeed;
        initialMovementSpeed = agent.speed;
        initialAcceleration = agent.acceleration;

        rb = GetComponent<Rigidbody>();
    }

    public override IEnumerator Attack()
    {
        //Rhino stops
        agent.isStopped = true;
        agent.speed = chargeSpeed;
        agent.angularSpeed = chargingTurnSpeed;
        agent.acceleration = chargeAcceleration;
        yield return new WaitForSeconds(chargeDelay);

        //Rhino runs towards player until within slam attack range
        agent.isStopped = false;
        float timer = 1f;
        while (Physics.CheckSphere(transform.position, slamAttackRange, playerLayerMask) == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(agent.steeringTarget - transform.position), Time.deltaTime);
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                agent.speed += 1;
                agent.angularSpeed += 1;
                agent.acceleration += 1;
                timer = 2f;
            }
            yield return null;
        }

        //Starts slam attack
        StartCoroutine(SlamAttack());
        yield break;
    }

    public IEnumerator SlamAttack()
    {
        //Stops the rhino
        agent.isStopped = true;
        yield return new WaitForSeconds(0.5f);

        //Attack is performed
        agent.velocity = Vector3.zero;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);

        //Attack ends, resets rhino to normal movement
        attackCollider.enabled = false;
        agent.isStopped = false;
        agent.angularSpeed = initialTurnSpeed;
        agent.speed = initialMovementSpeed;
        agent.acceleration = initialAcceleration;

        //Resets attack cooldown
        yield return new WaitForSeconds(1.5f);
        attacking = false;

        yield return null;
    }

    void FaceTarget()
    {
        var turnTowardNavSteeringTarget = agent.steeringTarget;

        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

}
