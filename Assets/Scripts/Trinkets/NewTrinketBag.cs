using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewTrinketBag : MonoBehaviour
{
    private NewTrinketManager trinketManager;


    private void Start()
    {
        trinketManager = NewTrinketManager.Instance;
        Debug.Log(trinketManager);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            trinketManager.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
