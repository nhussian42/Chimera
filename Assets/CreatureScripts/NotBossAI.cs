using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NotBossAI : Creature
{
    protected NavMeshAgent agent;
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
    }

    private void Update()
    {
        agent.destination = player.transform.position;

        if (attacking == false)
        {
            if (Physics.CheckSphere(transform.position, attackRange, playerLayerMask))
            {
                //Player is in range
                //Perform attack coroutine
                StartCoroutine(Attack());
                attacking = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TEMP_CharacterController>() != null)
        {
            Debug.Log("Player damaged");
            //Player gets damaged here
        }
    }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }
}
