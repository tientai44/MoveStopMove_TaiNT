using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.TextCore.Text;
public enum WeaponHold
{
    AxeHold,
    //Stick,
    KnifeHold,
    BoomerangHold,
    //ArrowHold,
    Candy0Hold,
    //UziHold
};
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
    public List<WeaponType> weapons;
    public List<Pool> poolList = new List<Pool>();
    BulletController bullet;
    Dictionary<string, List<GameObject>> objectPools = new Dictionary<string, List<GameObject>>();
    Dictionary<string, List<GameObject>> activeObjectPools = new Dictionary<string, List<GameObject>>();
    public Dictionary<WeaponType,WeaponHold> weaponHolds = new Dictionary<WeaponType, WeaponHold>();

    void Start()
    {
        GetInstance();
        weapons.Add(WeaponType.Axe);
        weapons.Add(WeaponType.Candy0);
        weapons.Add(WeaponType.Knife);
        weapons.Add(WeaponType.Boomerang);
        weaponHolds[WeaponType.Axe] = WeaponHold.AxeHold;
        weaponHolds[WeaponType.Knife] = WeaponHold.KnifeHold;
        weaponHolds[WeaponType.Boomerang] = WeaponHold.BoomerangHold;
        weaponHolds[WeaponType.Candy0] = WeaponHold.Candy0Hold;
        //weaponHolds[WeaponType.Uzi] = WeaponHold.UziHold;
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
        //LevelManager.GetInstance().LoadLevel();
        //GameController.GetInstance().L_character = SpawnManager.GetInstance().SpawnBot(GameController.GetInstance().numBot);
        
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
            activeObjectPools[tag].Add(go);
            switch(tag)
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
            activeObjectPools[tag].Add(go);
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

                go.GetComponent<BotController>().OnInit();
                BasicReset(tag,go, tempPool);
                break;
            case "Player":
                BasicReset(tag,go,tempPool);
                go.GetComponent<PlayerController>().OnInit();
                
                break;
            case "Stick":
                WeaponReset(tag,go, tempPool);
                break;
            case "Candy0":
                WeaponReset(tag, go, tempPool); ;
                break;
            case "Boomerang":
                WeaponReset(tag, go, tempPool);
                break;
            case "Uzi":
                WeaponReset(tag, go, tempPool);
                break;
            case "Knife":
                WeaponReset(tag, go, tempPool);
                break;
            case "Axe":
                WeaponReset(tag, go, tempPool);
                break;
            default:
                BasicReset(tag, go, tempPool);
                break;
        }   
    }
    public void BasicReset(string tag,GameObject go,Pool tempPool)
    {
        go.transform.rotation = tempPool.poolObjectPrefab.transform.rotation;
        go.transform.localScale = tempPool.poolObjectPrefab.transform.localScale;
        activeObjectPools[tag].Remove(go);
        objectPools[tag].Add(go);
        go.SetActive(false);
    }
    public void WeaponReset(string tag,GameObject go, Pool tempPool)
    {
        go.GetComponent<BulletController>().ResetForce();
        go.GetComponent<BulletController>().Timer = 0;
        BasicReset(tag,go,tempPool);
    }
    public void ClearObjectActive(string tag)
    {
        while (activeObjectPools[tag].Count > 0)
        {
            ReturnToPool(tag, activeObjectPools[tag][0]);
        }
    }
}
