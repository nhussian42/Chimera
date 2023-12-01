using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System.Security.Cryptography;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;

    public static Action IntroZoom;
    public static Action BossZoom;

    private Animator animator;
    private bool isShop = true;
 
    void Awake()
    {
        // animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        IntroCutsceneTrigger.IntroCutscene += StartIntroCutscene;
        Boss1CutsceneTrigger.Boss1Cutscene += StartBossCutscene;
    }

    private void OnDisable()
    {
        IntroCutsceneTrigger.IntroCutscene -= StartIntroCutscene;
        Boss1CutsceneTrigger.Boss1Cutscene -= StartBossCutscene;
    }

    private void Start()
    {

    }

    private void StartIntroCutscene()
    {
        // cutsceneDirector.Play();
    }
    private void StartBossCutscene()
    {
        print("Cutscene playing!");
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
