using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
   [SerializeField] private AudioMixer audioMixer;
   private Slider _slider;
   private float _currentVolume = 1f;
   private void Start()
   {
      _slider = GetComponentInChildren<Slider>();
      _currentVolume = PlayerPrefs.GetFloat("AudioVolume", 1);
      _slider.value = _currentVolume;
   }

   public void SetVolume(float volume)
   {
      _currentVolume = Mathf.Log10(volume) * 20;
      audioMixer.SetFloat("volume",_currentVolume);
      PlayerPrefs.SetFloat("AudioVolume", volume);
   }
}
