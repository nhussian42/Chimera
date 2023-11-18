using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class NotBossAI : Creature
{
    protected GameObject player;
    private bool playerIFrame = false;
    [SerializeField] protected LayerMask playerLayerMask;    //Used to check distance from the player

    [SerializeField] protected bool attacking = false;   //Keeps track of if the creature is currently attacking

    private void Start()
    {

    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.Instance.gameObject;
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

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // PlayerController.Instance.isInvincible is temporary for fixing croc knockback while player is burrowing
        if (other.gameObject.GetComponent<CharacterController>() != null && attacking == true && playerIFrame == false && PlayerController.Instance.isInvincible == false)
        {
            playerIFrame = true;
            Invoke("PlayerIFrame", 0.5f);
            Debug.Log("Dealt damage to player");

            PlayerController.Instance.DistributeDamage(attackDamage);

            StartCoroutine(PlayerKnockback((player.transform.position - transform.position).normalized, knockbackForce, 0.4f));
        }
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null && attacking == true && playerIFrame == false && PlayerController.Instance.isInvincible == false && this.gameObject.layer == 9)
        {
            playerIFrame = true;
            Invoke("PlayerIFrame", 0.5f);
            Debug.Log("Dealt damage to player");

            PlayerController.Instance.DistributeDamage(attackDamage);

            StartCoroutine(PlayerKnockback((player.transform.position - transform.position).normalized, knockbackForce, 0.4f));
        }
    }

    void PlayerIFrame()
    {
        playerIFrame = false;
    }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }
}
