using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Arm : Limb
{
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private SideOfPlayer side;

    private Animator anim;

    public abstract void Attack();
}
