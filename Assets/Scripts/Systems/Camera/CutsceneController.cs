using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;

    public static Action IntroZoom;
    public static Action BossZoom;

    public static Action PlayWakeUp;

    private Animator animator;
    private bool isShop = true;

    private float timer = 0f;
 
    private void OnEnable()
    {
        IntroCutsceneTrigger.IntroWakeUp += StartWakeUp;
        Boss1CutsceneTrigger.Boss1Cutscene += StartBossCutscene;
    }

    private void OnDisable()
    {
        IntroCutsceneTrigger.IntroWakeUp -= StartWakeUp;
        Boss1CutsceneTrigger.Boss1Cutscene -= StartBossCutscene;
    }

    private void StartWakeUp()
    {
        PlayWakeUp?.Invoke();
    }

    private void StartBossCutscene()
    {
        // print("Cutscene playing!");
        cutsceneDirector.Play();
    }

    private void SwitchCam()
    {
        if (isShop)
        {
            animator.Play("DefaultCam");
        }
        else
        {
            animator.Play("IntroCam");
        }
    }
}
