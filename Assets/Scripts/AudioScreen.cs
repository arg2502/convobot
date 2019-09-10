using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioScreen : MonoBehaviour {

    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;

    private void Start()
    {
        float newValue;
        musicGroup.audioMixer.GetFloat("MusicVol", out newValue);
        musicSlider.value = GetPowVal(newValue);

        sfxGroup.audioMixer.GetFloat("SFXVol", out newValue);
        sfxSlider.value = GetPowVal(newValue);
    }

    public void SetMusicLevel(float sliderValue)
    {
        musicGroup.audioMixer.SetFloat("MusicVol", GetLogVal(sliderValue));
    }

    public void SetSFXLevel(float sliderValue)
    {
        sfxGroup.audioMixer.SetFloat("SFXVol", GetLogVal(sliderValue));
    }

    float GetLogVal(float value)
    {
        return Mathf.Log10(value) * 20f; 
    }

    float GetPowVal(float value)
    {
        return Mathf.Pow(10, value / 20f);
    }
}
