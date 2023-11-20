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
        AudioManager.Instance.PlayMenuSFX("SelectPlay");
        AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.PlayMusic("DungeonMusic");

        foreach (Button child in GetComponentsInChildren<Button>())
        {
            child.interactable = false;
        }
        
        StartPressed?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
