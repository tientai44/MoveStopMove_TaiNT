using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Indicator : MonoBehaviour
{

    public GameObject indicatorPrefab; 
    private List<IndicatorController> indicators = new List<IndicatorController>();
    [SerializeField] private Canvas indicatorCanvas;
    [SerializeField] private float offset = 20f;
    
    void Update()
    {
        if (GameController.GetInstance().L_character.Count <= 0)
        {
            return;
        }
        // Xoa het cac indicator cu
        foreach (IndicatorController indicator in indicators)
        {
            //Destroy(indicator.gameObject);
            indicator.gameObject.SetActive(false);
        }
        //indicators.Clear();
        int count = 0;
        // Tao indicator moi
        foreach (CharacterController character in GameController.GetInstance().L_character)
        {
            if (character.GetComponent<CharacterController>().IsDead || !character.gameObject.activeSelf)
            {
                continue;
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(character.TF.position);
            if (screenPos.z < 0 ||screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
            {
                
                // Tim vi tri dat indicator
                float x, y, z= 0;
                if (screenPos.z < 0)
                {
                    screenPos.y = Screen.height - screenPos.y;
                    screenPos.x = Screen.width - screenPos.x;
                }
                if (screenPos.x < 0)
                {
                    x = offset;
                }
                else if(screenPos.x>Screen.width)
                {
                    x = Screen.width - offset;
                }
                else
                {
                    x = screenPos.x;
                }
                if (screenPos.y < 0)
                {
                    y = offset;
                }
                else if(screenPos.y>Screen.height)
                {
                    y = Screen.height - offset;
                }
                else
                {
                    y=screenPos.y;
                }
                Vector2 middlePoint = new Vector2(Screen.width / 2, Screen.height / 2);
                Vector2 direction = new Vector2(screenPos.x - middlePoint.x / 2, screenPos.y - middlePoint.y);

                float goc = Mathf.Acos(CalCosin(direction.x,direction.y,0,1))/Mathf.PI*180;
                if (x > Screen.width / 2)
                {
                    goc = -goc;
                }

                Vector3 posSpawn = new Vector3(x,y,0);
                IndicatorController indicator;
                if (indicators.Count > count)
                {
                    indicator = indicators[count];
                    indicator.gameObject.SetActive(true);
                    indicator.TF.position = posSpawn;
                    indicator.ResetRotate();
                    count++;
                }
                else
                {
                    indicator = Instantiate(indicatorPrefab, posSpawn, Quaternion.identity, indicatorCanvas.transform).GetComponent<IndicatorController>();
                    indicators.Add(indicator);
                    count=int.MaxValue;
                }
                indicator.Rotate(new Vector3(0, 0, goc));
                Color c = character.ColorSkin.material.color;

                if (c != Color.white)
                {
                    indicator.SetColor(c);
                }
                else
                {
                    indicator.SetColor(Color.magenta);
                }
                indicator.SetPoint(character.Point);
                

            }
        }
    }
    private float CalCosin(float x1,float y1,float x2,float y2)
    {
        return (x1 * x2 + y1 * y2) / (Mathf.Sqrt(x1 * x1 + y1 * y1) * Mathf.Sqrt(x2 * x2 + y2 * y2));
    }

}