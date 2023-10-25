using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Legs : Limb
{
    [SerializeField] float movementSpeed;
    [SerializeField] float cooldownTime;
    protected bool canActivate = true;

    public bool CanActivate { get { return canActivate; } }
    public float MovementSpeed { get { return movementSpeed; } }

    public float CooldownTime { get { return cooldownTime; } }

    // Refs
    private PlayerController player; // needed to manipulate player visibility and movement

    public abstract void ActivateAbility();

    protected virtual IEnumerator Cooldown()
    {
        canActivate= false;
        yield return new WaitForSeconds(cooldownTime);
        canActivate = true;
    }

    public virtual void LoadStats(float health, float moveSpd)
    {
        Health = health;
        movementSpeed = moveSpd;
    }


}
