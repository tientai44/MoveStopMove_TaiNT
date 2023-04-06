using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator m_Animator;
    string currentAnimName;
 

    void Awake()
    {
        Debug.Log("Awake");
        currentAnimName = "enable";
        m_Animator = GetComponent<Animator>();
    }
    void OnInit()
    {
        if (currentAnimName=="enable")
        {
            ChangeAnim("enable");
        }
        else
        {
            ChangeAnim("disable");
        }
    }
    public void ChangeAnim(string animName)
    {
        Debug.Log(animName);    
        if (currentAnimName != animName && m_Animator != null)
        {
            m_Animator.ResetTrigger(animName);

            currentAnimName = animName;

            m_Animator.SetTrigger(currentAnimName);
        }
    }
    public void SwitchAnim()
    {
        if(currentAnimName == "enable")
        {
            ChangeAnim("disable");
        }
        else
        {
            ChangeAnim("enable");
        }
    }
}
