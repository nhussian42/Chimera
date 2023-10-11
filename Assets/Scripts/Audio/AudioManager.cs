using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, menuSFXSounds, worldSFXSounds, playerSFXSounds, enemySFXSounds;
    public AudioSource musicSource, menuSFXSource, worldSFXSource, playerSFXSource, enemySFXSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // PlayMusic("DungeonMusic");
    }

    public void PlayMusic(string name) 
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlayMenuSFX(string name)
    {
        Sound s = Array.Find(menuSFXSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            menuSFXSource.PlayOneShot(s.clip);
        }
    }

    public void PlayWorldSFX(string name)
    {
        Sound s = Array.Find(worldSFXSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            worldSFXSource.PlayOneShot(s.clip);
        }
    }
    public void PlayEnemySFX(string name)
    {
        Sound s = Array.Find(enemySFXSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            enemySFXSource.PlayOneShot(s.clip);
        }
    }

   public void PlayPlayerSFX(string name)
    {
        Sound s = Array.Find(playerSFXSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            playerSFXSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleMenuSFX()
    {
        menuSFXSource.mute = !menuSFXSource.mute;
    }

    public void ToggleWorldSFX()
    {
        worldSFXSource.mute = !worldSFXSource.mute;
    }

    public void ToggleEnemySFX()
    {
        enemySFXSource.mute = !enemySFXSource.mute;
    }

    public void TogglePlayerSFX()
    {
        playerSFXSource.mute = !playerSFXSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void MenuSFXVolume(float volume)
    {
        menuSFXSource.volume = volume;
    }
    public void WorldSFXVolume(float volume)
    {
        worldSFXSource.volume = volume;
    }
    public void EnemySFXVolume(float volume)
    {
        enemySFXSource.volume = volume;
    }

    public void PlayerSFXVolume(float volume)
    {
        playerSFXSource.volume = volume;
    }
}
