using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateController : GOSingleton<VibrateController>
{
    bool flag = false;// dang bat che do rung hay khong

    public bool Flag { get => flag; set => flag = value; }

    public void Vibrate()
    {
        if (!flag)
        {
            return;
        }
        // Vibrate for half a second
        Handheld.Vibrate();
    }

    public void Vibrate(float duration)
    {
        if (!flag)
        {
            return;
        }
        Debug.Log("vibrate");
        // Vibrate for the specified duration in seconds
        Handheld.Vibrate();
        Invoke("StopVibration", duration);
    }

    private void StopVibration()
    {
        Handheld.Vibrate();
    }
}
