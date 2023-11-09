using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoSlamAttackTrigger : MonoBehaviour
{
    [SerializeField] private CapsuleCollider attackCollider;
    public void TriggerSlamCollider()
    {
        attackCollider.enabled = !attackCollider.enabled;
    }
}
