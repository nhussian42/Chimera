using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;

    private float targetOrthoSize = 11f;
    
    [SerializeField]
    private float orthoSizeMin, orthoSizeMax;

    [SerializeField]
    private float orthoSizeIncrease = 2f;
    
    [SerializeField]
    private float zoomSpeed = 3f;

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera.m_Lens.OrthographicSize = targetOrthoSize;
        targetOrthoSize = Mathf.Clamp(targetOrthoSize, orthoSizeMin, orthoSizeMax);
    }

    // public void IntroZoom()
    // {

    // }
    // public void DeathZoom()
    // {

    // }
    public void ZoomCamera(float zoomSpeed, float zoomTime)
    {
        targetOrthoSize -= orthoSizeIncrease;
        CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
        StartCoroutine(ZoomDuration(zoomTime));
    }

    // public void ZoomOut()
    // {

    // }

    // public void ZoomIn()
    // {

    // }

    private IEnumerator ZoomDuration(float zoomTime)
    {
        yield return new WaitForSeconds(zoomTime);
        targetOrthoSize += orthoSizeIncrease;
        CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);

        yield return null;
    }
}
