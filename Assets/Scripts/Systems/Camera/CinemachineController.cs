using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    private Animator animator;
    private bool isShop = true;
 
    [SerializeField]
    private float introDelay;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        IntroCam();
    }

    private void IntroCam()
    {
        //StartCoroutine(IntroDelay(introDelay));
        //SwitchCam();

    }

    private IEnumerator IntroDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SwitchCam();
        yield return null;
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
