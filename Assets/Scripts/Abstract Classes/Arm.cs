using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Arm : Limb
{
    //Exposed properties
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private SideOfPlayer side;
    //[SerializeField] private ArmDrop dropPrefab;
    [SerializeField] private AttackRange attackRangePrefab;

    //Hidden properties
    private AttackRange attackRange;
    private bool canAttack = true;

    //Public getters
    public SideOfPlayer Side { get { return side; } }
    //public ArmDrop DropPrefab { get { return dropPrefab; } }
    public float AttackDamage { get { return attackDamage; } }
    
    public float AttackSpeed { get { return attackSpeed; }  }

    public bool CanAttack { get { return canAttack; } }

    

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
        // Instantiates and sets the arm's attack collider on the player's attack origin
        attackRange = Instantiate(attackRangePrefab.gameObject, player.attackRangeOrigin.position, player.attackRangeOrigin.rotation, player.attackRangeOrigin).GetComponent<AttackRange>();
        attackRange.InputArmReference(this);
        attackRange.gameObject.SetActive(false);
        StartCoroutine(Cooldown());
        
    }

    public virtual void Terminate()
    {
        // Destroys the instantiated attack collider
        Destroy(attackRange.gameObject);
        
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

    public void LoadStats(float atkDmg, float atkSpd, float maxHP, float currentHP)
    {
        attackDamage = atkDmg;
        attackSpeed = atkSpd;
        maxHealth = maxHP;
        Health = currentHP;
    }


    public void DebugLog()
    {
        Debug.Log("Arm Max HP: " + maxHealth.ToString());
    }
}
