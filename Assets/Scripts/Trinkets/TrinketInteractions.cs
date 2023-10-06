using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketInteractiosn : MonoBehaviour
{

    public GameObject TrinketOptionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        TrinketOptionsMenu.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //make it to where only the player can activate the Trinket Menu
        {
            Debug.Log("Collided with Player");
            TrinketOptionsMenu.SetActive(true);
            Destroy(gameObject);
        }
     
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
