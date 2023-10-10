using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketInteractiosn : MonoBehaviour
{

    public GameObject TrinketOptionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
        //TrinketOptionsMenu.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("TrinketPickedUp");
            Instantiate(TrinketOptionsMenu, new Vector3(0,0,0), Quaternion.identity);
            TrinketOptionsMenu.SetActive(true);
            Destroy(gameObject);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
