using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Item : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private GameObject collectEffect;
    public enum ItemTypes { Coin, Health, Energy };
    public ItemTypes CollectibleType;
    private float _currentVolumePrefs;
    private float _currentVolume;
    private void Start()
    {
        _currentVolumePrefs = PlayerPrefs.GetFloat("AudioVolume", 1);
        _currentVolume = Mathf.Log10(_currentVolumePrefs) * 20;
    }

    private static void PlayClipAtPoint
        (AudioClip clip, Vector3 position, float volume = 1.0f, AudioMixerGroup group = null)
    {
        if (clip == null) return;
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource) gameObject.AddComponent(typeof (AudioSource));
        if(group != null)
            audioSource.outputAudioMixerGroup = group;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(gameObject, clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    }
    public void CollectEffect()
    {
        if (collectSound)
            PlayClipAtPoint(collectSound, transform.position,1f,audioMixerGroup);
        
        if (collectEffect)
        {
            var position = transform.position;
            if (CollectibleType == ItemTypes.Energy || CollectibleType == ItemTypes.Health)
            {
                position = new Vector2(position.x, position.y + 1.5f);
            }
           
            Instantiate(collectEffect, new Vector3(position.x,position.y), Quaternion.identity);
        }

        Destroy(transform.parent.gameObject);
        
    }
    
}
