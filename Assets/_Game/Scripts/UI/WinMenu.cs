using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinMenu : UICanvas
{
    [SerializeField] TextMeshProUGUI coinText;
    public override void Open()
    {
        base.Open();
        coinText.text = StaticData.CoinGet.ToString();
    }
    public void NextLevelButton()
    {
        NewUIManager.GetInstance().OpenUI<PlayingMenu>();
        LevelManager.GetInstance().NextLevel();

        Close(0);
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
