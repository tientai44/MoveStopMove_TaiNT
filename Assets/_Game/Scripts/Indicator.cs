using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{

    public GameObject indicatorPrefab;
    private List<GameObject> indicators = new List<GameObject>();
    [SerializeField] private Canvas indicatorCanvas;
    [SerializeField] private float offset = 20f;
    void Update()
    {
        if (GameController.GetInstance().L_character.Count <= 0)
        {
            return;
        }
        // Xoá t?t c? các ?i?m hi?n th?
        foreach (GameObject indicator in indicators)
        {
            Destroy(indicator);
        }
        indicators.Clear();

        // T?o các ?i?m hi?n th? cho các ??i t??ng trong màn hình
        foreach (Transform trans in GameController.GetInstance().L_character)
        {
            if(trans.GetComponent<CharacterController>().IsDead || !trans.gameObject.activeSelf )
            {
                continue;
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(trans.position);
            if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
            {
                float x, y, z= 0;
                if (screenPos.z < 0)
                {
                    z = 0;
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


                Vector3 posSpawn = new Vector3(x,y,z);
                GameObject indicator = Instantiate(indicatorPrefab, posSpawn, Quaternion.identity, indicatorCanvas.transform) as GameObject;
                indicator.transform.Rotate(new Vector3(0,0,90));
                indicators.Add(indicator);

            }
        }
    }
    private double CalCosin(float x1,float y1,float x2,float y2)
    {
        return (x1 * x2 + y1 * y2) / (Math.Sqrt(x1 * x1 + y1 * y1) * Math.Sqrt(x2 * x2 + y2 * y2));
    }

}