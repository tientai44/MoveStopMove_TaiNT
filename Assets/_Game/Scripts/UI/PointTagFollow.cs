using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointTagFollow : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image BackGround;
    private Transform tf;
    private Vector3 offset = new Vector3(0,2.1f,0);
    public Transform TF
    {
        get
        {
            //tf ??= GetComponent<Transform>();
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    public void SetNameText(string name)
    {
        nameText.text = name;
    }
    public void SetOwner(CharacterController character)
    {
        characterController = character;
        
        Color c = character.ColorSkin.material.color;
        
        if (c != Color.white)
        {
            BackGround.color = c;
            nameText.color = c;
        }
        else
        {
            BackGround.color = Color.magenta;
            nameText.color = Color.magenta;
        }
    }
    private void Update()
    {
        if (characterController == null || !characterController.gameObject.activeSelf) {
            GameObjectPools.GetInstance().ReturnToPool(Constant.POINT_TAG, this.gameObject);
            return; 
        }

        pointText.text = characterController.Point.ToString();
        int num =(int) (characterController.Point / characterController.NumBotToLevelUp);
        //TF.position = characterController.TF.position + offset + num*new Vector3(0,0.4f,0);
        TF.position = characterController.PosPoinTag.position;
    }
}
