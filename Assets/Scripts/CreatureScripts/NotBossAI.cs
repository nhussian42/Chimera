using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class NotBossAI : Creature
{
    protected GameObject player;
    [SerializeField] protected LayerMask playerLayerMask;    //Used to check distance from the player

    protected bool attacking = false;   //Keeps track of if the creature is currently attacking

    private enum EnemyState
    {
        Idle,
        Chasing,
        Attacking
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerLayerMask = LayerMask.GetMask("Player");
        currentHealth = health;
    }

    protected virtual void Update()
    {
        agent.destination = player.transform.position;

        if (attacking == false && alive == true)
        {
            if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask))
            {
                //Player is in range
                //Perform attack coroutine
                StartCoroutine(Attack());
                attacking = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null && attacking == true)
        {
            PlayerController.Instance.DistributeDamage(attackDamage);
        }
    }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }
}
