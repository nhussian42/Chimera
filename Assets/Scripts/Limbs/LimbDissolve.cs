using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDissolve : MonoBehaviour
{
    float lerpDuration = 3; 
    [SerializeField] float startValue = 0; 
    [SerializeField] float endValue = 10; 
    float valueToLerp;

    Material mat;

    void Start()
    {
        
        mat = GetComponent<SkinnedMeshRenderer>().material;
        StartCoroutine(Lerp());
    }
    IEnumerator Lerp()
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
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

}
