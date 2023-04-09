using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{

    public List<CharacterController> l_charac = new List<CharacterController>();
    public bool ISHAVEPLAYER = false;
    [SerializeField] Material material;
   
    private void Update()
    {
        List<CharacterController> list = new List<CharacterController>();
        foreach (CharacterController c in l_charac)
        {
            if (c.IsDead)
            {
                list.Add(c);
            }
        }
        foreach (CharacterController c in list)
        {
            l_charac.Remove(c);
        }
        ISHAVEPLAYER = l_charac.Count > 0;
        if (ISHAVEPLAYER)
        {
            if(material != null)
            {
                Color c = material.color;
                if (c.a == 0.3f) return;
                c.a = 0.3f;
                material.color = c;
            }
        }
        else
        {
            if (material != null)
            {
                Color c = material.color;
                if (c.a == 1f) return;
                c.a = 1f;
                material.color = c;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER) || other.CompareTag(Constant.TAG_BOT))
        {
            if (!l_charac.Contains(Cache.GetCharacter(other)))
                l_charac.Add(Cache.GetCharacter(other));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER) || other.CompareTag(Constant.TAG_BOT))
        {
            if (l_charac.Contains(Cache.GetCharacter(other)))
                l_charac.Remove(Cache.GetCharacter(other));
        }
    }
    public bool IsAnyPlayer()
    {
        return l_charac.Count > 0;
    }


}
