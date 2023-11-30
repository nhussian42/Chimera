using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform startLocation;
    [SerializeField] private Quaternion startRotation;

    [SerializeField] private float spawnDelay;

    [SerializeField] private DartTriggerScript DartTrigger;
    
    private float timeSinceSpawned;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceSpawned = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {

        if (DartTrigger.detectedPlayer)
        {
            timeSinceSpawned += Time.deltaTime;
            if (timeSinceSpawned > spawnDelay)
            {
                Instantiate(projectile, startLocation.position, startRotation);
                timeSinceSpawned = 0;
            }
        }

        
    }
}
