using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHazard : MonoBehaviour
{
    private void Start()
    {
        hazardRef = GetComponentInChildren<HazardScript>();
    }
    [SerializeField] private float delay;
    private HazardScript hazardRef;
    //Activates trap after delay
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Detected player");
            hazardRef.Invoke("Activate", delay);
            //HazardScript.Instance.Invoke("Activate", delay);
        }
    }

    //OnStay and OnExit to detect if player sits on trap
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hazardRef.playerStay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hazardRef.playerStay = false;
        }
    }
}
