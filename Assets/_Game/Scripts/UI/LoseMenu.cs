using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseMenu : UICanvas
{
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI rankText;
    public override void Open()
    {
        base.Open();
        killText.text = "You're killed by "+StaticData.Killer;
        coinText.text = StaticData.CoinGet.ToString();
        rankText.text = "#" + StaticData.Rank.ToString();
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
