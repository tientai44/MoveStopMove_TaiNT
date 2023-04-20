using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : UICanvas
{
    //[SerializeField] private ToggleController _toggleController;
    [SerializeField] private Toggle effectSoundToggle;
    [SerializeField] private Toggle vibrateToggle;
    public override void Open()
    {
        base.Open();
        Time.timeScale = 0;
        effectSoundToggle.isOn = SoundManager2.GetInstance().FlagEffect;
        Debug.Log(VibrateController.GetInstance().Flag);
        vibrateToggle.isOn = VibrateController.GetInstance().Flag;
        
        Debug.Log(vibrateToggle.isOn);
    }
    public void ReturnButton()
    {
        NewUIManager.GetInstance().OpenUI<PlayingMenu>();
        Close(0);
        
    }
    
    public void MusicButton()
    {
        SoundManager2.GetInstance().SwitchSoundBackGround();
    }
    public void SliderVolume(Slider slider)
    {
        SoundManager2.GetInstance().UpdateVolume(slider);
    }
    public void EffectToggleClick()
    {
        SoundManager2.GetInstance().SetMusicEffect(effectSoundToggle.isOn);
        //_toggleController.SwitchAnim();
    }
    public void VibrateToggleClick()
    {
        VibrateController.GetInstance().Flag = vibrateToggle.isOn;
        //_toggleController.SwitchAnim();
    }
    public void HomeButton()
    {
        GameController.GetInstance().Lose();
        NewUIManager.GetInstance().CloseAll();
        NewUIManager.GetInstance().OpenUI<MainMenu>();
        
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        Time.timeScale = 1;
    }
}
