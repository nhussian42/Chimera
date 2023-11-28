using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;

    private float targetOrthoSize = 11f;
    [Header("Intro Zoom Parameters:")]
    [SerializeField]
    private float introOrthoSizeMin;
    [SerializeField]
    private float introOrthoSizeMax;
    [SerializeField]
    private float introZoomSpeed;
    [Header("Default Zoom Parameters:")]
    [SerializeField]
    private float orthoSizeMin, orthoSizeMax;

    [SerializeField]
    private float orthoSizeIncrease = 2f;
    
    // [SerializeField]
    // private float zoomSpeed = 3f;

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera.m_Lens.OrthographicSize = targetOrthoSize;

        // IntroZoom();
        // targetOrthoSize = Mathf.Clamp(targetOrthoSize, orthoSizeMin, orthoSizeMax);
    }
    
    public void IntroZoom()
    {
        ZoomOut(introOrthoSizeMin, introOrthoSizeMax, introZoomSpeed);
    }

    // public void DeathZoom()
    // {

    // }

    public void ZoomOut(float min, float max, float zoomSpeed)
    {
        float difference = max - min;
        min += difference;

        print(CinemachineVirtualCamera.m_Lens.OrthographicSize);
        CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CinemachineVirtualCamera.m_Lens.OrthographicSize, max, Time.deltaTime * zoomSpeed);
        print(CinemachineVirtualCamera.m_Lens.OrthographicSize);

        // StartCoroutine(ZoomDuration(zoomTime));
    }

    // public void ZoomIn()
    // {

    // }

    public void ZoomCamera(float zoomSpeed, float zoomTime)
    {
        targetOrthoSize -= orthoSizeIncrease;
        CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
        // StartCoroutine(ZoomDuration(zoomTime));
    }
    // private IEnumerator ZoomDuration(float zoomTime)
    // {
    //     yield return new WaitForSeconds(zoomTime);
    //     targetOrthoSize += orthoSizeIncrease;
    //     CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);

    //     yield return null;
    // }
}
