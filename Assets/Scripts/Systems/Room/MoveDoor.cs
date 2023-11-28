using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorOpenCondition
{
    AllCreaturesDefeated,
    WalkedUpTo,
    AllCreaturesDefeatedAndWalkedUpTo,

    AtStart
}

public class MoveDoor : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private DoorOpenCondition doorOpenCondition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        switch (doorOpenCondition)
        {
            case DoorOpenCondition.AllCreaturesDefeated:
                FloorManager.AllCreaturesDefeated += TriggerDoorOpen;
                break;
            case DoorOpenCondition.WalkedUpTo:
                EnableWalkUpTrigger();
                break;
            case DoorOpenCondition.AllCreaturesDefeatedAndWalkedUpTo:
                FloorManager.AllCreaturesDefeated += EnableWalkUpTrigger;
                break;
            case DoorOpenCondition.AtStart:
                TriggerDoorOpen();
                break;
        }
        
    }

    private void OnDisable()
    {
        switch (doorOpenCondition)
        {
            case DoorOpenCondition.AllCreaturesDefeated:
                FloorManager.AllCreaturesDefeated -= TriggerDoorOpen;
                break;
            case DoorOpenCondition.WalkedUpTo:
                //EnableWalkUpTrigger();
                break;
            case DoorOpenCondition.AllCreaturesDefeatedAndWalkedUpTo:
                FloorManager.AllCreaturesDefeated -= EnableWalkUpTrigger;
                break;
            case DoorOpenCondition.AtStart:
                //TriggerDoorOpen();
                break;
        }
    }

    public void TriggerDoorOpen()
    {
        anim.SetTrigger("Open");
        AudioManager.PlaySound3D(AudioEvents.Instance.OnDoorOpened, transform.position);
    }

    public void TriggerDoorClose()
    {
        anim.SetTrigger("Close");
    }

    private void EnableWalkUpTrigger()
    {
        transform.parent.Find("WalkUpTrigger").GetComponent<BoxCollider>().enabled = true;
    }
}
