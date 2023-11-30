using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHazard : MonoBehaviour
{
    [SerializeField] private float delay;
    //Activates trap after delay
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Detected player");
            HazardScript.Instance.Invoke("Activate", delay);
        }
    }

    //OnStay and OnExit to detect if player sits on trap
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HazardScript.Instance.playerStay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HazardScript.Instance.playerStay = false;
        }
    }
}
