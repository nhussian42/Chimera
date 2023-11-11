using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Grolfino : BossAI
{
    [Header("Spike Attack")]
    [SerializeField] private int numberOfSpikes;
    [SerializeField] private float timeBetweenSpikes;
    [SerializeField] private float spikeDamage;
    [SerializeField] private GameObject spike;

    [Header("Ranged Attack")]
    [SerializeField] private int numberOfProjectiles;
    [SerializeField] private float timeBetweenProjectiles;
    [SerializeField] private float projectileDamage;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float angleBetweenProjectiles;
    [SerializeField] private float projectileSpeed;


    [SerializeField] private float burrowCooldown = 3f;
    private float currentBurrowCooldown = 0f;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    [SerializeField] private float teleportRange = 30f;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        agent.destination = player.transform.position;
        agent.updatePosition = false;
        currentBurrowCooldown -= Time.deltaTime;
        if (currentBurrowCooldown < 0)
        {
            StartCoroutine(Burrow());
            currentBurrowCooldown = burrowCooldown;
        }
    }

    private IEnumerator StartSpikeAttack(int numberOfSpikes, float timeBetweenSpikes)
    {
        Vector3 dir = Quaternion.AngleAxis(0, Vector3.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(15, Vector3.up) * transform.forward;
        Vector3 dir3 = Quaternion.AngleAxis(-15, Vector3.up) * transform.forward;
        for (int i = 0; i < numberOfSpikes; i++)
        {
            GameObject s = Instantiate(spike, transform.position + (dir * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s2 = Instantiate(spike, transform.position + (dir2 * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s3 = Instantiate(spike, transform.position + (dir3 * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            // s.transform.localScale = s.transform.localScale * i / 5;
            // s2.transform.localScale = s2.transform.localScale * i / 5;
            // s3.transform.localScale = s3.transform.localScale * i / 5;
            s.GetComponent<Spike>().spikeDamage = spikeDamage;
            s2.GetComponent<Spike>().spikeDamage = spikeDamage;
            s3.GetComponent<Spike>().spikeDamage = spikeDamage;
            yield return new WaitForSeconds(timeBetweenSpikes);
        }
        yield return null;
    }

    private IEnumerator StartRangedAttack(int numberOfProjectiles, float timeBetweenProjectiles)
    {
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
        Vector3 targetPos;
        meshRenderer.enabled = false;
        healthbar.enabled = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(2f);

        if (RandomPoint(player.transform.position, teleportRange, out targetPos))
        {
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            meshRenderer.enabled = true;
            healthbar.enabled = true;
            boxCollider.enabled = true;

            yield return new WaitForSeconds(1f);


            if (Random.Range(0, 2) == 0)
            {
                StartCoroutine(StartRangedAttack(numberOfProjectiles, timeBetweenProjectiles));
            }
            else
            {
                StartCoroutine(StartSpikeAttack(numberOfSpikes, timeBetweenSpikes));
            }

            yield return null;
        }
        yield return null;
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
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
        result = Vector3.zero;
        return false;
    }
}