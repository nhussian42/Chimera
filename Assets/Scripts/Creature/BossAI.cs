using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class BossAI : Creature
{
    protected GameObject player;
    private bool iFrame = false;
    [SerializeField] protected LayerMask playerLayerMask;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.Instance.gameObject;
        playerLayerMask = LayerMask.GetMask("Player");
        currentHealth = health;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null && iFrame == false)
        {
            iFrame = true;
            Invoke("IFrame", 0.5f);
            Debug.Log("Dealt damage to player");
            PlayerController.Instance.DistributeDamage(attackDamage);
        }
    }

    void IFrame()
    {
        iFrame = false;
    }
}
