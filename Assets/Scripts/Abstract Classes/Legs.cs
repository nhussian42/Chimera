using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Legs : Limb
{
    [SerializeField] float movementSpeed;
    [SerializeField] float cooldownTime;

    public float MovementSpeed { get { return movementSpeed; } }

    // Refs
    private PlayerController player; // needed to manipulate player visibility and movement

    public abstract void ActivateAbility();

    protected virtual IEnumerator Cooldown(bool conditional)
    {
        conditional = false;
        yield return new WaitForSeconds(cooldownTime);
        conditional = true;
    }

    public virtual void LoadStats(float health, float moveSpd)
    {
        Health = health;
        movementSpeed = moveSpd;
    }


}
