using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{
    [SerializeField] Image indicatorImage;
    [SerializeField] Image pointBG;
    [SerializeField] TextMeshProUGUI pointText;
    Transform tf;
    Transform indicatorImgtf;
    Transform pointBGtf;
    public Transform TF{
        get {
            if(tf==null)
                tf = transform;
            return tf; 
        }
    }
    public Transform indicatorImgTF
    {
        get
        {
            if (indicatorImgtf == null)
                indicatorImgtf = indicatorImage.transform;
            return indicatorImgtf;
        }
    }
    public Transform pointBGTF
    {
        get
        {
            if (pointBGtf == null)
                pointBGtf = pointBG.transform;
            return pointBGtf;
        }
    }
    public void Rotate(Vector3 vector3)
    {
        TF.Rotate(vector3);
        pointBGTF.Rotate(-vector3);
    }
    public void ResetRotate()
    {
        TF.rotation = Quaternion.identity;
        pointBGTF.rotation = Quaternion.identity;
    }
    public void SetColor(Color color)
    {
        indicatorImage.color = color;
        pointBG.color = color;
    }
    public void SetPoint(int point)
    {
        pointText.text = point.ToString();
    }
}
