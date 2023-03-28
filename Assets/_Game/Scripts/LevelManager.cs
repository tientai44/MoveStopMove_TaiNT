using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : GOSingleton<LevelManager>
{
    private int levelId=0;
    [SerializeField] private LevelController currentLevel;
    [SerializeField] List<GameObject> allLevelPrefabs;

    public LevelController CurrentLevel { get => currentLevel; set => currentLevel = value; }

    void Goto(int level)
    {
        SpawnManager.GetInstance().SpawnBot(10);
    }
    public void LoadLevel()
    {
        GameController.GetInstance().cameraFollow.ResetOffset();
        GameController.GetInstance().ClearObjectSpawn();
        currentLevel = Instantiate(allLevelPrefabs[levelId]).GetComponent<LevelController>();
        GameController.GetInstance().OnInit(currentLevel);
    }
    public void NextLevel()
    {
        Destroy(currentLevel.gameObject);
        levelId++;
        LoadLevel();
    }
    public void Replay()
    {
        Destroy(currentLevel.gameObject);
        LoadLevel();
    }
    public void ClearCurrentLevel()
    {
        Destroy(currentLevel.gameObject);
    }
}
