using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private float timeElapsed = 0f;

    [Header("Zoom To Default Parameters:")]
    [SerializeField]
    private float defaultOrthoSize = 11f;
    [SerializeField]
    private float defaultDelay;
    [SerializeField]
    private float defaultDuration;
    [SerializeField]
    private AnimationCurve defaultCurve;

    [Header("Zoom To Intro Parameters:")]
    [SerializeField]
    private float introOrthoSize;
    [SerializeField]
    private float introDelay;
    [SerializeField]
    private float introDuration;
    [SerializeField]
    private AnimationCurve introCurve;

    [Header("Zoom To Death Parameters:")]
    [SerializeField]
    private float deathOrthoSize;
    [SerializeField]
    private float deathDuration;
    [SerializeField]
    private AnimationCurve deathCurve;

    [Header("Zoom To Boss Parameters:")]
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
        IntroCutsceneTrigger.IntroCutscene += IntroZoom;
        // Replace w/ CutsceneController.IntroZoom += IntroZoom;

        Boss1CutsceneTrigger.Boss1Cutscene += BossZoom;
        // Replace w/ CutsceneController.BossZoom += BossZoom;


        Grolfino.BossDead += DefaultZoom;
        // BossDead.BossDead += DefaultZoom;
    }

    private void OnDisable()
    {
        PlayerController.OnDie -= DeathZoom;
        IntroCutsceneTrigger.IntroCutscene -= IntroZoom;
        // Replace w/ CutsceneController.IntroZoom -= IntroZoom;

        Boss1CutsceneTrigger.Boss1Cutscene -= BossZoom;
        // Replace w/ CutsceneController.BossZoom -= BossZoom;

        Grolfino.BossDead -= DefaultZoom;
        // BossDead.BossDead -= DefaultZoom;

    }

    // Currently intro zoom working through this function, need to setup with CutsceneController
    void Start()
    {
        if (introComplete)
        {
            CinemachineVirtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
        }
        else
        {
            CinemachineVirtualCamera.m_Lens.OrthographicSize = introOrthoSize;

        }
    }
    public void IntroZoom()
    {
        if (!introComplete)
        {
            StartCoroutine(ZoomLerp(introDelay, introOrthoSize, defaultOrthoSize, introDuration, introCurve));
            introComplete = true;
        }
        else
        {
            CinemachineVirtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
        }
    }

    public void DeathZoom()
    {
        StartCoroutine(ZoomLerp(0f, CinemachineVirtualCamera.m_Lens.OrthographicSize, deathOrthoSize, deathDuration, deathCurve));
    }

    public void BossZoom()
    {
        StartCoroutine(ZoomLerp(0f, defaultOrthoSize, bossOrthoSize, bossDuration, bossCurve));
    }

    public void DefaultZoom()
    {
        // For some reason this isn't working even though it should
        StartCoroutine(ZoomLerp(defaultDelay, CinemachineVirtualCamera.m_Lens.OrthographicSize, defaultOrthoSize, defaultDuration, defaultCurve));

    }

    private IEnumerator ZoomLerp(float delay, float startOrthoSize, float endOrthoSize, float duration, AnimationCurve animCurve)
    {
        yield return new WaitForSeconds(delay);

        while (timeElapsed < duration)
        {
            Zoom(startOrthoSize, endOrthoSize, duration, animCurve);
            yield return null;
        }

        CinemachineVirtualCamera.m_Lens.OrthographicSize = endOrthoSize;
    }
    private void Zoom(float start, float end, float lerpDuration, AnimationCurve animCurve)
    {
        CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, animCurve.Evaluate(timeElapsed / lerpDuration));
        timeElapsed += Time.deltaTime;
    }
}
