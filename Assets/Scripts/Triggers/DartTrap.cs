using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform startLocation;
    [SerializeField] public Quaternion DartRotation;

    [SerializeField] private float spawnDelay;

    [SerializeField] private DartTriggerScript DartTrigger;
    
    private float timeSinceSpawned;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceSpawned = 0.75f;
        DartTrigger = GetComponentInChildren<DartTriggerScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (DartTrigger.detectedPlayer)
        {
            timeSinceSpawned += Time.deltaTime;
            if (timeSinceSpawned > spawnDelay)
            {
                var dartToggle = projectile.GetComponent<ProjectileScript>();
                
                var dart = Instantiate(projectile, startLocation.position, DartRotation);
                dart.transform.rotation = DartRotation;
                timeSinceSpawned = 0;             
                
            }
        }

        
    }
}
