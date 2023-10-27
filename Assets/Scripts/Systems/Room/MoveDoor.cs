using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        FloorManager.AllCreaturesDefeated += TriggerDoorOpen;
    }

    private void OnDisable()
    {
        FloorManager.AllCreaturesDefeated -= TriggerDoorOpen;
    }

    private void TriggerDoorOpen()
    {
        anim.SetTrigger("Open");
    }

    private void TriggerDoorClose()
    {
        anim.SetTrigger("Close");
    }
}
