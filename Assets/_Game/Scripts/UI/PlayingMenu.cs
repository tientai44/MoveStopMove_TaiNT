using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayingMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI aliveText;
    public override void Open()
    {
        base.Open();
    }
    public void SettingButton()
    {
        NewUIManager.GetInstance().OpenUI<SettingMenu>();
        Close(0);
    }
    public void SetAliveText(int alive)
    {
        aliveText.text = "Alive : " + alive.ToString();
    }

}
