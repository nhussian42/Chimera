using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Croc Legs Behavior to do list:
/* - Player cannot be damaged
 * - Player cannot attack or perform other abilities while underground
 */


public class CrocLegs : Legs
{
    [SerializeField] ParticleSystem burrowParticlePrefab;
    private GameObject burrowParticle;
    [SerializeField] float burrowDuration;

    private SkinnedMeshRenderer[] headRenderer = new SkinnedMeshRenderer[2];
    private SkinnedMeshRenderer coreRenderer;
    private SkinnedMeshRenderer rightArmRenderer;
    private SkinnedMeshRenderer leftArmRenderer;
    private SkinnedMeshRenderer[] legsRenderer = new SkinnedMeshRenderer[2];

    protected override void Start()
    {
        base.Start();

        // Get the Skinned Mesh Renderers of all the current limbs
        headRenderer = player.currentHead.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        coreRenderer = player.Core.gameObject.GetComponent<SkinnedMeshRenderer>();
        rightArmRenderer = player.currentRightArm.gameObject.GetComponent<SkinnedMeshRenderer>();
        leftArmRenderer = player.currentLeftArm.gameObject.GetComponent<SkinnedMeshRenderer>();
        legsRenderer = player.currentLegs.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    public override void ActivateAbility()
    {
        Vector3 burrowPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        burrowParticle = Instantiate(burrowParticlePrefab.gameObject, burrowPos, Quaternion.identity, player.transform);
        StartCoroutine(Burrow());
        StartCoroutine(Cooldown());
    }

    private IEnumerator Burrow()
    {
        player.ToggleInvincibility();
        player._attackLeft.Disable();
        player._attackRight.Disable();
        player._legsAbility.Disable();
        player._swapLimbs.Disable();
        player._interact.Disable();
        foreach (SkinnedMeshRenderer mesh in headRenderer) { mesh.enabled = false; }
        coreRenderer.enabled = false;
        rightArmRenderer.enabled = false;
        leftArmRenderer.enabled = false;
        foreach(SkinnedMeshRenderer mesh in legsRenderer) { mesh.enabled = false; }

        yield return new WaitForSeconds(burrowDuration);

        player.ToggleInvincibility();
        player._attackLeft.Enable();
        player._attackRight.Enable();
        player._legsAbility.Enable();
        player._swapLimbs.Enable();
        player._interact.Enable();
        foreach (SkinnedMeshRenderer mesh in headRenderer) { mesh.enabled = true; }
        coreRenderer.enabled = true;
        rightArmRenderer.enabled = true;
        leftArmRenderer.enabled = true;
        foreach (SkinnedMeshRenderer mesh in legsRenderer) { mesh.enabled = true; }

        Destroy(burrowParticle);
    }
}
