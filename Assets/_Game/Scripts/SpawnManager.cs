using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : GOSingleton<SpawnManager>
{
    private List<SetType> sets = new List<SetType> { SetType.Set1,SetType.Set2,SetType.Set3,SetType.Set4,SetType.Set5};
    public List<Transform> SpawnBot(int numBot)
    {
        List<Transform> transforms = new List<Transform>();
        PlayerController playerController = GameObjectPools.GetInstance().GetFromPool(CharacterType.Player.ToString(), LevelManager.GetInstance().CurrentLevel.L_SpawnPos[numBot].position).GetComponent<PlayerController>();
        transforms.Add(playerController.transform);
        playerController.OnInit();
        GameController.GetInstance().CurrentPlayer = playerController;
        for (int i = 0; i < numBot; i++)
        {
            if(i>= GameController.GetInstance().L_SpawnBot.Count)
            {
                break;
            }
            GameObject go= GameObjectPools.GetInstance().GetFromPool(CharacterType.Bot.ToString(), LevelManager.GetInstance().CurrentLevel.L_SpawnPos[i].position);
            go.GetComponent<BotController>().ChangeEquipment(GameObjectPools.GetInstance().weapons[Random.Range(0, GameObjectPools.GetInstance().weapons.Count)]);
            go.GetComponent<BotController>().SetFullSet(sets[Random.Range(0,sets.Count)]);
            transforms.Add(go.transform);
        }
        return transforms;
    }
}
