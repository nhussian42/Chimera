using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] private float _fadeOutTime;
    private void OnEnable()
    {
        FloorManager.LeaveRoom += FadeToBlack;
    }

    private void OnDisable()
    {
        FloorManager.LeaveRoom -= FadeToBlack;
    }
    private void FadeToBlack()
    {
        StartCoroutine(FadeOut(_fadeOutTime));
    }

    private IEnumerator FadeOut(float fadeOutTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        FloorManager.TransitionPlayer?.Invoke();
    }

}
