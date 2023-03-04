using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : GOSingleton<BulletPool>
{
    [SerializeField] private GameObject Prefabs;
    [SerializeField] private List<GameObject> pools = new List<GameObject>();
    [SerializeField] private List<GameObject> activePools = new List<GameObject>();
    public GameObject GetGameObject(Vector3 pos)
    {
        if (pools.Count == 0)
        {
            GameObject go = Instantiate(Prefabs, pos, Prefabs.transform.rotation);
            go.SetActive(true);
            activePools.Add(go);
            return go;
        }
        else
        {
            GameObject go = pools[0];
            go.SetActive(true);
            go.transform.position = pos;
            go.transform.rotation = Prefabs.transform.rotation;
            pools.RemoveAt(0);
            activePools.Add(go);
            return go;
        }
        
    }
    

    public void ReturnGameObject(GameObject go)
    {
        go.transform.rotation = Prefabs.transform.rotation;
        go.GetComponent<BulletController>().ResetForce();
        go.GetComponent<BulletController>().Timer = 0;
        activePools.Remove(go);
        pools.Add(go);
        go.SetActive(false);
    }

    public void ClearActive()
    {
        while (activePools.Count > 0)
        {
            ReturnGameObject(activePools[0]);
        }
    }
}
