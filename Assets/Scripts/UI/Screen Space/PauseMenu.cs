using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject ResumeButton;

    public static Action OnPressedQuit;
    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ResumeButton);
    }

    public void ResumePressed()
    {
        UIManager.ResumePressed?.Invoke();
    }
    public void QuitPressed()
    {
        OnPressedQuit?.Invoke();
    }

    public void RestartGame()
    {
        FloorManager.Instance.LoadMainMenu();
    }
}
