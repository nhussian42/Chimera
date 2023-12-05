using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEditor.UI;

public class Grolfino : BossAI
{
    [Header("Spike Attack")]
    [SerializeField] private int numberOfSpikes;
    [SerializeField] private float timeBetweenSpikes;
    [SerializeField] private float spikeDamage;
    [SerializeField] private float angleBeteenSpikes;
    [SerializeField] private float distanceBetweenSpikes;
    private float spikeSpawnOffset = 17;
    [SerializeField] private GameObject spike;


    [Header("Projectile Attack")]
    [SerializeField] private float timeBetweenProjectiles;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnPos;


    [Header("Movement")]
    [SerializeField, Tooltip("Time after unburrowing before attacking")] private float timeBeforeAttack;
    [SerializeField, Tooltip("Amount of time before unburrowing")] private float amountOfTimeBurrowed;
    [SerializeField, Tooltip("How far away from the player the boss unburrows")] private float teleportRange = 30f;
    [SerializeField, Tooltip("How long after an attack before the boss burrows again")] private float burrowDelay;


    private bool halfHealthThresholdReached = false;
    [SerializeField] private GameObject bossMesh;
    [SerializeField] private List<string> bossAttack;

    public bool burrowed = false;
    public bool slamAttack;
    public bool projectileAttack;
    public bool sweepAttack;

    // Gabe was here and evilly added this to make my life easier.
    public static Action BossDead;

    private FMOD.Studio.EventInstance bossBurrow;

    private void Start()
    {
        bossAttack.Add("ProjectileAttack");
        bossAttack.Add("SweepAttack");

        animator = GetComponentInChildren<Animator>();
        agent.isStopped = true;
        transform.LookAt(player.transform.position);
    }
    private void Update()
    {
        //This always makes the boss look at the player, can be disabled
        agent.destination = player.transform.position;

        //When boss reaches 50% health, attacks faster and more frequently
        // if (currentHealth < health / 2 && halfHealthThresholdReached == false)
        // {
        //     halfHealthThresholdReached = true;
        //     timeBetweenProjectiles /= 1.5f;
        //     projectileSpeed *= 2f;
        //     timeBetweenSpikes /= 1.5f;
        // }
    }

    private IEnumerator StartSweepAttack()
    {
        //Sweep attack animation goes here
        animator.SetBool("Sweep", true);
        //Makes collider active and sets the starting rotation
        yield return new WaitUntil(() => sweepAttack == true);
        
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCentipedeSwipeAttack,transform.position);

        //Makes the collider inactive
        animator.SetBool("Sweep", false);
        sweepAttack = false;
        yield return new WaitForSeconds(burrowDelay);
        StartCoroutine(Burrow());
        yield return null;
    }

    private IEnumerator StartSpikeAttack(int numberOfSpikes, float timeBetweenSpikes, float distanceBetweenSpikes)
    {
        //Slam attack animation goes here
        animator.SetBool("GroundSlam", true);
        //Calculates forward direction for attacks
        yield return new WaitUntil(() => slamAttack == true);
        Vector3 spawnPos = transform.position + (transform.forward * spikeSpawnOffset);
        Vector3 dir = Quaternion.AngleAxis(0, Vector3.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(-angleBeteenSpikes, Vector3.up) * transform.forward;
        Vector3 dir3 = Quaternion.AngleAxis(-angleBeteenSpikes * 2, Vector3.up) * transform.forward;
        Vector3 dir4 = Quaternion.AngleAxis(angleBeteenSpikes, Vector3.up) * transform.forward;
        Vector3 dir5 = Quaternion.AngleAxis(angleBeteenSpikes * 2, Vector3.up) * transform.forward;

        AudioManager.PlaySound3D(AudioEvents.Instance.OnCentipedeStompScreamAttack,transform.position);

        //Instantiates spikes in a straight line with slightly random rotation
        for (int i = 0; i < numberOfSpikes; i++)
        {
            GameObject s = Instantiate(spike, spawnPos + (dir * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s2 = Instantiate(spike, spawnPos + (dir2 * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s3 = Instantiate(spike, spawnPos + (dir3 * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s4 = Instantiate(spike, spawnPos + (dir4 * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s5 = Instantiate(spike, spawnPos + (dir5 * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));

            Vector3 newScale = new Vector3(1 + (i * 0.1f), 1 + (i * 0.1f), 1 + (i * 0.1f));
            s.transform.localScale = newScale;
            s2.transform.localScale = newScale;
            s3.transform.localScale = newScale;
            s4.transform.localScale = newScale;
            s5.transform.localScale = newScale;

            s.GetComponent<Spike>().spikeDamage = spikeDamage;
            s2.GetComponent<Spike>().spikeDamage = spikeDamage;
            s3.GetComponent<Spike>().spikeDamage = spikeDamage;
            s4.GetComponent<Spike>().spikeDamage = spikeDamage;
            s5.GetComponent<Spike>().spikeDamage = spikeDamage;

            yield return new WaitForSeconds(timeBetweenSpikes);
        }
        animator.SetBool("GroundSlam", false);
        slamAttack = false;
        yield return new WaitForSeconds(burrowDelay);
        StartCoroutine(Burrow());
        yield return null;
    }

    private IEnumerator StartRangedAttack(float timeBetweenProjectiles)
    {
        //Projectile attack animation goes here
        animator.SetInteger("ProjectileVariation", Random.Range(1, 3));
        animator.SetBool("Projectile", true);
        yield return new WaitUntil(() => projectileAttack == true);
        //Creatures projectiles in an arc in front of the boss
        //All projectiles are instantiated rotated away from the boss
        //Projectiles are automatically destroyed after 2.5s

        AudioManager.PlaySound3D(AudioEvents.Instance.OnCentipedeSpreadAttack,transform.position);   

        while (projectileAttack == true)
        {
            Vector3 spawnPos = new Vector3(projectileSpawnPos.position.x, 0, projectileSpawnPos.position.z);
            Vector3 bossPos = new Vector3(transform.position.x, 0, transform.position.z);
            Quaternion spawnRotation = Quaternion.LookRotation(spawnPos - (bossPos + transform.forward * 12));
            GameObject s = Instantiate(projectile, spawnPos + (Vector3.up * 2), spawnRotation);
            s.GetComponent<GrolfinoProjectile>().projectileDamage = projectileDamage;
            s.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, projectileSpeed));
            Destroy(s, 2.5f);
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
        animator.SetBool("Projectile", false);
        yield return new WaitForSeconds(burrowDelay);
        StartCoroutine(Burrow());
        yield return null;
    }

    public IEnumerator Burrow()
    {
        //Burrow animation goes here
        animator.SetInteger("BurrowVariation", Random.Range(1, 3));
        animator.SetBool("Burrow", true);
        bossBurrow = AudioManager.Instance.CreateEventInstance(AudioEvents.Instance.OnCentipedeBurrow);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(bossBurrow, transform);
        bossBurrow.start();
        Vector3 targetPos;
        yield return new WaitUntil(() => burrowed == true);

        //Teleports to the targeted position and enables renderer and collider
        if (RandomPoint(player.transform.position, teleportRange, out targetPos))
        {
            animator.SetInteger("BurrowVariation", Random.Range(1, 3));
            yield return new WaitForSeconds(amountOfTimeBurrowed);
            //Unburrow animation goes here
            animator.SetBool("Burrow", false);
            animator.SetBool("Idle", true);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            yield return new WaitUntil(() => burrowed == false);
            bossBurrow.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            bossBurrow.release();
            AudioManager.PlaySound3D(AudioEvents.Instance.OnCrocResurface,transform.position); 
            yield return new WaitForSeconds(timeBeforeAttack);
            //Randomly picks an attack to perform
            //Once an attack is performed, it is removed from the list
            //When every attack has been performed, the list is refilled
            animator.SetBool("Idle", false);
            if (bossAttack.Count == 0)
            {
                StartCoroutine(StartSpikeAttack(numberOfSpikes, timeBetweenSpikes, distanceBetweenSpikes));
                bossAttack.Add("ProjectileAttack");
                bossAttack.Add("SweepAttack");
            }
            else
            {
                int r = Random.Range(0, bossAttack.Count);
                if (bossAttack[r] == "ProjectileAttack")
                {
                    StartCoroutine(StartRangedAttack(timeBetweenProjectiles));
                    bossAttack.Remove("ProjectileAttack");
                }
                else if (bossAttack[r] == "SweepAttack")
                {
                    StartCoroutine(StartSweepAttack());
                    bossAttack.Remove("SweepAttack");
                }
            }
            yield return null;
        }
        bossBurrow.release();
        yield return null;
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        //Picks a random point on a unitsphere in a radius around the boss
        //Runs SamplePosition to check if that point is valid
        //If it is valid, returns the position
        //If it is invalid, it runs again
        //Currently runs 30 times, technically there's a chance it doesn't find a valid point in 30 tries
        //If it doesn't, it currently sets the position to it's current location
        for (int i = 0; i < 200; i++)
        {
            Vector2 circlePoint = Random.insideUnitCircle.normalized * range;
            Vector3 randomPoint = center + new Vector3(circlePoint.x, 0, circlePoint.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = transform.position;
        return false;
    }

    protected override void Die()
    {
        animator.Play("Death");
        AudioManager.PlaySound3D(AudioEvents.Instance.OnCentipedeDefeated,transform.position); 
        bossBurrow.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bossBurrow.release(); 
        agent.isStopped = true;
        alive = false;
        StopAllCoroutines();
        Destroy(this.gameObject, 3f);
        CreatureManager.AnyCreatureDied?.Invoke();

        // Another addition to my list of crimes -Gabe
        BossDead?.Invoke();
    }
}