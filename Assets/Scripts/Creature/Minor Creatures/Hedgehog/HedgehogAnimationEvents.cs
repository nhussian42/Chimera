using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogAnimationEvents : MonoBehaviour
{
    [SerializeField] private BoxCollider hedgehogAttackCollider;

    public void EnableHedgehogAttackCollider()
    {
        hedgehogAttackCollider.enabled = true;
    }

    public void DisableHedgehogAttackCollider()
    {
        hedgehogAttackCollider.enabled = false;
    }
}
