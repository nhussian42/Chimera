using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 startPos;
    public Vector3 endPos;
    private Vector3 targetPos;

    public float speed;

    public PlayerController controller;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        targetPos = endPos;
        ToggleActive();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); 
        if (transform.position == targetPos)
        {
            ToggleActive();
        }
    }

    private void ToggleActive()
    {   
        if(transform.position == endPos)
        {
            targetPos = startPos;
        }
        //Make hazard go in between two points indefinitely
        if(transform.position == startPos)
        {
            targetPos = endPos;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.health -= 5;
            Debug.Log("damage done");
            //Invoke("OnTriggerEnter", 1);
            Debug.Log("delay done");
            //Adds invincibility delay, probably better way to do this but should work
        }

    }
}
