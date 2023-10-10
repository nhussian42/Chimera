using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Arm : Limb
{
    //Exposed properties
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private Weight weight;
    [SerializeField] private SideOfPlayer side;
    [SerializeField] private ArmDrop dropPrefab;
    [SerializeField] private AttackRange attackRangePrefab;

    //Hidden properties
    private AttackRange attackRange;
    private bool canAttack = true;
    private Animator anim;


    //Public getters
    public Classification Classification { get { return classification; } }
    public Weight Weight { get { return weight; } }
    public SideOfPlayer Side { get { return side; } }
    public ArmDrop DropPrefab { get { return dropPrefab; } }
    public float AttackDamage { get { return attackDamage; } }
    
    public float AttackSpeed { get { return attackSpeed; }  }

    

    public virtual void Attack()
    {
        if(canAttack == true)
        {
            //Debug.Log("Attack");
            StartCoroutine(ActivateAttackRange());
        }
    }

    public virtual void Initialize(PlayerController player)
    {
        attackRange = Instantiate(attackRangePrefab.gameObject, player.attackRangeOrigin.position, player.attackRangeOrigin.rotation, player.attackRangeOrigin).GetComponent<AttackRange>();
        attackRange.InputArmReference(this);
        attackRange.gameObject.SetActive(false);
        StartCoroutine(Cooldown());
        //Debug.Log("Arm initialized");
    }

    public virtual void Terminate()
    {
        Destroy(attackRange.gameObject);
        //Debug.Log("Arm terminated");
    }

    private IEnumerator ActivateAttackRange()
    {
        canAttack = false;
        attackRange.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        attackRange.gameObject.SetActive(false);
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    public void UpdateAttackDamage(float amount) => attackDamage += amount;

    public void UpdateAttackSpeed(float amount) => attackSpeed -= amount;

    public void UpdateMaxHealth(float amount) => maxHealth += amount;

    public void DebugLog()
    {
        Debug.Log("Arm Max HP: " + maxHealth.ToString());
    }
}
