using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void ResumePressed()
    {
        UIManager.ResumePressed.Invoke();
    }
    public void QuitPressed()
    {
        UIManager.QuitPressed.Invoke();
    }
}
