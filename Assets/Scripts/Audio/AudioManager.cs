using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private StudioListener audioListener;
    private AudioEvents audioEvents;
    private EventInstance currentMusic;

    private int previousSceneMusicIndex = -1;

    private void Start()
    {
        audioEvents = AudioEvents.Instance;

        // StartNewSceneMusic(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        ChimeraSceneManager.OnSceneSwitched += StartNewSceneMusic;
        FloorManager.AllCreaturesDefeated += FadeOutCombatMusic;
    }

    private void OnDisable()
    {
        ChimeraSceneManager.OnSceneSwitched -= StartNewSceneMusic;
        FloorManager.AllCreaturesDefeated -= FadeOutCombatMusic;
    }
    
    public void PlaySound2D(EventReference audioEvent)
    {
        RuntimeManager.PlayOneShot(audioEvent, audioListener.transform.position);
    }

    public void PlaySound3D(EventReference audioEvent, Vector3 position)
    {
        RuntimeManager.PlayOneShot(audioEvent, position);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void CrossFadeMusic(EventInstance currentInstance, EventInstance newInstance)
    {
        currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentMusic.release();
        currentMusic = newInstance;
        currentMusic.start();
    }

    private void StartNewSceneMusic(int buildIndex)
    {
        audioListener = GameObject.FindObjectOfType<StudioListener>();
        EventInstance newMusicInstance = currentMusic;

        if (buildIndex == 0)
        {
            newMusicInstance = CreateEventInstance(audioEvents.OnMainMenuStarted);
        }
        else if (buildIndex == 1 && previousSceneMusicIndex != buildIndex)
        {
            newMusicInstance = CreateEventInstance(audioEvents.OnGameplayStarted);
        }
        else if (buildIndex == 1 && previousSceneMusicIndex == buildIndex)
        {
            newMusicInstance = CreateEventInstance(audioEvents.OnCombatStarted);
        }
        else
        {
            Debug.LogError($"Could not start new scene music at build index \"{buildIndex}\"");
        }
        
        previousSceneMusicIndex = buildIndex;

        CrossFadeMusic(currentMusic, newMusicInstance);
    }

    private void FadeOutCombatMusic()
    {
        CrossFadeMusic(currentMusic, CreateEventInstance(audioEvents.OnGameplayStarted));
    }
}
