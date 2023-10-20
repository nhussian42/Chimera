using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wolf : NotBossAI
{
    [SerializeField, Tooltip("How long it stops once in range before beginning the charge")] private float chargeDelay = 0.75f;
    [SerializeField, Tooltip("How fast it charges")] private float chargeSpeed = 10f;
    [SerializeField, Tooltip("How long it charges for")] private float chargeTime = 0.5f;
    [SerializeField, Tooltip("How long after a charge until it can charge again")] private float attackCooldown = 1f;

    [SerializeField] private GameObject attackCollider;

    private float baseSpeed;
    private Vector3 dir;
    private float angle;
    private bool circling;
    private void Start()
    {
        agent.destination = player.transform.position;
        baseSpeed = agent.speed;
        animator = GetComponentInChildren<Animator>();
        
    }
    public override IEnumerator Attack()
    {
        circling = false;
        attacking = true;
        dir = (player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180;
        int randomValue = Random.Range(12, 24);
        int randomPositiveNegative = Random.Range(0, 2);
        for (int i = 0; i < randomValue; i++)
        {
            if (randomPositiveNegative == 0)
            {
                angle += 15;
            }
            else
            {
                angle -= 15;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(agent.steeringTarget - transform.position), Time.deltaTime);
            yield return new WaitForSeconds(0.2f);
        }
        agent.destination = player.transform.position;
        agent.speed = 2f;
        StartCoroutine(Pounce());

        // AudioManager.Instance.PlayMajEnemySFX("WolfBark");

        circling = false;
        yield return null;
    }

    public IEnumerator Pounce()
    {
        animator.SetBool("Attack", true);
        //Stops the movement
        attacking = true;
        float rotationDuration = chargeDelay;
        var lookPos = player.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        // while (rotationDuration > 0f)
        // {
        //     rotationDuration -= Time.deltaTime;
        //     transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
        // }
        agent.velocity = Vector3.zero;
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        AudioManager.Instance.PlayMajEnemySFX("WolfSlash");
        agent.isStopped = true;
        agent.speed = baseSpeed;
        attackCollider.SetActive(true);
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(chargeTime);

        //Sets velocity to 0 and resumes movement
        attackCollider.SetActive(false);
        rb.velocity = Vector3.zero;
        agent.isStopped = false;
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(attackCooldown);

        //Resets attack cooldown
        attacking = false;
        yield return null;
        yield return null;
    }

    public IEnumerator CircleTarget()
    {

        yield return null;
    }

    private Vector3 PointOnXZCircle(Vector3 center, float radius, float angle)
    {
        float a = angle * Mathf.PI / 180f;
        return center + new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * radius;
    }

    protected override void Update()
    {
        if (circling == false && attacking == false && alive == true)
        {
            agent.destination = player.transform.position;
            if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask))
            {
                //Player is in range
                //Perform attack coroutine
                StartCoroutine(Attack());
                circling = true;
                AudioManager.Instance.PlayMajEnemySFX("WolfBark");

            }

        }
        else if (circling == true && alive == true)
        {
            agent.destination = PointOnXZCircle(player.transform.position, attackRange, angle);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
        }
    }

    protected override void Die()
    {
        AudioManager.Instance.PlayMajEnemySFX("WolfDeath");

        SpawnDrop();
        animator.Play("Death");
        agent.isStopped = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        alive = false;
        StopAllCoroutines();
        Destroy(this.gameObject, 2f);
        //Something happens
        //Death
    }
}
