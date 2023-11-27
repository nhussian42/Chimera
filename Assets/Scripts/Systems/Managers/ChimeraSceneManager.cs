using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChimeraSceneManager : Singleton<ChimeraSceneManager>
{
    [SerializeField] private float _fadeOutTime;
    [SerializeField] private float _fadeInTime;

    public static Action<float> FadeValueChanged;
    public static Action<int> OnSceneSwitched;

    private void OnEnable()
    {
        FloorManager.LeaveRoom += FadeOutToBlack;
        FloorManager.NextRoomLoaded += FadeInToBlack;

        MainMenu.StartPressed += FadeOutToBlack;
        //PlayerController.OnDie += FadeOutToBlack;
    }

    private void OnDisable()
    {
        FloorManager.LeaveRoom -= FadeOutToBlack;
        FloorManager.NextRoomLoaded -= FadeInToBlack;

        MainMenu.StartPressed -= FadeOutToBlack;
    }

    private void Start()
    {
        OnSceneSwitched?.Invoke(SceneManager.GetActiveScene().buildIndex);
    }

    private void FadeOutToBlack()
    {
        StartCoroutine(FadeOut(_fadeOutTime));
    }

    private void FadeInToBlack()
    {
        StartCoroutine(FadeIn(_fadeOutTime));
    }

    private IEnumerator FadeOut(float fadeOutTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            FadeValueChanged?.Invoke(elapsedTime/fadeOutTime);
            yield return null;
        }
        FloorManager.LoadNextRoom?.Invoke();
        MainMenu.LoadFirstRoom?.Invoke();
    }

    private IEnumerator FadeIn(float fadeInTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            FadeValueChanged?.Invoke(1 - elapsedTime/fadeInTime);
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

        // trigger an action to the UI Manager with operation
        // loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            // float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            // loadingBarFill.fillAmount = progressValue;


            yield return null;
        }
    }
}
