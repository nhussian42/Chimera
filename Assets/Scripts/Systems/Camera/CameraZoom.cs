using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private float timeElapsed = 0f;

    // private float targetOrthoSize = 11f;
    [SerializeField]
    private float defaultOrthoSize = 11f;

    [Header("Intro Zoom Parameters:")]
    [SerializeField]
    private float introOrthoSize;
    [SerializeField]
    private float introDelay;
    [SerializeField]
    private float introDuration;
    [SerializeField]
    private AnimationCurve introCurve;

    [Header("Death Zoom Parameters:")]
    [SerializeField]
    private float deathOrthoSize;
    [SerializeField]
    private float deathDuration;
    [SerializeField]
    private AnimationCurve deathCurve;

    [Header("Boss Zoom Parameters:")]
    [SerializeField]
    private float bossOrthoSize;
    [SerializeField]
    private float bossDuration;
    [SerializeField]
    private AnimationCurve bossCurve;

    private static bool introComplete = false;
    private static bool inBossRoom = false;

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        PlayerController.OnDie += DeathZoom;
    }

    private void OnDisable()
    {
        PlayerController.OnDie -= DeathZoom;
    }

    // Currently intro zoom working through this function, need to setup with CutsceneController
    void Start()
    {
        if (!introComplete)
        {
            CinemachineVirtualCamera.m_Lens.OrthographicSize = introOrthoSize;
            StartCoroutine(IntroLerp(introDelay));
            introComplete = true;
        }
        else
        {
            CinemachineVirtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
        }
    }

    public void IntroZoom()
    {
        if (!introComplete)
        {
            CinemachineVirtualCamera.m_Lens.OrthographicSize = introOrthoSize;
            StartCoroutine(IntroLerp(introDelay));
            introComplete = true;
        }
        else
        {
            CinemachineVirtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
        }
    }

    public void DeathZoom()
    {
        StartCoroutine(DeathLerp(0f));
    }

    public void BossZoom()
    {
        StartCoroutine(BossLerp(0f));
    }

    private IEnumerator IntroLerp(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (timeElapsed < introDuration)
        {
            Zoom(introOrthoSize, defaultOrthoSize, introDuration, introCurve);
            yield return null;
        }

        CinemachineVirtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
    }

    private IEnumerator DeathLerp(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (timeElapsed < introDuration)
        {
            Zoom(CinemachineVirtualCamera.m_Lens.OrthographicSize, deathOrthoSize, deathDuration, deathCurve);
            yield return null;
        }

        CinemachineVirtualCamera.m_Lens.OrthographicSize = deathOrthoSize;
    }

    private IEnumerator BossLerp(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (timeElapsed < introDuration)
        {
            Zoom(defaultOrthoSize, bossOrthoSize, bossDuration, bossCurve);
            yield return null;
        }

        CinemachineVirtualCamera.m_Lens.OrthographicSize = deathOrthoSize;
    }
    private void Zoom(float start, float end, float lerpDuration, AnimationCurve animCurve)
    {
        CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, animCurve.Evaluate(timeElapsed / lerpDuration));
        timeElapsed += Time.deltaTime;
    }

    // public void ZoomIn()
    // {

    // }
}
