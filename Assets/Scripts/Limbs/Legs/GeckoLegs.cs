using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoLegs : Legs
{
    [SerializeField] float dashDistance;
    [SerializeField] float dashSpeed;
    [SerializeField] int defaultDashCharges;
    int currentDashCharge;

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
        }
    }

    public override void PlayAnim()
    {
        player.Animator.SetTrigger("Dash");
        
        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerBaseDash);
    }

    public override void ActivateAbility()
    {
        t = 0;
        activated = true;
        currentDashCharge++;
        if(currentDashCharge == defaultDashCharges)
        {
            currentDashCharge = 0;
            StartCoroutine(Cooldown());
        }
    }
}
