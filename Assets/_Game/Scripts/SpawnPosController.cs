using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosController : MonoBehaviour
{
    bool isHaveCharacter=false;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Bot")
           isHaveCharacter=true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Bot")
            isHaveCharacter = false;
    }
    public bool IsAnyPlayer()
    {
        return isHaveCharacter;
    }
}
