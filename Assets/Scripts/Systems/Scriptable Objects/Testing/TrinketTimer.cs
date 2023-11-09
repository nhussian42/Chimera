using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketTimer : MonoBehaviour
{
    public void Play(float time)
    {
        StartCoroutine(Coroutine(time));
    }
    public IEnumerator Coroutine(float time)
    {
        Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(time);
        Debug.Log("Coroutine Finished in " + time + " seconds");
    }
}
