using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackRange : MonoBehaviour
{
    //[SerializeField] private ParticleSystem vfx;
    private Arm arm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Creature creature))
        {
            creature.TakeDamage((int)arm.AttackDamage);
            creature.Knockback(creature.transform.position - transform.position, 4, 0.05f);
        }
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    private void DisableAttackRange()
    {
        print("disable");
        arm.AttackRangePool.Release(this);
    }

    public void InputArmReference(Arm controllingArm)
    {
        arm = controllingArm;
    }
}
