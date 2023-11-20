using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileAnimationEvents : MonoBehaviour
{

    [SerializeField] private SphereCollider resurfaceAttackCollider;
    [SerializeField] private MeshCollider biteAttackCollider;

    public void EnableResurfaceAttackCollider()
    {
        resurfaceAttackCollider.enabled = true;
    }

    public void DisableResurfaceAttackCollider()
    {
        resurfaceAttackCollider.enabled = false;
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
