using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    private StudioListener audioListener;
    public EventInstance CurrentMusic { get; private set; }
    private List<EventInstance> instantiatedEventInstances = new List<EventInstance>();
    private int previousSceneMusicIndex = -1;

    [SerializeField] [Range(0, 1)] public float masterVolume = 0.5f;
    [SerializeField] [Range(0, 1)] public float musicVolume = 1f;
    [SerializeField] [Range(0, 1)] public float SFXVolume = 1f;

    public Bus MasterBus { get; private set; }
    public Bus MusicBus { get; private set; }
    public Bus SFXBus { get; private set; }

    protected override void Init()
    {
        MasterBus = RuntimeManager.GetBus("bus:/");
        MusicBus = RuntimeManager.GetBus("Bus:/Music");
        SFXBus = RuntimeManager.GetBus("Bus:/SFX");
    }

    private void OnEnable()
    {
        ChimeraSceneManager.OnSceneSwitched += StartNewSceneMusic;
        FloorManager.AllCreaturesDefeated += FadeOutCombatMusic;
        FloorManager.LeaveRoom += CleanUpInstances;
    }

    private void OnDisable()
    {
        ChimeraSceneManager.OnSceneSwitched -= StartNewSceneMusic;
        FloorManager.AllCreaturesDefeated -= FadeOutCombatMusic;
        FloorManager.LeaveRoom -= CleanUpInstances;
    }
    
    public static void PlaySound2D(EventReference audioEvent)
    {
        RuntimeManager.PlayOneShot(audioEvent);
    }

    public static void PlaySound3D(EventReference audioEvent, Vector3 position)
    {
        RuntimeManager.PlayOneShot(audioEvent, position);
    }

    public static void SetFloat(EventInstance instance, string parameterName, float parameterValue)
    {
        instance.setParameterByName(parameterName, parameterValue);
    }

    public static void SetBool(EventInstance instance, string parameterName, bool parameterBool)
    {
        float parameterValue = parameterBool ? 1 : 0;
        instance.setParameterByName(parameterName, parameterValue);
    }

    public static void SetVolume(Bus volumeBus, float volume)
    {
        volumeBus.setVolume(volume);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        instantiatedEventInstances.Add(eventInstance);
        return eventInstance;
    }

    public EventInstance CreatePersistentEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void RemoveEventInstance(EventInstance instanceToRemove)
    {
        instantiatedEventInstances.Remove(instanceToRemove);
        instanceToRemove.release();
    }

    public void CrossFadeMusic(EventInstance newInstance)
    {
        CurrentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        CurrentMusic.release();
        CurrentMusic = newInstance;
        CurrentMusic.start();
    }

    public void CrossFadeMusicOut()
    {
        CurrentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        CurrentMusic.release();
    }

    private void StartNewSceneMusic(int buildIndex)
    {
        audioListener = GameObject.FindObjectOfType<StudioListener>();
        EventInstance newMusicInstance = CurrentMusic;

        if (buildIndex == 0)
        {
            newMusicInstance = CreatePersistentEventInstance(AudioEvents.Instance.OnMainMenuStarted);
        }
        else if (buildIndex == 1 && previousSceneMusicIndex != buildIndex)
        {
            newMusicInstance = CreatePersistentEventInstance(AudioEvents.Instance.OnGameplayStarted);
        }
        else if (buildIndex == 1 && previousSceneMusicIndex == buildIndex)
        {
            newMusicInstance = CreatePersistentEventInstance(AudioEvents.Instance.OnCombatStarted);
        }
        else
        {
            Debug.LogError($"Could not start new scene music at build index \"{buildIndex}\"");
        }
        
        previousSceneMusicIndex = buildIndex;

        CrossFadeMusic(newMusicInstance);
    }

    private void FadeOutCombatMusic()
    {
        CrossFadeMusic(CreateEventInstance(AudioEvents.Instance.OnGameplayStarted));
    }

    private void CleanUpInstances()
    {
        foreach (EventInstance instance in instantiatedEventInstances)
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
        instantiatedEventInstances.Clear();
    }
}
