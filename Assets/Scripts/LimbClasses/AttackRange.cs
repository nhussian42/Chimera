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
        if(other.TryGetComponent<CreatureWithDrop>(out CreatureWithDrop creature) == true)
        {
            creature.TakeDamage((int)arm.attackDamage);
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
