using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.ShortcutManagement;
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

    [SerializeField] private GameObject attackIndicator;

    private void OnEnable()
    {
        initialTurnSpeed = agent.angularSpeed;
        initialMovementSpeed = agent.speed;
        initialAcceleration = agent.acceleration;
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
        while (Physics.CheckSphere(transform.position, slamAttackRange, playerLayerMask) == false)
        {           
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
        agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);

        //Attack is performed
        GetComponent<BoxCollider>().enabled = true;
        attackIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //Attack ends, resets rhino to normal movement
        GetComponent<BoxCollider>().enabled = false;
        agent.isStopped = false;
        attackIndicator.SetActive(false);
        agent.angularSpeed = initialTurnSpeed;
        agent.speed = initialMovementSpeed;
        agent.acceleration = initialAcceleration;

        //Resets attack cooldown
        yield return new WaitForSeconds(1.5f);
        attacking = false;

        yield return null;
    }
}
