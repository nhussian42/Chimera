using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GrolfinoAnimationEvents : MonoBehaviour
{
    [SerializeField] private BoxCollider bossCollider;
    [SerializeField] private GameObject bossMesh;
    [SerializeField] private BoxCollider bossHeadCollider;
    [SerializeField] private CapsuleCollider bossHornsColliders;
    [SerializeField] private SphereCollider slamCollider;
    private Grolfino grolfino;
    private void Start()
    {
        grolfino = GetComponentInParent<Grolfino>();
    }
    public void ExitIntroPhase()
    {
        StartCoroutine(grolfino.Burrow());
    }

    public void EnableCollider()
    {
        bossCollider.enabled = true;
        grolfino.burrowed = false;
    }

    public void DisableCollider()
    {
        bossCollider.enabled = false;
    }

    public void StartBurrow()
    {
        bossMesh.SetActive(false);
        grolfino.burrowed = true;
    }

    public void EndBurrow()
    {
        bossMesh.SetActive(true);
    }

    public void GroundSlam()
    {
        grolfino.slamAttack = true;
        slamCollider.enabled = true;
        grolfino.knockbackForce = 75f;
    }

    public void EndGroundSlam()
    {
        slamCollider.enabled = false;
    }

    public void ProjectileAttack()
    {
        grolfino.projectileAttack = true;
        grolfino.knockbackForce = 20f;
    }

    public void EndProjectileAttack()
    {
        grolfino.projectileAttack = false;
    }

    public void StartSweepAttack()
    {
        grolfino.sweepAttack = true;
        bossHeadCollider.enabled = true;
        bossHornsColliders.enabled = true;
        grolfino.knockbackForce = 35f;
    }

    public void EndSweepAttack()
    {
        grolfino.sweepAttack = false;
        bossHeadCollider.enabled = false;
        bossHornsColliders.enabled = false;
    }
}
