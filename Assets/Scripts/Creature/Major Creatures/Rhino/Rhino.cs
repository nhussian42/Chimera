using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Callbacks;
using UnityEditor.ShortcutManagement;
#endif
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

    [SerializeField, Tooltip("How long it takes for the slam attack to deal damage")] private float slamDelay = 1f;
    [SerializeField] private float chargeKnockback;
    [SerializeField] private float slamKnockback;
    public bool slammed = false;

    private Rigidbody rb;
    private float baseAttackDamage;
    private bool moving = true;

    FMOD.Studio.EventInstance RhinoCharge;

    private void Start()
    {
        agent.updatePosition = false;

        baseAttackDamage = attackDamage;
        initialTurnSpeed = agent.angularSpeed;
        initialMovementSpeed = agent.speed;
        initialAcceleration = agent.acceleration;

        rb = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {

        if (alive == true)
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

        if (moving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * agent.speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(agent.steeringTarget - transform.position), Time.deltaTime);
        }
    }

    public override IEnumerator Attack()
    {
        //Rhino stops
        StartCoroutine(FacePlayer(2));
        yield return new WaitUntil(() => Physics.Raycast(transform.position, transform.forward, Mathf.Infinity, playerLayerMask));

        Debug.Log("We see the player");
        moving = false;
        agent.speed = chargeSpeed;
        agent.angularSpeed = chargingTurnSpeed;
        agent.acceleration = chargeAcceleration;
        animator.SetBool("Charge", true);

        yield return new WaitForSeconds(chargeDelay);
        moving = true;
        //Rhino runs towards player until within slam attack range
        RhinoCharge = AudioManager.Instance.CreateEventInstance(AudioEvents.Instance.OnRhinoCharge);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(RhinoCharge, transform);
        RhinoCharge.start();
        float timer = 1f;
        while (Physics.CheckSphere(transform.position, slamAttackRange, playerLayerMask) == false)
        {
            agent.destination = player.transform.position;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                agent.speed += 1;
                agent.angularSpeed += 1;
                agent.acceleration += 1;
                timer = 2f;
                attackDamage = Mathf.RoundToInt(agent.velocity.magnitude);
                knockbackForce = Mathf.Clamp(knockbackForce, Mathf.RoundToInt(agent.velocity.magnitude), 50f);
            }
            yield return null;
        }

        attackDamage = baseAttackDamage;
        //Starts slam attack
        animator.SetBool("Charge", false);
        StartCoroutine(SlamAttack());
        RhinoCharge.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        RhinoCharge.release();
        yield return new WaitForSeconds(0.5f);
        yield break;
    }

    public IEnumerator SlamAttack()
    {
        //Stops the rhino
        knockbackForce = slamKnockback;
        moving = false;
        animator.SetBool("Slam", true);

        //Attack is performed
        yield return new WaitUntil(() => slammed == true);

        //Attack ends, resets rhino to normal movement
        moving = true;
        agent.speed = initialMovementSpeed;
        animator.SetBool("Slam", false);
        slammed = false;

        //Resets attack cooldown
        Vector3 targetPos;
        if (RandomPoint(player.transform.position, attackRange, out targetPos))
        {
            agent.destination = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            yield return new WaitForSeconds(1);
            yield return null;
        }

        yield return new WaitUntil(() => Vector3.Distance(transform.position, agent.destination) < 2f);
        attacking = false;

        yield return null;
    }

    private IEnumerator FacePlayer(int rotationTime)
    {
        float timer = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        Quaternion startRotation = transform.rotation;
        while (timer < rotationTime)
        {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, lookRotation, timer / rotationTime);
            yield return null;
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = center + Random.onUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    protected override void Die()
    {
        animator.Play("Death");
        RhinoCharge.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        RhinoCharge.release();
        AudioManager.PlaySound3D(AudioEvents.Instance.OnRhinoDeath, transform.position);
        agent.isStopped = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        alive = false;
        CreatureManager.AnyCreatureDied?.Invoke();
        Destroy(this.gameObject, 3.5f);
        StopAllCoroutines();
    }
}
