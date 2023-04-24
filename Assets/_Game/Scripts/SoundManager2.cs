using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager2 : GOSingleton<SoundManager2>
{ 
    public AudioMixer audioMixer; // Tranh viec am thanh de nhau
    public AudioSource audioBackGround; // Luu am thanh nen
    bool flagEffect = true; // Kiem tra bat tat am thanh
    public List<AudioSource> hiteffect; // Danh sach sound effect

    public bool FlagEffect { get => flagEffect; set => flagEffect = value; }

    public void Awake()
    {
        Init(80,80);
    }

    public void Init(float musicVolume, float sfxvolume)
    {
        audioMixer.SetFloat(Constant.BACKGROUND_VOLUME, musicVolume);
        audioMixer.SetFloat(Constant.EFFECT_VOLUME, sfxvolume);
        foreach(AudioSource x in hiteffect)
        {
            Debug.Log(x.clip.name);
        }
    }

    public void PlaySound(string name)// Phat mot am thanh co ten duoc truyen vao
    {
        if (!flagEffect)
        {
            // Tat dang tat nhac thi khong lam gi ca
            return;
        }
        // Tim nhac co ten giong voi bien truyen vao va chay am thanh
        for (int i = 0; i < hiteffect.Count; i++)
            if (hiteffect[i].clip.name == name)
                hiteffect[i].Play();
    }
    public void StopSound(string name)
    {
        // Dung de tat am thanh
        for (int i = 0; i < hiteffect.Count; i++)
            if (hiteffect[i].clip.name == name)
                hiteffect[i].Stop();
    }
    public void SwitchSoundBackGround()
    {
          // Dang bat thi tat , dang tat thi bat
          if (audioBackGround.isPlaying)
              audioBackGround.Stop();
          else
              audioBackGround.Play();
    }
    public void ChangeVolumeBackground(float volume)
    {
        //Chinh volume background
        audioMixer.SetFloat("BackgroundVolume", volume);
    }

    public void ChangeVolumeEffect(float volume)
    {
        //Chinh volume effect
        audioMixer.SetFloat("EffectVolume", volume);
    }
    public void SwitchMusicEffect()
    {
        //Tat bat sound effect
        flagEffect=!flagEffect;
    }
    public void SetMusicEffect(bool isOn)
    {
        flagEffect = isOn;
    }
    public void UpdateVolume( Slider slider)
    {
        //chinh volume
        audioBackGround.volume = slider.value;
    }
}
