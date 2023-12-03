using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimationEvents : MonoBehaviour
{
    [SerializeField] private MeshCollider wolfAttackCollider;
    [SerializeField] private GameObject wolfSlashVFX;
    private ParticleSystem[] particleSystems;

    private void Start()
    {
        particleSystems = wolfSlashVFX.GetComponentsInChildren<ParticleSystem>();
    }

    public void EnableWolfAttackCollider()
    {
        wolfAttackCollider.enabled = true;
    }

    public void PlayWolfSlashVFX()
    {
        foreach (ParticleSystem p in particleSystems)
        {
            p.Play();
        }
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
