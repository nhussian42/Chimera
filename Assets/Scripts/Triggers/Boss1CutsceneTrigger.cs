using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Boss1CutsceneTrigger : MonoBehaviour
{
    private bool _triggered;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject gate;
    [SerializeField] private float gateCloseTime;
    [SerializeField] private float timeUntilPlayerMovementRestored;
    [SerializeField] private float bossSpawnDelay;
    private BossRoom bossRoom;
    [SerializeField] private BossHealthBarUI bossHPBar;

    // private Collider cutsceneTrigger;

    public static Action Boss1Cutscene;

    void Awake()
    {
        // cutsceneTrigger = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        bossRoom = GetComponentInParent<BossRoom>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!_triggered && other.gameObject.GetComponent<PlayerController>() != null)
        {
            _triggered = true;
            
            Boss1Cutscene?.Invoke();
            AudioManager.Instance.CrossFadeMusicOut();
            StartCoroutine(CloseGate(gateCloseTime));
            Invoke("SpawnBoss", bossSpawnDelay);
            
            Debug.Log("Ran in trigger");
            PlayerController.Instance.DisableAllDefaultControls();
            Invoke("ResumePlayerControls", timeUntilPlayerMovementRestored);
        }
    }

    private void SpawnBoss()
    {
        bossRoom.SpawnBoss();
        PlayBossMusic();
    }

    private IEnumerator CloseGate(float closeTime)
    {
        yield return new WaitForSeconds(0.3f);
        AudioManager.PlaySound3D(AudioEvents.Instance.OnBossDoorOpened, transform.position);
        float timer = 0;
        float startPosY = gate.transform.position.y;
        while (timer < closeTime)
        {
            timer += Time.deltaTime;

            gate.transform.position = new Vector3(gate.transform.position.x, Mathf.Lerp(startPosY, 0, timer / closeTime), gate.transform.position.z);
            yield return null;
        }
        yield return null;
    }

    private void PlayBossMusic()
    {
        AudioManager.Instance.CrossFadeMusic(AudioManager.Instance.CreateEventInstance(AudioEvents.Instance.OnDungeonBossStarted));
    }

    private void ResumePlayerControls()
    {
        // print("Player input reenabled");
        PlayerController.Instance.EnableAllDefaultControls();
        var bossBar = Instantiate(bossHPBar);
        bossBar.entered = true;
        bossRoom.bossSpawned = true;
    }
}
