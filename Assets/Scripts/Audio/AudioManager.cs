using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    private StudioListener audioListener;
    private AudioEvents audioEvents;
    public EventInstance CurrentMusic { get; private set; }
    private List<EventInstance> instantiatedEventInstances = new List<EventInstance>();
    private int previousSceneMusicIndex = -1;

    private void OnEnable()
    {
        audioEvents = AudioEvents.Instance;

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

    private void StartNewSceneMusic(int buildIndex)
    {
        audioListener = GameObject.FindObjectOfType<StudioListener>();
        EventInstance newMusicInstance = CurrentMusic;

        if (buildIndex == 0)
        {
            newMusicInstance = CreatePersistentEventInstance(audioEvents.OnMainMenuStarted);
        }
        else if (buildIndex == 1 && previousSceneMusicIndex != buildIndex)
        {
            newMusicInstance = CreatePersistentEventInstance(audioEvents.OnGameplayStarted);
        }
        else if (buildIndex == 1 && previousSceneMusicIndex == buildIndex)
        {
            newMusicInstance = CreatePersistentEventInstance(audioEvents.OnCombatStarted);
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
        CrossFadeMusic(CreateEventInstance(audioEvents.OnGameplayStarted));
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
