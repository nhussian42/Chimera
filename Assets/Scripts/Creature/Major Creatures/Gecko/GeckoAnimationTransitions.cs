using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoAnimationTransitions : MonoBehaviour
{
    [SerializeField] private MeshCollider attackCollider;
    [SerializeField] private BoxCollider geckoCollider;
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    public void EnableGeckoCollider()
    {
        geckoCollider.enabled = true;
    }

    public void DisableGeckoCollider()
    {
        geckoCollider.enabled = true;
    }
}
