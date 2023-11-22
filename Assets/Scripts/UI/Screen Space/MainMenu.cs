using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static Action StartPressed;
    public static Action LoadFirstRoom;

    private void OnEnable()
    {
        LoadFirstRoom += LoadFirstRoomScene;
    }

    private void OnDisable()
    {
        LoadFirstRoom -= LoadFirstRoomScene;
    }

    private void LoadFirstRoomScene()
    {
        ChimeraSceneManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame()
    {
        foreach (Button child in GetComponentsInChildren<Button>())
        {
            child.interactable = false;
        }

        AudioManager.PlaySound2D(AudioEvents.Instance.OnGameStart);
        
        StartPressed?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
