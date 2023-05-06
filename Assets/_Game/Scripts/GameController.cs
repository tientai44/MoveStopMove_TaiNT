using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;
public enum GameState
{
    Wait,Normal
}
public class GameController : GOSingleton<GameController>
{
    [SerializeField] public int Alive;
    [SerializeField] private List<CharacterController> l_character = new List<CharacterController>();
    [SerializeField] private List<Transform> l_SpawnBot = new List<Transform>();
    //public FixedJoystick joystick;
    public FloatingJoystick joystick;
    public CameraFollow cameraFollow;
    public int numBot = 10;
    public int numSpawn;
    public PlayerController currentPlayer;
    public GameState gameState;
    private float timeWait=5f;
    public List<CharacterController> L_character { get => l_character; set => l_character = value; }
    public int NumSpawn { get => numSpawn; set => numSpawn = value; }
    public List<Transform> L_SpawnBot { get => l_SpawnBot; set => l_SpawnBot = value; }
    public PlayerController CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }
    public GameState GetGameState { get => gameState; set => gameState = value; }

    private void Awake()
    {
        //base.Awake();
        SaveLoadManager.GetInstance().OnInit();
        gameState = GameState.Normal;
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }
    private void Start()
    {
        GetInstance();
        
    }
    public void OnInit(LevelController level)
    {
        L_character.Clear();
        Alive = level.Alive;
        numBot = level.NumBot;
        numSpawn = Alive - numBot;
        L_SpawnBot = level.L_SpawnPos;

        L_character = SpawnManager.GetInstance().SpawnBot(numBot);
        //UIManager.GetInstance().SetAliveText(Alive);
        NewUIManager.GetInstance().GetUI<PlayingMenu>().SetAliveText(Alive);

    }
    public bool isSpawnEnemy()
    {
        return numSpawn > 0;
    }
    public void UpdateAliveText()
    {
        Alive -= 1;
        //UIManager.GetInstance().SetAliveText(Alive);
        NewUIManager.GetInstance().GetUI<PlayingMenu>().SetAliveText(Alive);
        if (Alive == 0)
        {
            StartCoroutine(ReadyWin(timeWait));            
            
        }
    }
    public IEnumerator ReadyWin(float time)
    {
        cameraFollow.ZoomIn();
        currentPlayer.Win();
        currentPlayer.ChangeAnim(Constant.ANIM_DANCE);
        yield return new WaitForSeconds(time);
        cameraFollow.ZoomOut();
        Win();

        
    }
    public Vector3 GetRandomSpawnPos()
    {
        //TODO: dung for thay the, bot viec getcomponent o day
        List<Vector3> l_Spawn = new List<Vector3>();
        foreach (Transform tran in l_SpawnBot)
        {
            if (!tran.GetComponent<SpawnPosController>().IsAnyPlayer())
            {
                l_Spawn.Add(tran.position);
            }
        }
        if (l_Spawn.Count > 0)
            return l_Spawn[Random.Range(0, l_Spawn.Count)];
        else
        {
            Debug.Log("No Space Pos");
            return l_SpawnBot[Random.Range(0, l_SpawnBot.Count)].position;
        }
    }
    public void Win()
    {
        GameObjectPools.GetInstance().ClearObjectActive(CharacterType.Bot.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(CharacterType.Player.ToString());
        ClearObjectSpawn();
        //UIManager.GetInstance().DisplayWinPanel();
        NewUIManager.GetInstance().CloseAll();
        NewUIManager.GetInstance().OpenUI<WinMenu>();
        //SaveLoadManager.GetInstance().Data1.Coin += point;
        //SaveLoadManager.GetInstance().Data1.WeaponCurrent = currentWeapon.ToString();
        SaveLoadManager.GetInstance().Save();
        Debug.Log("Now Coin: " + SaveLoadManager.GetInstance().Data1.Coin);
        Debug.Log("Now Weapon: " + SaveLoadManager.GetInstance().Data1.WeaponCurrent);
    }
    public void GoWaiting()
    {
        GetGameState = GameState.Wait;
        NewUIManager.GetInstance().OpenUI<UIRevive>();
    }
    public void Lose()
    {
        GameObjectPools.GetInstance().ClearObjectActive(CharacterType.Bot.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(CharacterType.Player.ToString());
        ClearObjectSpawn();
        //UIManager.GetInstance().DisplayLosePanel();
        NewUIManager.GetInstance().CloseAll();
        NewUIManager.GetInstance().OpenUI<LoseMenu>();
        SaveLoadManager.GetInstance().Save();
        Debug.Log("Now Coin: " + SaveLoadManager.GetInstance().Data1.Coin);
        Debug.Log("Now Weapon: " + SaveLoadManager.GetInstance().Data1.WeaponCurrent);
    }
    public void ClearObjectSpawn()
    {
        GameObjectPools.GetInstance().ClearObjectActive(CharacterType.Bot.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(CharacterType.Player.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(WeaponType.Knife.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(WeaponType.Boomerang.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(WeaponType.Candy0.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(WeaponType.Axe.ToString());
        GameObjectPools.GetInstance().ClearObjectActive(Constant.POINT_TAG);
    }
}
