using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : GOSingleton<UIManager>
{
    [SerializeField]private TextMeshProUGUI aliveText;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    private void Start()
    {
        GetInstance();
    }
    
    public void SetAliveText(int alive)
    {
        aliveText.text = "Alive : " + alive.ToString();
    }
    public void DisplayPlayPanel()
    {
        Time.timeScale = 1;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void DisplayWinPanel()
    {
        Time.timeScale = 0;
        playPanel.SetActive(false);
        winPanel.SetActive(true);
    }
    public void DisplayLosePanel()
    {
        Time.timeScale = 0;
        playPanel.SetActive(false);
        losePanel.SetActive(true);
    }
}
