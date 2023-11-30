using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.EventSystems;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private Slider _masterSlider, _musicSlider, _sfxSlider;

    [SerializeField]
    private GameObject SettingsGO;
    [SerializeField]
    private TextMeshProUGUI masterPercent, musicPercent, sfxPercent;

    [SerializeField]
    private GameObject fullScreenToggle;

    private float changeTracker = 0;

    // [SerializeField]
    // private AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        _masterSlider.value = SaveValues.masterVolume;
        _musicSlider.value = SaveValues.musicVolume;
        _sfxSlider.value = SaveValues.sfxVolume;

        fullScreenToggle.GetComponent<Toggle>().isOn = SaveValues.isFullscreen;
    }

    public void SelectSettingsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_masterSlider.gameObject);
    }

    public void ReSelectSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SettingsGO);
    }

    public void MasterVolume()
    {
        masterPercent.text = Mathf.RoundToInt(_masterSlider.value * 100) + "%";
        // mixer.SetFloat ("MasterVol", Mathf.Log10 (_masterSlider.value) * 20);
        AudioManager.SetVolume(AudioManager.Instance.MasterBus, _masterSlider.value);
        SaveValues.masterVolume = _masterSlider.value;

        PlayVolumeChangeSound();
    }

    public void MusicVolume()
    {
        musicPercent.text = Mathf.RoundToInt(_musicSlider.value * 100) + "%";
        AudioManager.SetVolume(AudioManager.Instance.MusicBus, _musicSlider.value);
        SaveValues.musicVolume = _musicSlider.value;

        PlayVolumeChangeSound();
    }
    public void SFXVolume()
    {
        sfxPercent.text = Mathf.RoundToInt(_sfxSlider.value * 100) + "%";
        AudioManager.SetVolume(AudioManager.Instance.SFXBus, _sfxSlider.value);
        SaveValues.sfxVolume = _sfxSlider.value;

        PlayVolumeChangeSound();
    }

    private void PlayVolumeChangeSound()
    {
        if (changeTracker++ > 5)
        {
            AudioManager.PlaySound2D(AudioEvents.Instance.OnMenuButtonHovered);
            changeTracker = 0;
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveValues.isFullscreen = isFullscreen;
    }
}
