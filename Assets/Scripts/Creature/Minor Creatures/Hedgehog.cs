using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Hedgehog : NotBossAI
{
    [SerializeField, Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 0.75f;
    [SerializeField, Tooltip("How fast it charges")] private float chargeSpeed = 10f;
    [SerializeField, Tooltip("How long it charges for")] private float chargeTime = 0.5f;
    [SerializeField, Tooltip("How long after a charge until it can charge again")] private float attackCooldown = 1f;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Running", true);
    }
    public override IEnumerator Attack()
    {
        //Stops the movement
        animator.SetBool("Charging", true);
        AudioManager.Instance.PlayMinEnemySFX("HedgehogSqueak");
        attacking = true;
        agent.velocity = Vector3.zero;
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        AudioManager.Instance.PlayMinEnemySFX("HedgehogAttack");
        animator.SetBool("Attacking", true);
        agent.isStopped = true;
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(chargeTime);

        //Sets velocity to 0 and resumes movement
        animator.SetBool("Attacking", false);
        animator.SetBool("Charging", false);
        rb.velocity = Vector3.zero;
        agent.isStopped = false;
        yield return new WaitForSeconds(attackCooldown);

        //Resets attack cooldown
        attacking = false;
        yield return null;
    }
}
