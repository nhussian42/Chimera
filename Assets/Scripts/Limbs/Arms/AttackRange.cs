using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class AttackRange : MonoBehaviour
{
    //[SerializeField] private ParticleSystem vfx;
    public static Action AttackEnded;
    private Arm arm;
    private Quaternion startRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Creature creature))
        {
            AudioManager.PlaySound3D(AudioEvents.Instance.OnPlayerHitConnected, other.transform.position);
            creature.TakeDamage((int)arm.AttackDamage);
            creature.Knockback(creature.transform.position - transform.position, 20, 0.05f);
            
            // Temp stun code, refactor later
            if (PlayerController.Instance.currentHead.TryGetComponent<RhinoHead>(out RhinoHead rhinoHead) 
                && creature.TryGetComponent<NotBossAI>(out NotBossAI creatureAI))
                if (creature.CurrentHealth > 0)
                    creatureAI.Stun(0.75f, rhinoHead.VFXPrefab);
        }

        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<BreakablePot>().SpawnBones();
            other.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    private void OnDisable()
    {
        AttackEnded?.Invoke();
    }

    private void DisableAttackRange()
    {
        AttackEnded?.Invoke();
        //transform.GetChild(0).rotation = startRotation;
        Invoke(nameof(ReleaseRange), 2f);
    }

    private void ReleaseRange()
    {
        arm.AttackRangePool.Release(this);
    }

    public void InputArmReference(Arm controllingArm, SideOfPlayer side, float angle)
    {
        arm = controllingArm;
        //startRotation = transform.GetChild(0).localRotation;

        // spaghetti code for getting the gecko/wolf claws to rotate
        if (side == SideOfPlayer.Right)
            angle = -angle;

        if (controllingArm.Weight == Weight.Light)
            transform.GetChild(0).localEulerAngles = new Vector3(angle, -90, 0);
    }

    public void InputArmReference(Arm controllingArm)
    {
        arm = controllingArm;
    }
}
