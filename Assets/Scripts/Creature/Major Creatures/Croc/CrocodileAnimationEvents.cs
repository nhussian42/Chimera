using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileAnimationEvents : MonoBehaviour
{

    [SerializeField] private SphereCollider resurfaceAttackCollider;
    [SerializeField] private MeshCollider biteAttackCollider;
    [SerializeField] private BoxCollider crocCollider;
    

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

    public void EnableBiteAttackCollider()
    {
        biteAttackCollider.enabled = true;
    }

    public void DisableBiteAttackCollider()
    {
        biteAttackCollider.enabled = false;
    }
}
