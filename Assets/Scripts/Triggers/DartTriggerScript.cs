using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTriggerScript : MonoBehaviour
{
    private string targetTag = "Player";
    public bool detectedPlayer;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == targetTag)
        {
            detectedPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == targetTag)
        {
            detectedPlayer = false;
        }
    }
}
