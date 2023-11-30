using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class BossAI : Creature
{
    protected GameObject player;
    private bool playeriFrame = false;
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
        // PlayerController.Instance.isInvincible is temporary for fixing croc knockback while player is burrowing
        if (other.gameObject.GetComponent<CharacterController>() != null && PlayerController.Instance.isInvincible == false)
        {
            Debug.Log("Dealt damage to player");

            PlayerController.Instance.DistributeDamage(attackDamage);

            StartCoroutine(PlayerKnockback((player.transform.position - transform.position).normalized, knockbackForce, 0.4f));
        }
    }

    void IFrame()
    {
        playeriFrame = false;
    }
}
