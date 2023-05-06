
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRevive : UICanvas
{
    [SerializeField] TextMeshProUGUI timeLoadtxt;
    [SerializeField] Transform circleTimeTF;
    private float timeLoad;
    private float speed = 360f;
    public override void Setup()
    {
        Debug.Log("SetUp");
        base.Setup();
        timeLoad = 5;
        circleTimeTF.rotation = Quaternion.identity;
    }
    public override void Open()
    {
        NewUIManager.GetInstance().CloseUI<PlayingMenu>();
        base.Open();
        
        
      
    }
    private void Update()
    {
        if (timeLoad > 0)
        {
            circleTimeTF.Rotate(0,0,speed*Time.deltaTime);
            timeLoad -= Time.deltaTime;
            Debug.Log(timeLoad);
            timeLoadtxt.SetText(timeLoad.ToString("F0"));
                       
        }
        if (timeLoad <= 0)
        {
            CloseButton();
        }
    }

    public void ReviveButton()
    {
     
        Close(0);
        NewUIManager.GetInstance().OpenUI<PlayingMenu>();
        GameController.GetInstance().currentPlayer.DeSpawn();
        GameController.GetInstance().GetGameState = GameState.Normal;
        //LevelManager.Ins.OnRevive();
        //UIManager.Ins.OpenUI<UIGameplay>();
    }

    public void CloseButton()
    {
        Close(0);
        GameController.GetInstance().GetGameState = GameState.Normal;

        GameObjectPools.GetInstance().ReturnToPool(CharacterType.Player.ToString(), GameController.GetInstance().currentPlayer.gameObject);

        GameController.GetInstance().Lose();
        //LevelManager.Ins.Fail();
    }
}
