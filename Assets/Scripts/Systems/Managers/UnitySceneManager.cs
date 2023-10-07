using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitySceneManager : Singleton<UnitySceneManager>
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
        FloorManager.LoadNextRoom?.Invoke();
    }

    private IEnumerator FadeIn(float fadeInTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        FloorManager.EnableFloor?.Invoke();
    }

    public void LoadScene(int sceneID)
    {
        // AudioManager.Instance.musicSource.Stop();
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    private IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        // loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            // float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            // loadingBarFill.fillAmount = progressValue;


            yield return null;
        }
    }

}
