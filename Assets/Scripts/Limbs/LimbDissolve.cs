using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class LimbDissolve : MonoBehaviour
{
    [SerializeField] float dissolveDuration = 3; 
    [SerializeField] float undissolvedHeight = 0; 
    [SerializeField] float fullyDissolvedHeight = 10;

    Material mat;

    private void Start()
    {
        mat = GetComponent<SkinnedMeshRenderer>().material;
        Dissolve();
    }

    private void OnDisable()
    {
        mat.SetFloat("_DissolveHeight", undissolvedHeight);
    }

    public void Dissolve()
    {
        StartCoroutine(DissolveLimb());
    }

    public void DissolveWithColor(Color dissolveColor)
    {
        mat.SetColor("_DissolveColor", dissolveColor);
        StartCoroutine(DissolveLimb());
    }

    private IEnumerator DissolveLimb()
    {
        float timeElapsed = 0;
        float dissolveHeight;

        while (timeElapsed < dissolveDuration)
        {
            dissolveHeight = Mathf.Lerp(undissolvedHeight, fullyDissolvedHeight, timeElapsed / dissolveDuration);
            timeElapsed += Time.deltaTime;
            mat.SetFloat("_DissolveHeight", dissolveHeight);
            yield return null;
        }
    }
}
