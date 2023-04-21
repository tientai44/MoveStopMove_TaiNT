using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UICanvas
{
    [SerializeField]private TextMeshProUGUI coinText;
    [SerializeField] private Toggle effectSoundtoggle;
    [SerializeField] private Toggle vibrateToggle;
    [SerializeField] Image processImg;
    int count = 0;
    public override void Open()
    {
        base.Open();
        SetCoinText(SaveLoadManager.GetInstance().Data1.Coin);
        
        processImg.fillAmount = (SaveLoadManager.GetInstance().Data1.LevelID + 1f) / 5;
        if ( GameController.GetInstance().currentPlayer == null||!GameController.GetInstance().currentPlayer.gameObject.activeSelf )
            GameController.GetInstance().currentPlayer= GameObjectPools.GetInstance().GetFromPool(CharacterType.Player.ToString(),Vector3.zero).GetComponent<PlayerController>();
        GameController.GetInstance().currentPlayer.OnInit();
        GameController.GetInstance().currentPlayer.Point = 0;
        GameController.GetInstance().cameraFollow.ZoomIn();
        GameController.GetInstance().cameraFollow.Offset += new Vector3(0, 2, -1);
        //if (SoundManager2.GetInstance().FlagEffect)
        //    count = 0;
        //else
        //    count = 0;
        count = 0;
        effectSoundtoggle.isOn = !SoundManager2.GetInstance().FlagEffect;
        vibrateToggle.isOn = VibrateController.GetInstance().Flag;
        
        
    }
    public TextMeshProUGUI CoinText { get => coinText; set => coinText = value; }
    public void QuestionButtonClick()
    {
        NewUIManager.GetInstance().OpenUI<HowToPlay>();
        Close(0.5f);
    }
    public void EffectSoundButton()
    {
        Debug.Log(count);
        if (count == 0)
        {
            count += 1;
            return;
        }
        SoundManager2.GetInstance().SetMusicEffect(!effectSoundtoggle.isOn);
        SoundManager2.GetInstance().PlaySound(Constant.ATTACK_MUSIC_NAME);
    }
    public void VibrateToggleClick()
    {
        VibrateController.GetInstance().Flag = vibrateToggle.isOn;
        VibrateController.GetInstance().Vibrate(0.1f);
    }
    public void WeaponButton()
    {
        NewUIManager.GetInstance().OpenUI<WeaponMenu>();
        Close(0);
    }
    public void PlayButton()
    {
        NewUIManager.GetInstance().OpenUI<PlayingMenu>();
        LevelManager.GetInstance().LoadLevel();
        Close(0);
    }
    public void SettingButton()
    {

        NewUIManager.GetInstance().OpenUI<SettingMenu>();
        Close(0);
    }
    public void SkinButton()
    {
        NewUIManager.GetInstance().OpenUI<SkinMenu>();
        Close(0);
    }
    public void SetCoinText(int coin)
    {
        coinText.text =  coin.ToString();
    }
    public override void Close(float delayTime)
    {
        base.Close(delayTime);
        
    }
    
}
