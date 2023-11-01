using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grolfino : BossAI
{
    [Header("Spike Attack")]
    [SerializeField] private int numberOfSpikes;
    [SerializeField] private float timeBetweenSpikes;
    [SerializeField] private GameObject spike;


    [SerializeField] private float burrowCooldown = 3f;
    private float currentBurrowCooldown = 3f;
    private MeshRenderer meshRenderer;
    [SerializeField] private float teleportRange = 30f;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
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
        Vector3 dir = Quaternion.AngleAxis(15, Vector3.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(-15, Vector3.up) * transform.forward;
        for (int i = 0; i < numberOfSpikes; i++)
        {
            GameObject s = Instantiate(spike, transform.position + (transform.forward * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s2 = Instantiate(spike, transform.position + (dir * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            GameObject s3 = Instantiate(spike, transform.position + (dir2 * i), Quaternion.Euler(Random.Range(0, 11), 0, Random.Range(0, 11)));
            yield return new WaitForSeconds(timeBetweenSpikes);
        }
        yield return null;
    }

    private IEnumerator Burrow()
    {
        Vector3 targetPos;
        meshRenderer.enabled = false;
        healthbar.enabled = false;
        yield return new WaitForSeconds(2f);

        if (RandomPoint(player.transform.position, teleportRange, out targetPos))
        {
            Debug.Log(targetPos);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            meshRenderer.enabled = true;
            healthbar.enabled = true;

            yield return new WaitForSeconds(1f);

            StartCoroutine(StartSpikeAttack(numberOfSpikes, timeBetweenSpikes));
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