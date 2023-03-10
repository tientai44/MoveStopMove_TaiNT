using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameObjectPools : GOSingleton<GameObjectPools>
{
    [System.Serializable]
    public class Pool
    {
        public GameObject poolObjectPrefab;
        public int poolCount;
        public bool canGrow;
        public string tag;
    }
    public List<Pool> poolList = new List<Pool>();
    BulletController bullet;
    Dictionary<string, List<GameObject>> objectPools = new Dictionary<string, List<GameObject>>();
    Dictionary<string, List<GameObject>> activeObjectPools = new Dictionary<string, List<GameObject>>();
    void Start()
    {
        GetInstance();
        foreach (Pool pool in poolList)
        {
            List<GameObject> l = new List<GameObject>();
            for (int i = 0; i < pool.poolCount; i++)
            {
                GameObject obj = Instantiate(pool.poolObjectPrefab);
                obj.SetActive(false);
                l.Add(obj);
                
            }
            objectPools[pool.tag] = l;
            activeObjectPools[pool.tag] = new List<GameObject>();
            //objectPools.Add(pool.tag, l);
        }
        GameController.GetInstance().L_character = SpawnManager.GetInstance().SpawnBot(GameController.GetInstance().numBot);
        
    }

    public GameObject GetFromPool(string tag)
    {
        Pool tempPool = new Pool();
        foreach (Pool pool in poolList)
        {
            if (tag == pool.tag)
            {
                tempPool = pool;
                break;
            }
        }

        if (objectPools[tag].Count > 0)
        {
            GameObject go = objectPools[tag][0];
            objectPools[tag].RemoveAt(0);
            return go;
        }
        else if (tempPool.canGrow)
        {
            GameObject obj = Instantiate(tempPool.poolObjectPrefab);

            return obj;
        }
        else
        {
            return null;
        }
    }
    public GameObject GetFromPool(string tag,Vector3 pos)
    {
        Pool tempPool = new Pool();
        
        foreach (Pool pool in poolList)
        {
            if (tag == pool.tag)
            {
                tempPool = pool;
                break;
            }
        }


        if (objectPools[tag].Count > 0)
        {
            GameObject go = objectPools[tag][0];
            go.transform.position = pos;
            go.SetActive(true);
            objectPools[tag].RemoveAt(0);
            switch (tag)
            {
                case "Boomerang":
                    go.GetComponent<BoomerangController>().SetFirstPoint(pos);
                    break;
               
            }
            return go;
        }
        else if (tempPool.canGrow)
        {
            GameObject go = Instantiate(tempPool.poolObjectPrefab);
            go.transform.position=pos;
            go.SetActive(true);
            objectPools[tag].Remove(go);
            switch (tag)
            {
                case "Boomerang":
                    go.GetComponent<BoomerangController>().SetFirstPoint(pos);
                    break;
                
            }
            return go;
        }
        else
        {
            return null;
        }
    }
    public void ReturnToPool(string tag, GameObject go)
    {
        Pool tempPool = new Pool();
        foreach (Pool pool in poolList)
        {
            if (tag == pool.tag)
            {
                tempPool = pool;
                break;
            }
        }
        switch (tag) {
            case "Bot":
                go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
                activeObjectPools[tag].Remove(go);
                objectPools[tag].Add(go);
                go.SetActive(false);
                break;
            case "Player":
                go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
                activeObjectPools[tag].Remove(go);
                objectPools[tag].Add(go);
                go.SetActive(false);
                break;
            case "Stick":
                go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
                go.GetComponent<BulletController>().ResetForce();
                go.GetComponent<BulletController>().Timer = 0;
                activeObjectPools[tag].Remove(go);
                objectPools[tag].Add(go);
                go.SetActive(false);
                break;
            case "Candy1":
                go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
                go.GetComponent<BulletController>().ResetForce();
                go.GetComponent<BulletController>().Timer = 0;
                activeObjectPools[tag].Remove(go);
                objectPools[tag].Add(go);
                go.SetActive(false);
                break;
            case "Boomerang":
                go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
                go.GetComponent<BulletController>().ResetForce();
                go.GetComponent<BulletController>().Timer = 0;
                activeObjectPools[tag].Remove(go);
                objectPools[tag].Add(go);
                go.SetActive(false);
                break;
            case "Uzi":
                go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
                go.GetComponent<BulletController>().ResetForce();
                go.GetComponent<BulletController>().Timer = 0;
                activeObjectPools[tag].Remove(go);
                objectPools[tag].Add(go);
                go.SetActive(false);
                break;
            case "Knife":
                go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
                go.GetComponent<BulletController>().ResetForce();
                go.GetComponent<BulletController>().Timer = 0;
                activeObjectPools[tag].Remove(go);
                objectPools[tag].Add(go);
                go.SetActive(false);
                break;

        }
    }
}
