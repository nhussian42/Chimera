using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RhinoAnimationEvents : MonoBehaviour
{
    [SerializeField] private BoxCollider chargeAttackCollider;
    [SerializeField] private SphereCollider slamAttackCollider;
    [SerializeField] private GameObject slamParticleSystem;

    public void EnableChargeAttackCollider()
    {
        chargeAttackCollider.enabled = true;
    }

    public void DisableChargeAttackCollider()
    {
        chargeAttackCollider.enabled = false;
    }

    public void EnableSlamAttackCollider()
    {
        slamAttackCollider.enabled = true;
        GameObject s = Instantiate(slamParticleSystem, transform.position + (transform.forward * 2), transform.rotation);
        Destroy(s, 2f);
        AudioManager.PlaySound3D(AudioEvents.Instance.OnRhinoSlam, s.transform.position);
    }

    public void DisableSlamAttackCollider()
    {
        slamAttackCollider.enabled = false;
    }

    public void SlamCompleted()
    {
        GetComponentInParent<Rhino>().slammed = true;
    }

}
