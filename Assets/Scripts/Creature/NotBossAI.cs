using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class NotBossAI : Creature
{
    protected GameObject player;
    private bool iFrame = false;
    [SerializeField] protected LayerMask playerLayerMask;    //Used to check distance from the player

    protected bool attacking = false;   //Keeps track of if the creature is currently attacking

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

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null && attacking == true && iFrame == false)
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

    public virtual IEnumerator Attack()
    {
        yield return null;
    }
}
