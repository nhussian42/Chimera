using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPopup : MonoBehaviour
{
    public GameObject PopupIndicator;

    void OnTriggerEnter(Collider ObjectEntering)
    {
        if(ObjectEntering.tag == "Player")
        {
            PopupIndicator.SetActive(true);
        }
    }
    void OnTriggerExit(Collider ObjectExiting)
    {
        if (ObjectExiting.tag == "Player")
        {
            PopupIndicator.SetActive(false);
        }
    }
}
