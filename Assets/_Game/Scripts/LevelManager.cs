using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : GOSingleton<LevelManager>
{
    [SerializeField] private int currentLevel;
    void Goto(int level)
    {
        SpawnManager.GetInstance().SpawnBot(10);
    }
}
