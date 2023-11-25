using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegs : Legs
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
        }
    }

    public override void ActivateAbility()
    {
        t = 0;
        activated = true;
        StartCoroutine(Cooldown());
    }

}
