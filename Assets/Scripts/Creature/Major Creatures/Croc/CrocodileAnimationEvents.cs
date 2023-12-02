using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CrocodileAnimationEvents : MonoBehaviour
{

    [SerializeField] private SphereCollider resurfaceAttackCollider;
    [SerializeField] private MeshCollider biteAttackCollider;
    [SerializeField] private BoxCollider crocCollider;
    [SerializeField] private GameObject biteVFX;
    [SerializeField] private Transform biteVFXspawnPos;


    public void EnableResurfaceAttackCollider()
    {
        CameraShake.Instance.CreatureAttackShake();
        resurfaceAttackCollider.enabled = true;
        crocCollider.enabled = true;
    }

    public void DisableResurfaceAttackCollider()
    {
        resurfaceAttackCollider.enabled = false;
    }

    public void StartBiteVFX()
    {
        GameObject particle = Instantiate(biteVFX, biteVFXspawnPos.position, transform.rotation);
        Destroy(particle, 1.5f);
    }
    public void EnableBiteAttackCollider()
    {
        biteAttackCollider.enabled = true;
    }

    public void DisableBiteAttackCollider()
    {
        biteAttackCollider.enabled = false;
    }
}
