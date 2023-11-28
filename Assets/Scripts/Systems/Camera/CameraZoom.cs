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
    private float introLerpDuration;

    [Header("Death Zoom Parameters:")]
    [SerializeField]
    private float deathOrthoSize;
    [SerializeField]
    private float deathLerpDuration;

    [Header("Boss Zoom Parameters:")]
    [SerializeField]
    private float bossOrthoSize;
    [SerializeField]
    private float bossDelay;
    [SerializeField]
    private float bossLerpDuration;

    private static bool introComplete = false;
    private static bool inBossRoom = false;

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

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

    // If we decide to make a controller for transitions/cutscenes
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
        StartCoroutine(DeathLerp());
    }

    private IEnumerator IntroLerp(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (timeElapsed < introLerpDuration)
        {
            Zoom(introOrthoSize, defaultOrthoSize, introLerpDuration);
            yield return null;
        }

        CinemachineVirtualCamera.m_Lens.OrthographicSize = defaultOrthoSize;
    }

    private IEnumerator DeathLerp()
    {
        // yield return new WaitForSeconds(delay);

        while (timeElapsed < introLerpDuration)
        {
            Zoom(defaultOrthoSize, deathOrthoSize, deathLerpDuration);
            yield return null;
        }

        CinemachineVirtualCamera.m_Lens.OrthographicSize = deathOrthoSize;
    }
    private void Zoom(float start, float end, float lerpDuration)
    {
        CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, timeElapsed / lerpDuration);
        timeElapsed += Time.deltaTime;
    }

    // public void ZoomIn()
    // {

    // }
}
