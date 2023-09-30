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

    [SerializeField] private float chargeDelay = 1f;
    [SerializeField] private float chargeAcceleration = 4f;
    [SerializeField] private float chargingTurnSpeed = 10f;
    [SerializeField] private float chargeSpeed = 15f;

    [SerializeField] private float slamAttackRange = 2f;

    [SerializeField] private GameObject attackIndicator;

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

        while (Physics.CheckSphere(transform.position, slamAttackRange, playerLayerMask) == false)
        {           
            yield return null;
        }

        StartCoroutine(SlamAttack());
        yield break;
    }

    public IEnumerator SlamAttack()
    {
        Debug.Log("Starting slam attack");
        agent.isStopped = true;
        agent.velocity = Vector3.one;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Attacked");
        attackIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Resetting");
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
