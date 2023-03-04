using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : GOSingleton<SpawnManager>
{
    public List<Transform> SpawnBot(int numBot)
    {
        List<Transform> transforms = new List<Transform>();
        transforms.Add(GameObjectPools.GetInstance().GetFromPool("Player", GameController.GetInstance().L_SpawnBot[numBot].position).transform);
        for (int i = 0; i < numBot; i++)
        {
            if(i>= GameController.GetInstance().L_SpawnBot.Count)
            {
                break;
            }
            GameObject go= GameObjectPools.GetInstance().GetFromPool("Bot", GameController.GetInstance().L_SpawnBot[i].position);
            transforms.Add(go.transform);
        }
        return transforms;
    }
}
