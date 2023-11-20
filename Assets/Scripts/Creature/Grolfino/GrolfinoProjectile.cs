using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrolfinoProjectile : MonoBehaviour
{
    public float projectileDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null)
        {
            Debug.Log("Dealt damage to player");
            PlayerController.Instance.DistributeDamage(projectileDamage);
        }
    }
}
