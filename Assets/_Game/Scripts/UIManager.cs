using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : GOSingleton<UIManager>
{
    [SerializeField]private TextMeshProUGUI aliveText;

    private void Start()
    {
        GetInstance();
    }
    
    public void SetAliveText(int alive)
    {
        aliveText.text = "Alive : " + alive.ToString();
    }
}
