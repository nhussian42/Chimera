using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [SerializeField] private float movespeed;
    [SerializeField] private float timeAlive;
    //[SerializeField] private GameObject self;
    private float timeSinceSpawned = 0f;
    private GameObject self;
    private Quaternion startRotation;

    [SerializeField] float damage;

    private void OnEnable()
    {
        self = this.gameObject;       
    }
    // Update is called once per frame
    void Update()
    {
        
        //transform.position += movespeed * transform.forward * Time.deltaTime;
        transform.Translate((transform.forward*movespeed) * Time.deltaTime);
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
            Destroy(self);
        }
    }
}
