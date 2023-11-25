using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : Singleton<CameraShake>
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;

    [Header("Player Camera Shake:")]
    [SerializeField]
    private float heavyAttackShake;
    [SerializeField]
    private float lightAttackShake;
    [SerializeField]
    private float takeHeavyDamageShake;
    [SerializeField]
    private float takeLightDamageShake;

    [SerializeField]
    private float heavyAttackShakeTime;
    [SerializeField]
    private float lightAttackShakeTime;
    [SerializeField]
    private float takeHeavyDamageShakeTime;
    [SerializeField]
    private float takeLightDamageShakeTime;

    [Header("Creature Camera Shake:")]
    [SerializeField]
    private float creatureAttackShake;
    [SerializeField]
    private float creatureAttackShakeTime;
    
    [SerializeField]
    private float creatureBurrowShake;
    [SerializeField]
    private float bossAttackShake;
    [SerializeField]
    private float bossAttackShakeTime;
    [SerializeField]
    private float bossBurrowShake;


    // TODO camera shake for heavy enemies/boss attacks
    private float timer;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    protected override void Init()
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

    public void CreatureAttackShake()
    {
        ShakeCamera(creatureAttackShake, creatureAttackShakeTime);
    }

    public void BossAttackShake()
    {
        ShakeCamera(bossAttackShake, bossAttackShakeTime);
    }

    public void CreatureBurrowShake(bool isBurrow)
    {
        ConditionalShakeCamera(creatureBurrowShake, isBurrow);
    }

    public void BossBurrowShake(bool isBurrow)
    {
        ConditionalShakeCamera(bossBurrowShake, isBurrow);
    }
    
    private void ShakeCamera(float shakeIntensity, float shakeTime)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    private void ConditionalShakeCamera(float shakeIntensity, bool condition)
    {
        if (condition)
        {
            CinemachineBasicMultiChannelPerlin cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmcp.m_AmplitudeGain = shakeIntensity;
        }
        else
        {
            CinemachineBasicMultiChannelPerlin cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmcp.m_AmplitudeGain = 0f;
        }
    }

    private void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0f;

        timer = 0;
    }

    private void Update()
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
