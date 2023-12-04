using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class GrolfinoAnimationEvents : MonoBehaviour
{
    [SerializeField] private BoxCollider bossCollider;
    [SerializeField] private GameObject bossMesh;
    [SerializeField] private BoxCollider bossHeadCollider;
    [SerializeField] private CapsuleCollider bossHornsColliders;
    [SerializeField] private SphereCollider slamCollider;
    [SerializeField] private CapsuleCollider slamAttackCollider;
    [SerializeField] private GameObject slamVFX;
    [SerializeField] private GameObject bossCrackIntro;
    [SerializeField] private GameObject bossCrack;

    private Grolfino grolfino;
    private GameObject bossCrackUnburrowObject;
    private GameObject bossCrackBurrowObject;
    private void Start()
    {
        grolfino = GetComponentInParent<Grolfino>();
    }
    public void ExitIntroPhase()
    {
        StartCoroutine(grolfino.Burrow());
    }

    public void IntroBurrow(float zPos)
    {
        bossCrackUnburrowObject = Instantiate(bossCrackIntro, new Vector3(transform.position.x, 0.2f, transform.position.z) - (transform.forward * zPos), Quaternion.identity);
        bossCrackUnburrowObject.GetComponentInChildren<VisualEffect>().SetVector3("Position", new Vector3(transform.position.x, 0.2f, transform.position.z) - (transform.forward * zPos));

    }

    public void Unburrow(float zPos)
    {
        bossCrackUnburrowObject = Instantiate(bossCrack, new Vector3(transform.position.x, 0.2f, transform.position.z) - (transform.forward * zPos), Quaternion.identity);
        bossCrackUnburrowObject.GetComponentInChildren<VisualEffect>().SetVector3("Position", new Vector3(transform.position.x, 0.2f, transform.position.z) - (transform.forward * zPos));
    }

    public void UnburrowEnd()
    {
        bossCrackUnburrowObject.GetComponentInChildren<Animator>().SetBool("Burrow", true);
        Destroy(bossCrackUnburrowObject, 1.5f);
    }

    public void Burrow(float zPos)
    {
        bossCrackBurrowObject = Instantiate(bossCrack, new Vector3(transform.position.x, 0.2f, transform.position.z) - (transform.forward * zPos), Quaternion.identity);
        bossCrackBurrowObject.GetComponentInChildren<VisualEffect>().SetVector3("Position", new Vector3(transform.position.x, 0.2f, transform.position.z) - (transform.forward * zPos));
    }

    public void BurrowEnd()
    {
        if (bossCrackBurrowObject != null)
        {
            bossCrackBurrowObject.GetComponentInChildren<Animator>().SetBool("Burrow", true);
            Destroy(bossCrackBurrowObject, 1f);
        }
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
        CameraShake.Instance.BossBurrowShake(true);
    }

    public void EndBurrow()
    {
        bossCrackIntro.SetActive(true);
        bossMesh.SetActive(true);
        CameraShake.Instance.BossBurrowShake(false);
        CameraShake.Instance.BossAttackShake();
    }

    public void GroundSlam()
    {
        grolfino.slamAttack = true;
        slamCollider.enabled = true;
        slamAttackCollider.enabled = true;
        GameObject g = Instantiate(slamVFX, new Vector3(transform.position.x, 0.2f, transform.position.z) + (transform.forward * 16), Quaternion.identity);
        Destroy(g, 2.5f);
        grolfino.knockbackForce = 75f;
        CameraShake.Instance.BossAttackShake();
    }

    public void ResetGroundSlamCollider()
    {
        slamAttackCollider.enabled = false;
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
