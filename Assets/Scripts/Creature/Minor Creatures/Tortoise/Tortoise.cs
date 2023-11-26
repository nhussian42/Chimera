using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tortoise : NotBossAI
{
    [SerializeField, Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 0.75f;
    [SerializeField, Tooltip("How long it charges for")] private float chargeTime = 1f;
    [SerializeField, Tooltip("How long after a charge until it can charge again")] private float attackCooldown = 1f;
    [SerializeField, Tooltip("How fast the charge is")] private float chargeSpeed;
    [SerializeField, Tooltip("How much the attack knocks the player back")] private float chargeKnockback;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Running", true);
    }

    public override IEnumerator Attack()
    {
        //Stops the movement
        knockbackForce = chargeKnockback;
        animator.SetBool("Attacking", true);
        attacking = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        float timer = 0;
        Vector3 endPos = player.transform.position;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, endPos, chargeSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.transform.position) < 0.5f)
            {
                break;
            }
            yield return null;
        }
        agent.isStopped = false;
        yield return new WaitForSeconds(attackCooldown);

        //Resets attack cooldown
        attacking = false;
        yield return null;
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.PlaySound3D(AudioEvents.Instance.OnTortoiseDeath, transform.position);
    }
}
