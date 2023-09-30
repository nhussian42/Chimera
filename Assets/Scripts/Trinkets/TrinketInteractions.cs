using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketInteractiosn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider");
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
