using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutsceneTrigger : MonoBehaviour
{
    private static bool _triggered;

    // private Collider cutsceneTrigger;

    public static Action IntroWakeUp;

    [SerializeField]
    private float timeUntilPlayerMovementRestored;

    void Awake()
    {
        // cutsceneTrigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            IntroWakeUp?.Invoke();
            PlayerController.Instance.DisableAllDefaultControls();
            Invoke("ResumePlayerControls", timeUntilPlayerMovementRestored);
        }
    }

    private void ResumePlayerControls()
    {
        PlayerController.Instance.EnableAllDefaultControls();
    }
}
