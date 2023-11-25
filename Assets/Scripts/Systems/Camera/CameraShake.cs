using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;

    [SerializeField]
    private float heavyAttackShake = 3f;
    [SerializeField]
    private float lightAttackShake = 1f;
    [SerializeField]
    private float takeHeavyDamageShake = 3f;
    [SerializeField]
    private float takeLightDamageShake = 1f;

    [SerializeField]
    private float heavyAttackShakeTime = 0.2f;
    [SerializeField]
    private float lightAttackShakeTime = 0.2f;
    [SerializeField]
    private float takeHeavyDamageShakeTime = 0.2f;
    [SerializeField]
    private float takeLightDamageShakeTime = 0.2f;

    // TODO camera shake for heavy enemies/boss attacks
    private float timer;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        StopShake();
    }

    public void LightAttackShake()
    {
        ShakeCamera(lightAttackShake, lightAttackShakeTime);
    }

    public void HeavyAttackShake()
    {
        ShakeCamera(heavyAttackShake, heavyAttackShakeTime);
    }

    public void TakeLightDmgShake()
    {
        ShakeCamera(takeLightDamageShake, takeLightDamageShakeTime);
    }

    public void TakeHeavyDmgShake()
    {
        ShakeCamera(takeHeavyDamageShake, takeHeavyDamageShakeTime);
    }
    public void ShakeCamera(float shakeIntensity, float shakeTime)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0f;

        timer = 0;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopShake();
            }
        }
    }
}
