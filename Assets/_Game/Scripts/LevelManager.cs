using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : GOSingleton<LevelManager>
{
    private int levelId=0;
    [SerializeField] private LevelController currentLevel;
    [SerializeField] List<GameObject> allLevelPrefabs;
    [SerializeField] List<NavMeshData> allNavMeshDatas = new List<NavMeshData>();
    [SerializeField] NavMeshData meshData;
    public LevelController CurrentLevel { get => currentLevel; set => currentLevel = value; }

    void Goto(int level)
    {
        levelId = level;
        LoadLevel();
    }
    public void LoadLevel()
    {
        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        GameController.GetInstance().cameraFollow.ResetOffset();
        GameController.GetInstance().ClearObjectSpawn();
        currentLevel = Instantiate(allLevelPrefabs[levelId]).GetComponent<LevelController>();
        GameController.GetInstance().OnInit(currentLevel);

        NavMesh.RemoveAllNavMeshData();
        meshData = allNavMeshDatas[levelId];
        NavMesh.AddNavMeshData(allNavMeshDatas[levelId]);
    }
    public void LoadBackGround()
    {
        //if(currentLevel != null)
        //{
        //    Destroy(currentLevel.gameObject);
        //}
        levelId = SaveLoadManager.GetInstance().Data1.LevelID;
        GameController.GetInstance().cameraFollow.ResetOffset();
        GameController.GetInstance().ClearObjectSpawn();
        currentLevel = Instantiate(allLevelPrefabs[levelId]).GetComponent<LevelController>();
        //nav mesh
        NavMesh.RemoveAllNavMeshData();
        meshData = allNavMeshDatas[levelId];
        NavMesh.AddNavMeshData(allNavMeshDatas[levelId]);
    }
    

    public void NextLevel()
    {
        Destroy(currentLevel.gameObject);
        levelId++;
        SaveLoadManager.GetInstance().Data1.LevelID = levelId;
        SaveLoadManager.GetInstance().Save();
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
