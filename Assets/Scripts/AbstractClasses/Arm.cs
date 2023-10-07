using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Arm : Limb
{
    [SerializeField] private float AttackDamage;
    public float attackDamage { get { return AttackDamage; } }
    [SerializeField] private float attackSpeed;
    [SerializeField] private AttackRange attackRangePrefab;
    private AttackRange attackRange;
    private bool canAttack = true;
    //[SerializeField] private SideOfPlayer side;      Why do we need this?

    private Animator anim;

    public virtual void Attack()
    {
        if(canAttack == true)
        {
            StartCoroutine(ActivateAttackRange());
        }
    }

    public virtual void Initialize(PlayerController player)
    {
        attackRange = Instantiate(attackRangePrefab.gameObject, player.attackRangeOrigin.position, player.attackRangeOrigin.rotation, player.attackRangeOrigin).GetComponent<AttackRange>();
        attackRange.InputArmReference(this);
        attackRange.gameObject.SetActive(false);
    }

    public virtual void Terminate()
    {
        Destroy(attackRange.gameObject);
    }

    private IEnumerator ActivateAttackRange()
    {
        canAttack = false;
        attackRange.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        if(attackRange != null)
        {
            attackRange.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
}
