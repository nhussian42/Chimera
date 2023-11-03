using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackRange : MonoBehaviour
{
    //[SerializeField] private ParticleSystem vfx;
    private Arm arm;

    private void OnEnable()
    {
        //vfx.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Creature>(out Creature creature) == true)
        {
            creature.TakeDamage((int)arm.AttackDamage);
            creature.Knockback(creature.transform.position - transform.position, 8, 0.05f);
        }
    }

    public void InputArmReference(Arm controllingArm)
    {
        arm = controllingArm;
    }

    private void OnDisable()
    {
        //vfx.Stop();
    }
}
