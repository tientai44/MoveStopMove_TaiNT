using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : GOSingleton<SpawnManager>
{
    private List<SetType> sets = new List<SetType> { SetType.Set1, SetType.Set2, SetType.Set3, SetType.Set4, SetType.Set5 };
    private List<HeadType> heads = new List<HeadType> { HeadType.Head1, HeadType.Head2, HeadType.Cowboy, HeadType.Crown, HeadType.Crown, HeadType.HatCap, HeadType.HeadPhone, HeadType.ArrowHead };
    private List<ShieldType> shieldTypes = new List<ShieldType>{ShieldType.Shield1,ShieldType.Shield2};
    
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
            BotController bot= GameObjectPools.GetInstance().GetFromPool(CharacterType.Bot.ToString(), LevelManager.GetInstance().CurrentLevel.L_SpawnPos[i].position).GetComponent<BotController>();

            //random weapon
            bot.ChangeEquipment(GameObjectPools.GetInstance().weapons[Random.Range(0, GameObjectPools.GetInstance().weapons.Count)]);
            int select = Random.Range(0, 2);

            if (select == 0)
            {
                //random skin
                bot.SetFullSet(sets[Random.Range(0, sets.Count)]);
            }
            else
            {
                //random equip
                bot.SetHead(heads[Random.Range(0,heads.Count)]);
                bot.SetPant(GameObjectPools.GetInstance().pantMaterials[Random.Range(0, GameObjectPools.GetInstance().pantMaterials.Count)]);
                bot.SetShield(shieldTypes[Random.Range(0,shieldTypes.Count)]);
            }

            transforms.Add(bot.TF);
        }
        return transforms;
    }
}
