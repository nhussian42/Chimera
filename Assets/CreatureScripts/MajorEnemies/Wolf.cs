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

    [SerializeField] private GameObject attackIndicator;

    private float baseSpeed;
    private Vector3 dir;
    private void Start()
    {
        agent.destination = player.transform.position;
        baseSpeed = agent.speed;
        
    }
    public override IEnumerator Attack()
    {
        attacking = true;
        dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180;
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
            agent.destination = PointOnXZCircle(player.transform.position, attackRange, angle);
            yield return new WaitForSeconds(0.3f);
        }
        agent.destination = player.transform.position;
        agent.speed = 2f;
        StartCoroutine(Pounce());
        yield return null;
    }

    public IEnumerator Pounce()
    {
        //Stops the movement
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
        attackIndicator.SetActive(true);
        yield return new WaitForSeconds(chargeDelay);

        //Charges foward
        agent.isStopped = true;
        agent.speed = baseSpeed;
        attackIndicator.SetActive(false);
        rb.AddForce(gameObject.transform.forward * chargeSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(chargeTime);

        //Sets velocity to 0 and resumes movement
        rb.velocity = Vector3.zero;
        agent.isStopped = false;
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
        if (attacking == false)
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
    }
}
