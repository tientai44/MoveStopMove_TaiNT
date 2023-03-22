using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosController : MonoBehaviour
{
    public List<CharacterController> l_charac = new List<CharacterController>();
    public bool ISHAVEPLAYER = false;
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player" || other.tag == "Bot")
    //    {
    //        isHaveCharacter = true;

    //    }
    //}
    private void Update()
    {
        List<CharacterController> list = new List<CharacterController>();
        foreach(CharacterController c in l_charac)
        {
            if (c.IsDead)
            {
                list.Add(c);
            }
        }
        foreach(CharacterController c in list)
        {
            l_charac.Remove(c);
        }
        ISHAVEPLAYER = l_charac.Count > 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER) || other.CompareTag(Constant.TAG_BOT))
        {
            if(!l_charac.Contains(Cache.GetCharacter(other)))
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
        return l_charac.Count>0;
    }
}
