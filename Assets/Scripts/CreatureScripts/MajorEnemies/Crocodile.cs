using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Crocodile : NotBossAI
{
    private Rigidbody rb;
    private float chargeSpeed = 20f;
    private bool underground = false;
    [SerializeField] private float digCooldown = 0f;
    [SerializeField] private float startingDigCooldown = 20f;
    [SerializeField] private MeshCollider attackCollider;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent.destination = player.transform.position;
    }

    public override IEnumerator Attack()
    {
        if (digCooldown < 0f)
        {
            Debug.Log("Dig attack");
            StartCoroutine(Dig());
        }
        else
        {
            Debug.Log("Regular attack");
            StartCoroutine(RegularAttack());
        }
        yield return null;
    }

    protected IEnumerator RegularAttack()
    {
        Debug.Log("Just walking at the player");
        attacking = true;
        yield return new WaitUntil(() => agent.remainingDistance < 4f);

        agent.isStopped = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);

        attackCollider.enabled = false;
        yield return new WaitForSeconds(1.5f);

        agent.isStopped = false;
        attacking = false;
        yield return null;
    }
    protected IEnumerator Dig()
    {
        attacking = true;
        Debug.Log("Digging...");
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);

        Debug.Log("Underground");
        underground = true;
        agent.isStopped = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        healthbar.enabled = false;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => agent.remainingDistance < 1f);

        Debug.Log("Appear near the player");
        underground = false;
        agent.isStopped = true;
        gameObject.transform.LookAt(player.transform.position);
        healthbar.enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(1f);

        Debug.Log("Charge the player");
        gameObject.transform.LookAt(player.transform.position);
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(0.75f);

        Debug.Log("Stop moving, reset behavior");
        agent.isStopped = false;
        rb.velocity = Vector3.zero;
        digCooldown = startingDigCooldown;
        attacking = false;

        yield return null;
    }

    protected override void Update()
    {

        if (attacking == false && alive == true && underground == false)
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

        if(underground == true && alive == true)
        {
            agent.destination = player.transform.position + (player.transform.forward * -7f);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
        }

        digCooldown -= Time.deltaTime;
    }
}
