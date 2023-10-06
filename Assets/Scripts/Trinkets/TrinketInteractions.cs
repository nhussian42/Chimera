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

    //Makes it to where if only the player collides with the trinket it will pop up the menu
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collider");
            TrinketOptionsMenu.SetActive(true);
            Destroy(gameObject);
        }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
