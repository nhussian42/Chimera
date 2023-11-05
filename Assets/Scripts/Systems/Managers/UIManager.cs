using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private PauseMenu _pauseMenu;

    public static Action QuitPressed;
    public static Action ResumePressed;

    private void OnEnable()
    {
        ResumePressed += ResumeGamePressed;
        
        PlayerController.OnGamePaused += EnablePauseMenu;
    }

    void OnDisable()
    {
        ResumePressed -= ResumeGamePressed;
        
        PlayerController.OnGamePaused -= EnablePauseMenu;
    }

    private void ResumeGamePressed()
    {
        if (GameManager.CurrentGameState == GameState.IsQuitting) return;
        DisablePauseMenu();
    }

    private void EnablePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
    }

    private void DisablePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
    }    
}
