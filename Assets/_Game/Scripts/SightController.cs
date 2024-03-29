using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{
    [SerializeField]private CharacterController characterOwner;
    CharacterController character;
    private void OnTriggerEnter(Collider other)
    {
        //Cache getcomponent
        if (other.TryGetComponent<CharacterController>(out character))
        {
            if (characterOwner is BotController)
            {
                (characterOwner as BotController).targetFollow = character;
            }
            characterOwner.L_AttackTarget.Add(character);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out character))
        {
            characterOwner.L_AttackTarget.Remove(character);
        }
    }
}
