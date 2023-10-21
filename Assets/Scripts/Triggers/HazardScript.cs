using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class HazardScript : MonoBehaviour
{

    public Transform startPos;
    public Transform endPos;
    private Transform currentPos;
    private Transform targetPos;
    private float sinTime;

    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        currentPos = startPos;
        targetPos = endPos;
        transform.position = currentPos.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position != targetPos.position)
        {
            sinTime += Time.deltaTime * moveSpeed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            float t = evaluate(sinTime);
            transform.position = Vector3.Lerp(currentPos.position, targetPos.position, t);
        }

        swap();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.DistributeDamage(10);
        }

    }
    
    public float evaluate(float x)
        {
            return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
        }

    public void swap()
    {
        if (transform.position != targetPos.position)
        {
            return;
        }

        Transform t = currentPos;
        currentPos = targetPos;
        targetPos = t;
        sinTime = 0;
    }
}
