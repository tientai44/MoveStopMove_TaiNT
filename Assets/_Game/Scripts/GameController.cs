using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : GOSingleton<GameController>
{
    [SerializeField] private int Alive;
    [SerializeField] private List<Transform> l_character = new List<Transform>();
    private int numSpawn;
    public List<Transform> L_character { get => l_character; set => l_character = value; }
    public int NumSpawn { get => numSpawn; set => numSpawn = value; }

    private void Start()
    {
        GetInstance();
        UIManager.GetInstance().SetAliveText(Alive);
        numSpawn = Alive - l_character.Count+1;
    }
    public bool isSpawnEnemy()
    {
        return numSpawn > 0;
    }
    public void UpdateAliveText()
    {
        Alive -= 1;
        UIManager.GetInstance().SetAliveText(Alive);
    }
    
}
