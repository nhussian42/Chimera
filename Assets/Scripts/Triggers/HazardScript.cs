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

    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    private bool activated;
    private bool deactivate;
    public bool playerStay;
    

    public static HazardScript Instance;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = startPos;
        targetPos = endPos;
        transform.position = currentPos.position;
        Instance = this;

    }


    void Update()
    {
        //activate/deactivate sends trap to point locations
        if (activated)
        {
            if (transform.position != targetPos.position)
            {
                sinTime += Time.deltaTime * moveSpeed;
                sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
                float       t = evaluate(sinTime);
                transform.position = Vector3.Lerp(currentPos.position, targetPos.position, t);
            }
            //Resets trap after 1 second
            if (transform.position == targetPos.position)
            {
                activated = false;
                Invoke("Deactivate", 1);
            }
        }
        

        if (deactivate)
        {
            if (transform.position != targetPos.position)
            {
                sinTime += Time.deltaTime * moveSpeed;
                sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
                float t = evaluate(sinTime);
                transform.position = Vector3.Lerp(currentPos.position, targetPos.position, t);
            }

            if (transform.position == targetPos.position)
            {
                //Prestigeous "GTFO" Mechanic
                deactivate = false;
                if (playerStay)
                {
                    Activate();
                }
            }
        }
        
        //swaps destination points
        if(transform.position == targetPos.position)
        {
            swap();         
        }
       
    }

    //Damages players
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.DistributeDamage(damage);
        }

    }
    
    private float evaluate(float x)
        {
            return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
        }

    private void swap()
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

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        deactivate = true;
    }
}
