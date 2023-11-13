using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Grolfino : BossAI
{
    [Header("Spike Attack")]
    [SerializeField] private int numberOfSpikes;
    [SerializeField] private float timeBetweenSpikes;
    [SerializeField] private float spikeDamage;
    [SerializeField] private float distanceBetweenSpikes;
    [SerializeField] private GameObject spike;


    [Header("Projectile Attack")]
    [SerializeField] private int numberOfProjectiles;
    [SerializeField] private float timeBetweenProjectiles;
    [SerializeField] private float projectileDamage;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float angleBetweenProjectiles;
    [SerializeField] private float projectileSpeed;


    [Header("Sweep Attack")]
    [SerializeField] private int sweepAngle;
    [SerializeField] private float sweepDuration;
    [SerializeField] private GameObject sweepAttackCollider;


    [Header("Movement")]
    [SerializeField] private float burrowCooldown = 3f;
    [SerializeField] private float teleportRange = 30f;

    private float currentBurrowCooldown = 0f;
    [SerializeField] private GameObject bossMesh;
    private BoxCollider boxCollider;
    private List<string> bossAttack;


    private void Start()
    {
        bossAttack.Add("SpikeAttack");
        bossAttack.Add("ProjectileAttack");
        bossAttack.Add("SweepAttack");

        boxCollider = GetComponent<BoxCollider>();
        agent.isStopped = true;
    }
    private void Update()
    {
        //This always makes the boss look at the player, can be disabled
        agent.destination = player.transform.position;

        //Keeps track of when the boss burrows
        currentBurrowCooldown -= Time.deltaTime;
        if (currentBurrowCooldown < 0)
        {
            StartCoroutine(Burrow());
            currentBurrowCooldown = burrowCooldown;
        }
    }

    private IEnumerator StartSweepAttack(int angle, float duration)
    {
        //Makes collider active and sets the starting rotation
        sweepAttackCollider.SetActive(true);
        UnityEditor.TransformUtils.SetInspectorRotation(sweepAttackCollider.transform, new Vector3(0, -angle / 2, 0));

        //Lerps the angle and sets the rotation
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float rotatedAngle = Mathf.Lerp(-angle / 2, angle / 2, timer / duration);
            UnityEditor.TransformUtils.SetInspectorRotation(sweepAttackCollider.transform, new Vector3(0, rotatedAngle, 0));
            yield return null;
        }

        //Makes the collider inactive
        sweepAttackCollider.SetActive(false);
        yield return null;
    }

    private IEnumerator StartSpikeAttack(int numberOfSpikes, float timeBetweenSpikes, float distanceBetweenSpikes)
    {
        //Calculates forward direction for attacks
        Vector3 dir = Quaternion.AngleAxis(0, Vector3.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(15, Vector3.up) * transform.forward;
        Vector3 dir3 = Quaternion.AngleAxis(-15, Vector3.up) * transform.forward;

        //Instantiates spikes in a straight line with slightly random rotation
        for (int i = 0; i < numberOfSpikes; i++)
        {
            GameObject s = Instantiate(spike, transform.position + (dir * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s2 = Instantiate(spike, transform.position + (dir2 * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s3 = Instantiate(spike, transform.position + (dir3 * distanceBetweenSpikes * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));

            Vector3 newScale = new Vector3(1 + (i * 0.1f), 1 + (i * 0.1f), 1 + (i * 0.1f));
            s.transform.localScale = newScale;
            s2.transform.localScale = newScale;
            s3.transform.localScale = newScale;

            s.GetComponent<Spike>().spikeDamage = spikeDamage;
            s2.GetComponent<Spike>().spikeDamage = spikeDamage;
            s3.GetComponent<Spike>().spikeDamage = spikeDamage;

            yield return new WaitForSeconds(timeBetweenSpikes);
        }
        yield return null;
    }

    private IEnumerator StartRangedAttack(int numberOfProjectiles, float timeBetweenProjectiles)
    {
        //Creatures projectiles in an arc in front of the boss
        //All projectiles are instantiated rotated away from the boss
        //Projectiles are automatically destroyed after 2.5s   
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float yRot = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y;
            float maximumNegativeAngle = (numberOfProjectiles - 1) / 2 * angleBetweenProjectiles;
            Vector3 angle = Quaternion.Euler(0, maximumNegativeAngle + (angleBetweenProjectiles * i), 0) * transform.forward;
            GameObject s = Instantiate(projectile, transform.position + angle, Quaternion.Euler(0, yRot - maximumNegativeAngle + (angleBetweenProjectiles * i), 0));
            s.GetComponent<GrolfinoProjectile>().projectileDamage = projectileDamage;
            s.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, projectileSpeed));
            Destroy(s, 2.5f);
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
        yield return null;
    }

    private IEnumerator Burrow()
    {
        //Disables mesh renderer and collider
        Vector3 targetPos;
        //meshRenderer.enabled = false;
        boxCollider.enabled = false;
        bossMesh.SetActive(false);
        yield return new WaitForSeconds(1f);

        //Teleports to the targeted position and enables renderer and collider
        if (RandomPoint(player.transform.position, teleportRange, out targetPos))
        {
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            //meshRenderer.enabled = true;
            boxCollider.enabled = true;
            bossMesh.SetActive(true);

            float timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime;
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                yield return null;
            }

            //Randomly picks an attack to perform
            //Once an attack is performed, it is removed from the list
            //When every attack has been performed, the list is refilled
            int r = Random.Range(0, bossAttack.Count);
            if (bossAttack[r] == "ProjectileAttack")
            {
                StartCoroutine(StartRangedAttack(numberOfProjectiles, timeBetweenProjectiles));
                bossAttack.Remove("ProjectileAttack");
            }
            else if (bossAttack[r] == "SpikeAttack")
            {
                StartCoroutine(StartSpikeAttack(numberOfSpikes, timeBetweenSpikes, distanceBetweenSpikes));
                bossAttack.Remove("SpikeAttack");
            }
            else if (bossAttack[r] == "SweepAttack")
            {
                StartCoroutine(StartSweepAttack(sweepAngle, sweepDuration));
                bossAttack.Remove("SweepAttack");
            }

            if (bossAttack.Count == 0)
            {
                bossAttack.Add("SpikeAttack");
                bossAttack.Add("ProjectileAttack");
                bossAttack.Add("SweepAttack");
            }
            yield return null;
        }
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
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.onUnitSphere * range;
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
}