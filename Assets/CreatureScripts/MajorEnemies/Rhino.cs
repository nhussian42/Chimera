using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.AI;

public class Rhino : NotBossAI
{
    private float initialTurnSpeed;
    private float initialMovementSpeed;
    private float initialAcceleration;

    [SerializeField] private float chargeDelay = 1f;
    [SerializeField] private float chargeAcceleration = 4f;
    [SerializeField] private float chargingTurnSpeed = 10f;
    [SerializeField] private float chargeSpeed = 15f;

    [SerializeField] private float slamAttackRange = 2f;



    private void OnEnable()
    {
        initialTurnSpeed = agent.angularSpeed;
        initialMovementSpeed = agent.speed;
        initialAcceleration = agent.acceleration;

    }

    public override IEnumerator Attack()
    {
        Debug.Log("Attacking");
        agent.isStopped = true;
        agent.acceleration = chargeAcceleration;
        yield return new WaitForSeconds(chargeDelay);

        agent.angularSpeed = chargingTurnSpeed;
        agent.isStopped = false;
        agent.speed = chargeSpeed;

        // while (Physics.CheckSphere(transform.position, attackRange, playerLayerMask))
        // {
        //     yield return null;
        // }

        yield break;
    }

    // public IEnumerator SlamAttack()
    // {
    //     Debug.Log("Starting slam attack");
    //     agent.isStopped = true;
    //     yield return new WaitForSeconds(0.5f);

    //     Debug.Log("Attacked");
    //     slamAttackCollider.enabled = true;
    //     yield return new WaitForSeconds(0.5f);

    //     Debug.Log("Resetting");
    //     slamAttackCollider.enabled = false;
    //     agent.isStopped = false;
    //     agent.angularSpeed = initialTurnSpeed;
    //     agent.speed = initialMovementSpeed;
    //     agent.acceleration = initialAcceleration;

    //     //Resets attack cooldown
    //     yield return new WaitForSeconds(2f);
    //     attacking = false;

    //     yield return null;
    // }
}
