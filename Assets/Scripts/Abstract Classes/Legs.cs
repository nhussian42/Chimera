using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Legs : Limb
{
    //Defaults
    [SerializeField] float defaultMovementSpeed;
    [SerializeField] float defaultCooldownTime;

    // Use these when programming behavior NOT the default values
    float movementSpeed;
    float cooldownTime;
    protected bool canActivate = true;

    public bool CanActivate { get { return canActivate; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public float CooldownTime { get { return cooldownTime; } }
    public float DefaultMovementSpeed { get { return defaultMovementSpeed; } }
    public float DefaultCooldownTime { get { return defaultCooldownTime; } }

    // Refs
    protected PlayerController player; // needed to manipulate player visibility and movement

    protected virtual void Start()
    {
        player = PlayerController.Instance;
    }

    public abstract void PlayAnim();

    // Called by PlayerController on input trigger
    public abstract void ActivateAbility();

    // Cooldown called after activating ability
    protected virtual IEnumerator Cooldown()
    {
        canActivate= false;
        yield return new WaitForSeconds(cooldownTime);
        canActivate = true;
        //Debug.Log("canActivate = true");
    }

    // Called to update stats after applying a trinket buff, loading a new scene, etc.
    public virtual void LoadStats(float moveSpd, float cldwn, float maxHP, float currentHP)
    {
        maxHealth = maxHP;
        Health = currentHP;
        movementSpeed = moveSpd;
        cooldownTime = cldwn;
    }

    public override void LoadDefaultStats()
    {
        base.LoadDefaultStats();
        movementSpeed = defaultMovementSpeed;
        cooldownTime = defaultCooldownTime;
    }

    public override void Disintegrate()
    {
        base.Disintegrate();
        PlayerController.Instance.RevertToDefault(this);
    }

}
