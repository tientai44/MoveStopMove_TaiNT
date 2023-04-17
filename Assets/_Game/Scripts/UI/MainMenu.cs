using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UICanvas
{
    [SerializeField]private TextMeshProUGUI coinText;
    public override void Open()
    {
        base.Open();
        SetCoinText(SaveLoadManager.GetInstance().Data1.Coin);
        if( GameController.GetInstance().currentPlayer == null||!GameController.GetInstance().currentPlayer.gameObject.activeSelf )
            GameController.GetInstance().currentPlayer= GameObjectPools.GetInstance().GetFromPool(CharacterType.Player.ToString(),Vector3.zero).GetComponent<PlayerController>();
        GameController.GetInstance().currentPlayer.OnInit();
    }
    public TextMeshProUGUI CoinText { get => coinText; set => coinText = value; }
    
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
