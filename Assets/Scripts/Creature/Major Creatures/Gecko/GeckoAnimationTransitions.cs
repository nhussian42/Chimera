using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoAnimationTransitions : MonoBehaviour
{
    [SerializeField] private MeshCollider attackCollider;
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }
}
