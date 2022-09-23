using UnityEngine;

/// <summary>
/// 動畫效果按鈕
/// </summary>
/// 點擊、懸浮、取消點擊、取消懸浮，皆播放動畫效果
public class AnimationBtn : ccButton
{
    /// <summary>效果動畫機</summary>
    public Animator m_Animator = null;
    /// <summary>被選擇後不會再觸發其餘效果動畫</summary>
    public bool _OnSelectNotPlay = false;
    /// <summary>點擊動畫</summary>
    public bool _OnPress = false;
    /// <summary>是否處於被選擇狀態</summary>
    private bool _bSelect = false;

    public override void f_Init(Sprite Logo, string strText, bool bHover = true)
    {
        base.f_Init(Logo, strText, bHover);

        if (!m_Animator)
        {
            m_Animator = GetComponentInChildren<Animator>();
        }
    }

    public override void f_UnHover()
    {
        base.f_UnHover();
        GameTools.f_PlayAnimator(m_Animator, "UnHover");
    }

    public override void f_Select()
    {
        base.f_Select();
        //播放選取動畫
        if (!_OnPress)
        {
            GameTools.f_PlayAnimator(m_Animator, "OnSelect");
            _bSelect = true;
            CallBack_OnHaver(gameObject, false, null);
        }
        else
        {
            _bSelect = true;
            GameTools.f_AnimPlayCallBack(m_Animator, "OnPress", f_UnPress);
        }
    }

    private void f_UnPress(object obj)
    {
        f_UnSelect();
    }

    public override void f_UnSelect()
    {
        base.f_UnSelect();
        //播放取消選取動畫
        _bSelect = false;
        GameTools.f_PlayAnimator(m_Animator, "UnSelect");
    }

    public override void CallBack_OnHaver(GameObject go, object obj1, object obj2)
    {
        bool bHover = (bool)obj1;
        if (m_HoverImage != null)
        {
            m_HoverImage.gameObject.SetActive(bHover);
        }

        if (_OnSelectNotPlay && _bSelect)
        {
            return;
        }
        if (bHover)//播放懸浮或取消懸浮動畫
        {
            GameTools.f_PlayAnimator(m_Animator, "OnHover");
        }
        else
        {
            GameTools.f_PlayAnimator(m_Animator, "UnHover");
        }
    }

    public override void f_Show()
    {
        base.f_Show();
    }

    public override void f_UnShow()
    {
        //關閉時連同文字也清除
        base.f_UnShow();
        GameUITools.f_SetText(_Text, "");
    }

    public override string f_GetText()
    {
        if (_Text) { return _Text.text; }
        return "";
    }

    public override void f_Interactable()
    {
        base.f_Interactable();
        GameTools.f_PlayAnimator(m_Animator, "OnEnable");
    }

    public override void f_Disteractable()
    {
        base.f_Disteractable();
        GameTools.f_PlayAnimator(m_Animator, "OnDisable");
    }
}
