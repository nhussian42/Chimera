using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class OptionsController : MonoBehaviour
{
    [SerializeField]
    private Slider _masterSlider, _musicSlider, _sfxSlider;
    [SerializeField]
    private TextMeshProUGUI masterPercent, musicPercent, sfxPercent;

    [SerializeField]
    private GameObject fullScreenToggle;

    [SerializeField]
    private AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        _masterSlider.value = SaveValues.masterVolume;
        _musicSlider.value = SaveValues.musicVolume;
        _sfxSlider.value = SaveValues.sfxVolume;

        fullScreenToggle.GetComponent<Toggle>().isOn = SaveValues.isFullscreen;
    }
    
    public void MasterVolume()
    {
        masterPercent.text = Mathf.RoundToInt(_masterSlider.value * 100) + "%";
        mixer.SetFloat ("MasterVol", Mathf.Log10 (_masterSlider.value) * 20);
        //AudioManager.Instance.MasterVolume(_masterSlider.value);
        SaveValues.masterVolume = _masterSlider.value;

    }

    public void MusicVolume()
    {
        musicPercent.text = Mathf.RoundToInt(_musicSlider.value * 100) + "%";
        AudioManager.Instance.MusicVolume(_musicSlider.value);
        SaveValues.musicVolume = _musicSlider.value;
    }
    public void SFXVolume()
    {
        sfxPercent.text = Mathf.RoundToInt(_sfxSlider.value * 100) + "%";
        AudioManager.Instance.SFXVolume(_sfxSlider.value); 
        AudioManager.Instance.WorldVolume(_sfxSlider.value);
        SaveValues.sfxVolume = _sfxSlider.value;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveValues.isFullscreen = isFullscreen;
    }
}
