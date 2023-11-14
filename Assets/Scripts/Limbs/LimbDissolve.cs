using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class LimbDissolve : MonoBehaviour
{
    [SerializeField] float dissolveDuration = 3; 
    [SerializeField] float startValue = 0; 
    [SerializeField] float endValue = 10; 
    float valueToLerp;

    Material mat;

    private void Start()
    {
        
        mat = GetComponent<SkinnedMeshRenderer>().material;
        StartCoroutine(Lerp());
    }
    private IEnumerator Lerp()
    {
        float timeElapsed = 0;
        while (timeElapsed < dissolveDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / dissolveDuration);
            timeElapsed += Time.deltaTime;
            mat.SetFloat("_DissolveHeight", valueToLerp);
            yield return null;
        }

        valueToLerp = endValue;
        float temp = startValue;
        startValue = endValue;
        endValue = temp;

        StartCoroutine(Lerp());
    }

    private void OnDisable()
    {
        
    }
}
