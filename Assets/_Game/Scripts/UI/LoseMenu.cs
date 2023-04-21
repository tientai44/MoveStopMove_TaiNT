using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseMenu : UICanvas
{
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI coinText;
    public override void Open()
    {
        base.Open();
        killText.text = "You're killed by "+StaticData.Killer;
        coinText.text = StaticData.CoinGet.ToString();
    }
    public void ReplayButton()
    {
        NewUIManager.GetInstance().OpenUI<PlayingMenu>();
        LevelManager.GetInstance().Replay();
        Close(0);
    }
    public void QuitButton()
    {
        NewUIManager.GetInstance().OpenUI<MainMenu>();
        Close(0);
    }
}
