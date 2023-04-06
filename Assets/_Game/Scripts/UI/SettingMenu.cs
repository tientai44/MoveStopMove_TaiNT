using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : UICanvas
{
    //[SerializeField] private ToggleController _toggleController;
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
    public void CheckBoxEffect()
    {
        SoundManager2.GetInstance().SwitchMusicEffect();
        //_toggleController.SwitchAnim();
    }
    public void HomeButton()
    {
        GameController.GetInstance().Lose();
        NewUIManager.GetInstance().CloseAll();
        NewUIManager.GetInstance().OpenUI<MainMenu>();
        
    }
}
