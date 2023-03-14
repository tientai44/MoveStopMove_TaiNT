using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosController : MonoBehaviour
{
    bool isHaveCharacter=false;
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
            if(c is BotController)
            {
                if(c.GetComponent<BotController>().CurrentState is DieState)
                {
                    list.Add(c);
                }
            }
            else
            {
                if (c.GetComponent<PlayerController>().MyState is PlayerState.Death)
                {
                    list.Add(c);
                }
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
        if (other.tag == "Player" || other.tag == "Bot")
        {
            if(!l_charac.Contains(other.GetComponent<CharacterController>()))
                l_charac.Add(other.GetComponent<CharacterController>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Bot")
        {
            isHaveCharacter = false;
            if (l_charac.Contains(other.GetComponent<CharacterController>()))
                l_charac.Remove(other.GetComponent<CharacterController>());
        }
    }
    public bool IsAnyPlayer()
    {
        return l_charac.Count>0;
    }
}
