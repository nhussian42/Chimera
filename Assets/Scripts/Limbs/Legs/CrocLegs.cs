using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class CrocLegs : Legs
{
    [SerializeField] GameObject burrowParticlePrefab;
    [SerializeField] GameObject trailParticlePrefab;
    private GameObject burrowParticleFX;
    private GameObject trailParticleFX;
    private VisualEffect trailEffect;
    [SerializeField] float burrowDuration;
    bool isUnderground = false;

    private SkinnedMeshRenderer[] headRenderer = new SkinnedMeshRenderer[2];
    private SkinnedMeshRenderer coreRenderer;
    private SkinnedMeshRenderer[] rightArmRenderer = new SkinnedMeshRenderer[2];
    private SkinnedMeshRenderer[] leftArmRenderer = new SkinnedMeshRenderer[2];
    private SkinnedMeshRenderer[] legsRenderer = new SkinnedMeshRenderer[2];

    public override void PlayAnim()
    {
        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerBurrow);
        player.DisableAllDefaultControls();
        player.Animator.SetTrigger("Burrow");
        burrowParticleFX = Instantiate(burrowParticlePrefab, player.transform.position, Quaternion.identity);
        VisualEffect burrowEffect = burrowParticleFX.GetComponentInChildren<VisualEffect>();
        burrowEffect.SetVector3("Position", new Vector3(player.transform.position.x, 0, player.transform.position.z));
    }

    private void Update()
    {
        if (isUnderground == true && trailEffect != null)
        {
            trailEffect.SetVector3("Position", new Vector3(player.transform.position.x, 0, player.transform.position.z));
        }
    }

    public override void ActivateAbility()
    {
        Destroy(burrowParticleFX);
        if (isUnderground == false)
        {
            player.EnableAllDefaultControls();
            player.ToggleInvincibility();
            isUnderground = true;

            // Get the Skinned Mesh Renderers of all the current limbs
            // (probably bad for performance to be getting all these components like this) but it could be fine?
            headRenderer = player.currentHead.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            coreRenderer = player.Core.gameObject.GetComponent<SkinnedMeshRenderer>();
            rightArmRenderer = player.currentRightArm.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            leftArmRenderer = player.currentLeftArm.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            legsRenderer = player.currentLegs.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            Vector3 trailPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            trailParticleFX = Instantiate(trailParticlePrefab, trailPos, Quaternion.identity, player.transform);
            trailEffect = trailParticleFX.GetComponentInChildren<VisualEffect>();
            StartCoroutine(Burrow());
        }
        else
        {
            Destroy(burrowParticleFX);
            SetMeshVisibility(true);
            player.ToggleInvincibility();
            isUnderground = false;
            player.EnableAllDefaultControls();
            StartCoroutine(Cooldown());
        }

    }

    private IEnumerator Burrow()
    {
        SetMeshVisibility(false);

        yield return new WaitForSeconds(burrowDuration);

        Destroy(trailParticleFX);
        player.DisableAllDefaultControls();
        burrowParticleFX = Instantiate(burrowParticlePrefab, player.transform.position, Quaternion.identity);
        VisualEffect burrowEffect = burrowParticleFX.GetComponentInChildren<VisualEffect>();
        burrowEffect.SetVector3("Position", new Vector3(player.transform.position.x, 0, player.transform.position.z));
        player.Animator.SetTrigger("Surface");
        AudioManager.PlaySound2D(AudioEvents.Instance.OnCrocResurface);
    }

    private void SetMeshVisibility(bool condition)
    {
        if (condition == false)
        {
            foreach (SkinnedMeshRenderer mesh in headRenderer) { mesh.enabled = false; }
            coreRenderer.enabled = false;
            foreach (SkinnedMeshRenderer mesh in rightArmRenderer) { mesh.enabled = false; }
            foreach (SkinnedMeshRenderer mesh in leftArmRenderer) { mesh.enabled = false; }
            foreach (SkinnedMeshRenderer mesh in legsRenderer) { mesh.enabled = false; }

        }
        else
        {
            foreach (SkinnedMeshRenderer mesh in headRenderer) { mesh.enabled = true; }
            coreRenderer.enabled = true;
            foreach (SkinnedMeshRenderer mesh in rightArmRenderer) { mesh.enabled = true; }
            foreach (SkinnedMeshRenderer mesh in leftArmRenderer) { mesh.enabled = true; }
            foreach (SkinnedMeshRenderer mesh in legsRenderer) { mesh.enabled = true; }
        }
    }
}
