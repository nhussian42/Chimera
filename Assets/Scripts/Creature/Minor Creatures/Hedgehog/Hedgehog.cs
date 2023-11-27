using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Hedgehog : NotBossAI
{
    [SerializeField, Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 0.75f;
    [SerializeField, Tooltip("How fast it charges")] private float chargeSpeed = 10f;
    [SerializeField, Tooltip("How long it charges for")] private float chargeTime = 1f;
    [SerializeField, Tooltip("How long after a charge until it can charge again")] private float attackCooldown = 1f;
    [SerializeField] private float chargeMultiplier;
    [SerializeField] private float chargeDistance = 10f;
    [SerializeField] private float chargeKnockback;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Running", true);
    }

    public override IEnumerator Attack()
    {
        //Stops the movement
        knockbackForce = chargeKnockback;
        animator.SetBool("Charging", true);
        // OLDAudioManager.Instance.PlayMinEnemySFX("HedgehogSqueak");
        attacking = true;
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        AudioManager.PlaySound3D(AudioEvents.Instance.OnHedgehogAttack, transform.position);
        animator.SetBool("Attacking", true);
        agent.isStopped = true;
        float timer = 0;
        Vector3 endPos = player.transform.position;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            transform.LookAt(player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, endPos, chargeMultiplier * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.transform.position) < 1f)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(chargeTime);

        //Sets velocity to 0 and resumes movement
        animator.SetBool("Attacking", false);
        animator.SetBool("Charging", false);
        agent.isStopped = false;
        yield return new WaitForSeconds(attackCooldown);

        //Resets attack cooldown
        attacking = false;
        yield return null;
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.PlaySound3D(AudioEvents.Instance.OnHedgehogDeath, transform.position);
    }
}
