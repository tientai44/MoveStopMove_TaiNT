using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : UICanvas
{
    public void ReturnButtonClick()
    {
        NewUIManager.GetInstance().OpenUI<MainMenu>();
        Close(0);
    }
}
