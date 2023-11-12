using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackRange : MonoBehaviour
{
    //[SerializeField] private ParticleSystem vfx;
    private Arm arm;
    private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Creature creature))
        {
            creature.TakeDamage((int)arm.AttackDamage);
            creature.Knockback(creature.transform.position - transform.position, 4, 0.05f);
        }
    }

    public void AnimateAttackRange()
    {
        if (anim != null) anim.speed = 1;
    }

    private void DisableAttackRange()
    {

        gameObject.SetActive(false);
    }

    public void InputArmReference(Arm controllingArm)
    {
        arm = controllingArm;
    }

    void Awake()
    {
        if (gameObject.TryGetComponent(out Animator anim))
        {
            anim.keepAnimatorStateOnDisable = false;
            this.anim = anim;
        }
    }
}
