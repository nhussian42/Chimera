using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimationEvents : MonoBehaviour
{
    [SerializeField] private MeshCollider wolfAttackCollider;

    public void EnableWolfAttackCollider()
    {
        wolfAttackCollider.enabled = true;
    }

    public void DisableWolfAttackCollider()
    {
        wolfAttackCollider.enabled = false;
    }

    public void AttackCompleted()
    {
        GetComponentInParent<Wolf>().attackCompleted = true;
    }
}
