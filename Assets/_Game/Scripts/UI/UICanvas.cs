using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    //public bool IsAvoidBackKey = false;
    public bool IsDestroyOnClose = false;
    ParticleSystem effectButton;
    protected RectTransform m_RectTransform;
    private Animator m_Animator;
    string currentAnimName;
    private float m_OffsetY = 0;

    private void Start()
    {
        OnInit();
    }

    //Init default Canvas
    //khoi tao gia tri canvas
    protected virtual void OnInit()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Animator = GetComponent<Animator>();

        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = m_RectTransform.offsetMin;
            Vector2 rightTop = m_RectTransform.offsetMax;
            rightTop.y = -100f;
            m_RectTransform.offsetMax = rightTop;
            leftBottom.y = 0f;
            m_RectTransform.offsetMin = leftBottom;
            m_OffsetY = 100f;
        }
    }

    //Setup canvas to avoid flash UI
    //set up mac dinh cho UI de tranh truong hop bi nhay' hinh
    public virtual void Setup()
    {
        NewUIManager.GetInstance().AddBackUI(this);
        NewUIManager.GetInstance().PushBackAction(this, BackKey);
    }


    //back key in android device
    //back key danh cho android
    public virtual void BackKey()
    {

    }

    //Open canvas
    //mo canvas
    public virtual void Open()
    {
        gameObject.SetActive(true);
        ChangeAnim(Constant.ANIM_OPEN);
    }

    //close canvas directly
    //dong truc tiep, ngay lap tuc
    public virtual void CloseDirectly()
    {
        NewUIManager.GetInstance().RemoveBackUI(this);
        gameObject.SetActive(false);
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }

    }

    //close canvas with delay time, used to anim UI action
    //dong canvas sau mot khoang thoi gian delay
    public virtual void Close(float delayTime)
    {
  
        Invoke(nameof(CloseDirectly), delayTime);
        ChangeAnim(Constant.ANIM_CLOSE);
    }
    public void UnEnableEffectButton()
    {
        if (effectButton != null)
        {
            GameObjectPools.GetInstance().ReturnToPool("ClickButtonEffect", effectButton.gameObject);
        }
    } 
    public void EnableEffectButton(Button button)
    {
        effectButton = GameObjectPools.GetInstance().GetFromPool("ClickButtonEffect", button.transform.position).GetComponent<ParticleSystem>();
        effectButton.transform.SetParent(FindObjectOfType<CanvasVFX>().transform);
        effectButton.Play();
    }
    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName && m_Animator!=null)
        {
            m_Animator.ResetTrigger(animName);

            currentAnimName = animName;

            m_Animator.SetTrigger(currentAnimName);
        }
    }
}
