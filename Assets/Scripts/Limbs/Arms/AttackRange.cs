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
            creature.TakeDamage((int)arm.AttackDamage);
            creature.Knockback(creature.transform.position - transform.position, 4, 0.05f);
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
