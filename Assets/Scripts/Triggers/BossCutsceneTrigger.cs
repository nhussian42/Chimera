using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutsceneTrigger : MonoBehaviour
{
    private bool _triggered;

    // private Collider cutsceneTrigger;

    public static Action BossCutscene;

    void Awake()
    {
        // cutsceneTrigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            BossCutscene?.Invoke();
        }
    }
}
