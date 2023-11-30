using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class DissolveObject : MonoBehaviour
{
    [SerializeField] float undissolvedHeight = 0; 
    [SerializeField] float fullyDissolvedHeight = 10;

    public static float dissolveDuration = 4f;

    Material [] mats;

    private void OnEnable()
    {
        mats = GetComponent<SkinnedMeshRenderer>().materials;
        // Dissolve();
    }

    private void OnDisable()
    {
        foreach (Material mat in mats)
            mat.SetFloat("_DissolveHeight", undissolvedHeight);
    }

    public void Dissolve(bool destroyAfterComplete)
    {
        StartCoroutine(DissolveOverTime(destroyAfterComplete));
    }

    public void DissolveWithColor(Color dissolveColor, bool destroyAfterComplete)
    {
        foreach (Material mat in mats)
            mat.SetColor("_DissolveColor", dissolveColor);

        StartCoroutine(DissolveOverTime(destroyAfterComplete));
    }

    private IEnumerator DissolveOverTime(bool destroyAfterComplete)
    {
        float timeElapsed = 0;
        float dissolveHeight;

        while (timeElapsed < dissolveDuration)
        {
            dissolveHeight = Mathf.Lerp(undissolvedHeight, fullyDissolvedHeight, timeElapsed / dissolveDuration);
            timeElapsed += Time.deltaTime;

            foreach (Material mat in mats)
                mat.SetFloat("_DissolveHeight", dissolveHeight);
            
            yield return null;
        }

        gameObject.SetActive(false);
        if (destroyAfterComplete) Destroy(gameObject);
    }
}
