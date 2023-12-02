using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class NotBossAI : Creature
{
    protected GameObject player;
    private bool playerIFrame = false;
    protected bool stunned = false; //temp variable for stun behavior, refactor this later - Amon
    protected bool stunnable = true;
    [SerializeField] private Transform stunSpawnTransform;
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
        if(stunned != true)
        {
            Debug.Log("Update() called");
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
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // PlayerController.Instance.isInvincible is temporary for fixing croc knockback while player is burrowing
        if (other.gameObject.GetComponent<CharacterController>() != null && attacking == true && playerIFrame == false && PlayerController.Instance.isInvincible == false)
        {
            playerIFrame = true;
            Invoke("PlayerIFrame", 0.5f);
            //Debug.Log("Dealt damage to player");

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

    public virtual void ResetAttackBooleans()
    {
        attacking = false;
    }

    public void Stun(float duration, GameObject stunFX)
    {
        if(stunnable == true)
        {
            //Debug.Log("Called Stun()");
            StopAllCoroutines();
            StartCoroutine(Stunned(duration, stunFX));
        }      
    }

    // temp function for stun behavior, refactor this later - Amon
    protected IEnumerator Stunned(float duration, GameObject stunFX)
    {
        // Debug.Log("Called Stunned()");
        // set stunned bool to true for length of duration then set it back to false, instantiate stunned VFX at pos (See Update() function) - Amon 
        stunned = true;
        StartCoroutine(StunCooldown(3.0f));
        GameObject stunnedFX = Instantiate(stunFX, stunSpawnTransform); // Instantiate particle effect passed from RhinoHead, get VisualEffect component in children and set pos
        VisualEffect effect = stunnedFX.GetComponentInChildren<VisualEffect>();
        effect.SetVector3("Position", stunSpawnTransform.position);
        yield return new WaitForSeconds(duration);
        Destroy(stunnedFX);
        ResetAttackBooleans();
        stunned = false;
    }

    protected IEnumerator StunCooldown(float duration)
    {
        //Debug.Log("Called StunCooldown()");
        stunnable = false;
        yield return new WaitForSeconds(duration);
        stunnable = true;
    }
}
