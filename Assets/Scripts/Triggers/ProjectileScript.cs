using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [SerializeField] private float movespeed;
    [SerializeField] private float timeAlive;
    private float timeSinceSpawned = 0f;

    [SerializeField] float damage;

    // Update is called once per frame
    void Update()
    {
        transform.position += movespeed * transform.forward * Time.deltaTime;

        timeSinceSpawned += Time.deltaTime;

        if (timeSinceSpawned > timeAlive)
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Hit Player");
            PlayerController.Instance.DistributeDamage(damage);
            Destroy(gameObject);
        }
    }
}
