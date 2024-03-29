using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager :GOSingleton<SoundManager>
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource audioBackGround;
    [SerializeField] public AudioClip attackSound;
    [SerializeField] AudioSource audioEffect;
    [SerializeField] public AudioClip killSound;
    private void Start()
    {
        GetInstance();
    }
    public void SwitchMusic(AudioSource audio)
    {
        if (audio.isPlaying)
        {
            audio.Stop();
        }
        else
        {
            audio.Play();
        }
    }
    public void EnableMusicEffect()
    {
        if (audioEffect.volume > 0)
        {
            audioEffect.volume = 0;
        }
        else
        {
            audioEffect.volume = 1;
        }
    }
    public void UpdateMusicVolume(Slider slider )
    {
        audioBackGround.volume = slider.value;
    }
    public void PlayOneShot(AudioClip audioClip)
    {
        audioEffect.PlayOneShot(audioClip); 
    }

}
