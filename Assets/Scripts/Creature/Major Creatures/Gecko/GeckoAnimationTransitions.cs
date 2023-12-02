using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoAnimationTransitions : MonoBehaviour
{
    [SerializeField] private MeshCollider attackCollider;
    [SerializeField] private BoxCollider geckoCollider;
    [SerializeField] private GameObject biteVFX;
    [SerializeField] private Transform biteVFXspawnPos;
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    public void BiteAttackVFX()
    {
        GameObject particle = Instantiate(biteVFX, biteVFXspawnPos.position, Quaternion.identity, this.transform);
        Destroy(particle, 1.5f);
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
