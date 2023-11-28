using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    private Animator animator;
    private bool introCam = true;

    [SerializeField]
    private float delayTime;
    void Awake()
    {
        animator = GetComponent<Animator>();
        IntroCam();
    }

    private void IntroCam()
    {
        StartCoroutine(IntroDelay(delayTime));
        // SwitchCam();

    }

    private IEnumerator IntroDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SwitchCam();
        yield return null;
    }

    private void SwitchCam()
    {
        if (introCam)
        {
            animator.Play("DefaultCam");
        }
        else
        {
            animator.Play("IntroCam");
        }
    }
}
