using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1CutsceneTrigger : MonoBehaviour
{
    private bool _triggered;

    // private Collider cutsceneTrigger;

    public static Action Boss1Cutscene;

    void Awake()
    {
        // cutsceneTrigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            Boss1Cutscene?.Invoke();
        }
    }
}
