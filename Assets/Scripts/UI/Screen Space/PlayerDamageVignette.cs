using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDamageVignette : MonoBehaviour
{
    private Vignette vignette;

    [SerializeField] private float cooldownTime;
    [SerializeField] private float fadeOutDurationMultiplier = 15;
    [SerializeField] [Range(0,1)] private float lowerBoundIntensity = 0.4f;
    [SerializeField] [Range(0,1)] private float upperBoundIntensity = 0.55f;
    private bool cooledDown = true;

    private void Awake()
    {
        VolumeProfile profile = GetComponent<Volume>().sharedProfile;
        if (profile.TryGet<Vignette>(out Vignette vignette))
        {
            this.vignette = vignette;
        }
    }

    private void OnEnable()
    {
        PlayerController.OnDamageReceived += UpdateIntensity;
    }

    private void Start()
    {
        UpdateIntensity();
    }

    private void OnDisable()
    {
        PlayerController.OnDamageReceived -= UpdateIntensity;
    }

    private void UpdateIntensity()
    {
        if (!cooledDown) return;
        StopAllCoroutines();

        float lerpValue = 1 - PlayerController.Instance.Core.Health/PlayerController.Instance.Core.MaxHealth;
        vignette.intensity.value = Mathf.Lerp(lowerBoundIntensity, upperBoundIntensity, lerpValue);
        StartCoroutine(FadeOutVignette(vignette.intensity.value * fadeOutDurationMultiplier));
        StartCoroutine(Cooldown());
    }

    private IEnumerator FadeOutVignette(float fadeOutTime)
    {
        float curTime = 0;
        float startingIntensity = vignette.intensity.value;

        while (curTime < fadeOutTime)
        {
            curTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(startingIntensity, 0, curTime/fadeOutTime);
            yield return null;
        }
    }

    private IEnumerator Cooldown()
    {
        float curTime = 0;
        cooledDown = false;

        while (curTime < cooldownTime)
        {
            curTime += Time.deltaTime;

            yield return null;
        }

        cooledDown = true;
    }
}
