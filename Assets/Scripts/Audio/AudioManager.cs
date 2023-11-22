using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : Singleton<AudioManager>
{
    private StudioListener audioListener;
    private AudioEvents audioEvents;
    private EventInstance currentMusic;
    private int previousSceneMusicIndex = -1;

    private void OnEnable()
    {
        audioEvents = AudioEvents.Instance;

        ChimeraSceneManager.OnSceneSwitched += StartNewSceneMusic;
        FloorManager.AllCreaturesDefeated += FadeOutCombatMusic;
    }

    private void OnDisable()
    {
        ChimeraSceneManager.OnSceneSwitched -= StartNewSceneMusic;
        FloorManager.AllCreaturesDefeated -= FadeOutCombatMusic;
    }
    
    public static void PlaySound2D(EventReference audioEvent)
    {
        RuntimeManager.PlayOneShot(audioEvent);
    }

    public static void PlaySound3D(EventReference audioEvent, Vector3 position)
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
