using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private static GameState _currentGameState;
    public static GameState CurrentGameState {get { return _currentGameState; } }

    public static Action OnUnpause;

    private void OnEnable()
    {
        PlayerController.OnGamePaused += PauseGame;
        PlayerController.ToggleMenuPause += PauseGame;
        UIManager.ResumePressed += UnpauseGame;
        UIManager.QuitPressed += QuitGame;
    }

    private void OnDisable()
    {
        PlayerController.OnGamePaused -= PauseGame;
        PlayerController.ToggleMenuPause -= PauseGame;
        UIManager.ResumePressed -= UnpauseGame;
        UIManager.QuitPressed -= QuitGame;
    }

    // TODO Swap player input to UI controls when game is paused
    private void PauseGame()
    {
        if (!PlayerController.Instance ||
         _currentGameState == GameState.IsLoading ||
        _currentGameState == GameState.IsQuitting) return;

        _currentGameState = GameState.IsPaused;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    private void UnpauseGame()
    {
        if (!PlayerController.Instance ||
         _currentGameState == GameState.IsLoading ||
        _currentGameState == GameState.IsQuitting) return;

        _currentGameState = GameState.IsPlaying;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        OnUnpause?.Invoke();
    }

    private void QuitGame()
    {
        _currentGameState = GameState.IsQuitting;
        Application.Quit();
    }
}
