using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : GOSingleton<SpawnManager>
{
    private List<SetType> setTypes = new List<SetType> { SetType.Set1, SetType.Set2, SetType.Set3, SetType.Set4, SetType.Set5 };
    private List<HeadType> headTypes = new List<HeadType> { HeadType.Head1, HeadType.Head2, HeadType.Cowboy, HeadType.Crown, HeadType.Crown, HeadType.HatCap, HeadType.HeadPhone, HeadType.ArrowHead };
    private List<ShieldType> shieldTypes = new List<ShieldType>{ShieldType.Shield1,ShieldType.Shield2};
    
    public List<CharacterController> SpawnBot(int numBot)
    {
        List<CharacterController> characters = new List<CharacterController>();
        PlayerController playerController = GameObjectPools.GetInstance().GetFromPool(CharacterType.Player.ToString(), LevelManager.GetInstance().CurrentLevel.L_SpawnPos[numBot].position).GetComponent<PlayerController>();
        characters.Add(playerController);
        playerController.OnInit();
        playerController.Point = 0;
        GameObjectPools.GetInstance().GetFromPool(Constant.POINT_TAG, playerController.TF.position).GetComponent<PointTagFollow>().SetOwner(playerController);
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
            int select = Random.Range(0, 5);

            if (select == 0)
            {
                //random skin
                bot.SetFullSet(setTypes[Random.Range(0, setTypes.Count)]);
            }
            else
            {
                //random equip
                bot.SetHead(headTypes[Random.Range(0,headTypes.Count)]);
                bot.SetPant(GameObjectPools.GetInstance().pantMaterials[Random.Range(0, GameObjectPools.GetInstance().pantMaterials.Count)]);
                bot.SetShield(shieldTypes[Random.Range(0,shieldTypes.Count)]);
                bot.SetColorSkin(GameObjectPools.GetInstance().characterMaterial[Random.Range(0, GameObjectPools.GetInstance().characterMaterial.Count)]);
            }
            GameObjectPools.GetInstance().GetFromPool(Constant.POINT_TAG, playerController.TF.position).GetComponent<PointTagFollow>().SetOwner(bot);
            bot.Point = 0;
            characters.Add(bot);
        }
        return characters;
    }
}
