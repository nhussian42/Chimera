using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfLegs : Legs
{
    [SerializeField] float dashDistance;
    [SerializeField] float dashSpeed;

    private float t; //interpolator
    private bool activated;

    private void Update()
    {
        if (activated == true)
        {
            Vector3 dashPos = player.transform.forward * Mathf.Lerp(0, dashDistance, t) * Time.deltaTime;
            player._controller.Move(dashPos);

            t += dashSpeed * Time.deltaTime;
        }

        if (t >= 1)
        {
            activated = false;
            //player.EnableAllDefaultControls();
        }
    }

    public override void PlayAnim()
    {
        //player.DisableAllDefaultControls();
        player.Animator.SetTrigger("Pounce");
        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerPounce);
    }

    public override void ActivateAbility()
    {
        //Debug.Log("pounce");
        t = 0;
        activated = true;
        StartCoroutine(Cooldown());
    }
}
