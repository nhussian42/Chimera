using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject ResumeButton;
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
        UIManager.QuitPressed?.Invoke();
    }

    public void RestartGame()
    {
        ChimeraSceneManager.Instance.LoadScene(0);
    }
}
