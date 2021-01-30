using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    public AudioMixer mixer;
    public string volumeGroup = "Master";

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(volumeGroup, Mathf.Log10(sliderValue) * 20);
    }

}
