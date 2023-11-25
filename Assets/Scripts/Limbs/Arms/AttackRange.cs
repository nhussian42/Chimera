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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Creature creature))
        {
            AudioManager.PlaySound3D(AudioEvents.Instance.OnPlayerHitConnected, other.transform.position);
            creature.TakeDamage((int)arm.AttackDamage);
            creature.Knockback(creature.transform.position - transform.position, 20, 0.05f);
        }
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    private void DisableAttackRange()
    {
        AttackEnded?.Invoke();
        arm.AttackRangePool.Release(this);
    }

    public void InputArmReference(Arm controllingArm)
    {
        arm = controllingArm;
    }
}
