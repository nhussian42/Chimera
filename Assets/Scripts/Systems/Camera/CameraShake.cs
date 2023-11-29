using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : Singleton<CameraShake>
{
    // public CameraZoom cameraZoom;
    // [SerializeField] private CinemachineBrain _cinemachineBrain;
    // private CinemachineStateDrivenCamera _cinemachineStateDrivenCamera;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    [Header("Player Camera Shake:")]
    [SerializeField]
    private float heavyAttackShake;
    [SerializeField]
    private float heavyAttackShakeTime;
    [SerializeField]
    private float lightAttackShake;
    [SerializeField]
    private float lightAttackShakeTime;
    [SerializeField]
    private float takeHeavyDamageShake;
    [SerializeField]
    private float takeHeavyDamageShakeTime;
    [SerializeField]
    private float takeLightDamageShake;
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
    private CinemachineBasicMultiChannelPerlin cbmcp;

    protected override void Init()
    {
        // _cinemachineStateDrivenCamera = GetComponent<CinemachineStateDrivenCamera>();
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
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
        // cameraZoom.ZoomCamera(heavyAttackShake, heavyAttackShakeTime);
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
        //CinemachineVirtualCamera vc = GetActiveCamera(_cinemachineStateDrivenCamera);
        //CinemachineBasicMultiChannelPerlin cbmcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // Debug.Log(cbmcp.gameObject.name);
        CinemachineBasicMultiChannelPerlin cbmcp = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = shakeIntensity;

        StartCoroutine(ShakeDuration(shakeTime));
    }

    private IEnumerator ShakeDuration(float shakeTime)
    {
        yield return new WaitForSeconds(shakeTime);
        StopShake();
        yield return null;
    }

    private void ConditionalShakeCamera(float shakeIntensity, bool condition)
    {
        if (condition)
        {
            // CinemachineVirtualCamera vc = GetActiveCamera(_cinemachineStateDrivenCamera);
            // CinemachineBasicMultiChannelPerlin cbmcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            CinemachineBasicMultiChannelPerlin cbmcp = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmcp.m_AmplitudeGain = shakeIntensity;
        }
        else
        {
            StopShake();
        }
    }

    public void StopShake()
    {
        // Debug.Log("camera s");
        // CinemachineVirtualCamera vc = GetActiveCamera(_cinemachineStateDrivenCamera);
        // CinemachineBasicMultiChannelPerlin cbmcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CinemachineBasicMultiChannelPerlin cbmcp = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0f;
    }

    //  private CinemachineVirtualCamera GetActiveCamera(CinemachineStateDrivenCamera stateCamera)
    //  {
    //      foreach (CinemachineVirtualCamera child in stateCamera.GetComponentsInChildren<CinemachineVirtualCamera>())
    //      {
    //          if (child.isActiveAndEnabled)
    //          {
    //              return child;
    //          }
    //      }
    //      return null;
    //  }

    //  private IEnumerator WaitForActiveCamera()
    //  {
    //      yield return null;
    //
    //  }
}
