using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System.Security.Cryptography;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;

    private Animator animator;
    private bool isShop = true;
 
    [SerializeField]
    private float introDelay;
    void Awake()
    {
        // animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        BossCutsceneTrigger.BossCutscene += StartBossCutscene;
    }

    private void OnDisable()
    {
        BossCutsceneTrigger.BossCutscene -= StartBossCutscene;
    }

    private void Start()
    {

    }

    private void StartBossCutscene()
    {
        print("Cutscene playing!");
        //cutsceneDirector.Play();
    }

    private void StartIntroCutscene()
    {
        // cutsceneDirector.Play();
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
