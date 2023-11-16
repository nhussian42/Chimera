using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Arm : Limb
{
    //Defaults
    [SerializeField] private float defaultAttackDamage;
    [SerializeField] private float defaultAttackSpeed;
    [SerializeField] private SideOfPlayer side;
    [SerializeField] private AttackRange attackRangePrefab;

    //Exposed properties
    private float attackDamage;
    private float attackSpeed;

    private static GameObject attackRangeParent;
    
    //Hidden properties

   // private bool canAttack = true;


    //Public getters
    public SideOfPlayer Side { get { return side; } }
    public float AttackDamage { get { return attackDamage; } }
    
    public float AttackSpeed { get { return attackSpeed; }  }

    //public bool CanAttack { get { return canAttack; } set { canAttack = value; }}
    public float DefaultAttackDamage { get { return defaultAttackDamage; } }
    public float DefaultAttackSpeed { get { return defaultAttackSpeed; } }
    

    // Public variables
    public ObjectPool<AttackRange> AttackRangePool { get; private set; }
    

    // Called by Player Controller on animation event
    public virtual void Attack()
    {
        AttackRange attackRange = AttackRangePool.Get();
        

        // May possibly need to set an attack range origin for every single arm instead
        if (Side == SideOfPlayer.Left)
        {
            Transform origin = PlayerController.Instance.AttackRangeLeftOrigin;
            attackRange.transform.SetPositionAndRotation(origin.position, Quaternion.Euler(0, PlayerController.Instance.transform.eulerAngles.y, 0));
        }
        else if (Side == SideOfPlayer.Right)
        {
            Transform origin = PlayerController.Instance.AttackRangeRightOrigin;
            attackRange.transform.SetPositionAndRotation(origin.position, Quaternion.Euler(0, PlayerController.Instance.transform.eulerAngles.y, 0));
        }
        
        //StartCoroutine(Cooldown());
        
        //StartCoroutine(ActivateAttackRange());
    }

    // Was used to prevent extra inputs after attacking, but does not seem to be needed anymore
    //public virtual void PauseInput()
    //{
    //    canAttack = false;
    //}

    // Must be called when swapping an arm, makes sure the arm has an attack collider that pops up when attacking
    public virtual void Initialize(PlayerController player)
    {
        // Create the attack range parent
        if (attackRangeParent == null)
        {
            attackRangeParent = Instantiate(new GameObject());
            attackRangeParent.name = "Attack Range Pool";
        }

        // Create an object pool so multiple attack ranges can be instantiated at once without instantiating attack ranges during attacks
        AttackRangePool = new ObjectPool<AttackRange>(CreateAttackRange,
        range => { range.gameObject.SetActive(true); },
        range => { range.gameObject.SetActive(false); },
        range => { Destroy(range.gameObject); },
        false,
        5,
        20
        );

        // Instantiates and sets the arm's attack collider on the player's attack origin
        // attackRange = Instantiate(attackRangePrefab.gameObject, player.attackRangeOrigin.position, player.attackRangeOrigin.rotation).GetComponent<AttackRange>();
        // attackRange.InputArmReference(this);
        // attackRange.gameObject.SetActive(false);
    }

    private AttackRange CreateAttackRange()
    {
        AttackRange attackRange = Instantiate(attackRangePrefab.gameObject, attackRangeParent.transform).GetComponent<AttackRange>();
        attackRange.InputArmReference(this);
        // attackRange.gameObject.SetActive(false);
        return attackRange;
    }

    // Must be called when swapping an arm, destroys the previously instantiated attack collider so a new one can be made
    public virtual void Terminate()
    {
        AttackRangePool.Dispose();
        // Destroys the instantiated attack collider
        // Destroy(attackRange.gameObject);
        
    }

    // Internal attack coroutine that is called by Attack()
    // private IEnumerator ActivateAttackRange()
    // {
    //     canAttack = false;
    //     attackRange.gameObject.SetActive(true);
    //     yield return new WaitForSeconds(5f);
    //     attackRange.gameObject.SetActive(false);
    //     StartCoroutine(Cooldown());
    // }

    // Called after attacking: OLD     - Attack Speed controlled by animation speed now
    // private IEnumerator Cooldown()
    // {
    //     canAttack = false;
    //     yield return new WaitForSeconds(attackSpeed);
    //     canAttack = true;
    // }

    // These functions are obsolete - Amon
    public void UpdateAttackDamage(float amount) => attackDamage += amount;

    public void UpdateAttackSpeed(float amount) => attackSpeed -= amount;

    public void UpdateMaxHealth(float amount) => maxHealth += amount;

    // I think this function is obsolete? - Amon
    public void UpdateCurrentHealth(float amount)
    {
        if ((currentHealth + amount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else currentHealth += amount;
    }
    
    // Called by PlayerController after equipping new trinket buff, loading new scene, etc.
    public void LoadStats(float atkDmg, float atkSpd, float maxHP, float currentHP)
    {
        attackDamage = atkDmg;
        attackSpeed = atkSpd;
        maxHealth = maxHP;
        Health = currentHP;
        //Debug.Log(this.name + "'s current health: " + Health);
    }

    public override void LoadDefaultStats()
    {
        base.LoadDefaultStats();
        attackDamage = defaultAttackDamage;
        attackSpeed = defaultAttackSpeed;
    }

    public override void Disintegrate()
    {
        base.Disintegrate();
        PlayerController.Instance.RevertToDefault(this);
    }

    public void DebugLog()
    {
        Debug.Log("Arm Max HP: " + maxHealth.ToString());
    }
}
