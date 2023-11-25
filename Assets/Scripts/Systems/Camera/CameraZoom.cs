using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private float targetOrthoSize = 11f;
    float orthoSizeIncrease = 2f;
    float zoomSpeed = 3f;

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        CinemachineVirtualCamera.m_Lens.OrthographicSize = targetOrthoSize;
    }
    public void Zoom(bool zoom)
    {
        if (zoom)
        {
            targetOrthoSize -= orthoSizeIncrease;
            CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
        }
        else
        {
            targetOrthoSize += orthoSizeIncrease;
            CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
        }
    }
}
