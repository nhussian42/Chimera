using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocLegs : Legs
{
    [SerializeField] ParticleSystem burrowParticlePrefab;
    private GameObject burrowParticle;
    [SerializeField] float burrowDuration;

    private SkinnedMeshRenderer[] headRenderer = new SkinnedMeshRenderer[2];
    private SkinnedMeshRenderer coreRenderer;
    private SkinnedMeshRenderer[] rightArmRenderer = new SkinnedMeshRenderer[2];
    private SkinnedMeshRenderer[] leftArmRenderer = new SkinnedMeshRenderer[2];
    private SkinnedMeshRenderer[] legsRenderer = new SkinnedMeshRenderer[2];

    public override void ActivateAbility()
    {
        // Get the Skinned Mesh Renderers of all the current limbs
        // (probably bad for performance to be getting all these components like this) but it could be fine?
        headRenderer = player.currentHead.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        coreRenderer = player.Core.gameObject.GetComponent<SkinnedMeshRenderer>();
        rightArmRenderer = player.currentRightArm.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        leftArmRenderer = player.currentLeftArm.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        legsRenderer = player.currentLegs.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        Vector3 burrowPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        burrowParticle = Instantiate(burrowParticlePrefab.gameObject, burrowPos, Quaternion.identity, player.transform);
        StartCoroutine(Burrow());
        StartCoroutine(Cooldown());
    }

    private IEnumerator Burrow()
    {
        player.ToggleInvincibility();
        foreach (SkinnedMeshRenderer mesh in headRenderer) { mesh.enabled = false; }
        coreRenderer.enabled = false;
        foreach (SkinnedMeshRenderer mesh in rightArmRenderer) { mesh.enabled = false; }
        foreach (SkinnedMeshRenderer mesh in leftArmRenderer) { mesh.enabled = false; }
        foreach (SkinnedMeshRenderer mesh in legsRenderer) { mesh.enabled = false; }

        yield return new WaitForSeconds(burrowDuration);

        player.ToggleInvincibility();
        foreach (SkinnedMeshRenderer mesh in headRenderer) { mesh.enabled = true; }
        coreRenderer.enabled = true;
        foreach (SkinnedMeshRenderer mesh in rightArmRenderer) { mesh.enabled = true; }
        foreach (SkinnedMeshRenderer mesh in leftArmRenderer) { mesh.enabled = true; }
        foreach (SkinnedMeshRenderer mesh in legsRenderer) { mesh.enabled = true; }

        Destroy(burrowParticle);
    }
}
