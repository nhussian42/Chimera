using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseAnimationEvents : MonoBehaviour
{
    [SerializeField] private BoxCollider tortoiseAttackCollider;

    public void EnableTortoiseAttackCollider()
    {
        tortoiseAttackCollider.enabled = true;
    }

    public void DisableTortoiseAttackCollider()
    {
        tortoiseAttackCollider.enabled = false;
        GetComponent<Animator>().SetBool("Attacking", false);
    }
}
