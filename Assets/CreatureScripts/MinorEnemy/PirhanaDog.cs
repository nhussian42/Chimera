using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirhanaDog : NotBossAI
{
    [SerializeField][Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 0.75f;
    [SerializeField][Tooltip("How fast it charges")] private float chargeSpeed = 10f;
    [SerializeField][Tooltip("How long it charges for")] private float chargeTime = 0.5f;
    [SerializeField][Tooltip("How long after a charge until it can charge again")] private float attackCooldown = 1f;

    public override IEnumerator Attack()
    {
        //Stops the movement
        agent.isStopped = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(chargeTime);

        //Sets velocity to 0 and resumes movement
        rb.velocity = Vector3.zero;
        agent.isStopped = false;
        yield return new WaitForSeconds(attackCooldown);

        //Resets attack cooldown
        attacking = false;
        yield return null;
    }
}
