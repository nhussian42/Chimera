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

    [Header("[CUTSCENE REWORK NEEDED] Zoom To Boss Parameters:")]
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
        IntroCutsceneTrigger.IntroWakeUp += IntroZoom;
        Boss1CutsceneTrigger.Boss1Cutscene += BossZoom;


        Grolfino.BossDead += DefaultZoom;
    }

    private void OnDisable()
    {
        PlayerController.OnDie -= DeathZoom;
        IntroCutsceneTrigger.IntroWakeUp -= IntroZoom;
        Boss1CutsceneTrigger.Boss1Cutscene -= BossZoom;

        Grolfino.BossDead -= DefaultZoom;
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
        // BUG: Snapping to 7 and lerping to 6 in boss room
        StartCoroutine(ZoomLerp(0f, CinemachineVirtualCamera.m_Lens.OrthographicSize, deathOrthoSize, deathDuration, deathCurve));
    }

    public void BossZoom()
    {
        StartCoroutine(ZoomLerp(0f, defaultOrthoSize, bossOrthoSize, bossDuration, bossCurve));
    }

    public void DefaultZoom()
    {
        // BUG: Snapping to 11.5 and lerping to 11 in boss room
        StartCoroutine(ZoomLerp(defaultDelay, CinemachineVirtualCamera.m_Lens.OrthographicSize, defaultOrthoSize, defaultDuration, defaultCurve));

    }

    private IEnumerator ZoomLerp(float delay, float start, float end, float duration, AnimationCurve animCurve)
    {
        // print(CinemachineVirtualCamera.m_Lens.OrthographicSize);
        yield return new WaitForSeconds(delay);
        // print(CinemachineVirtualCamera.m_Lens.OrthographicSize);

        while (timeElapsed < duration)
        {        
            // print(CinemachineVirtualCamera.m_Lens.OrthographicSize);

            CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, animCurve.Evaluate(timeElapsed / duration));
            timeElapsed += Time.deltaTime;            
            yield return null;
        }

        CinemachineVirtualCamera.m_Lens.OrthographicSize = end;
    }
}
