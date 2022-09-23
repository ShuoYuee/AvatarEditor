using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DressItem : ListItem
{
    /// <summary>按鈕組件</summary>
    private Button m_Btn;
    /// <summary>動畫機組件</summary>
    private Animator m_Animator;

    /// <summary>ID</summary>
    private int m_Id = 0;
    /// <summary>時裝Type</summary>
    private int iDressID = 0;
    /// <summary>UI是否鎖定</summary>
    private bool bLock = false;

    public OptionEvent OnClick_Event = new OptionEvent();

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="iId">ID</param>
    /// <param name="iDress">時裝Type</param>
    /// <param name="tLock">是否鎖定UI</param>
    public void f_Init(int iId, int iDress, bool tLock = false)
    {
        m_Id = iId;
        iDressID = iDress;
        bLock = tLock;

        //設定Image組件
        if (TryGetComponent(out Image tImage))
        {
            m_Image = tImage;
        }
        else
        {
            m_Image = gameObject.AddComponent<Image>();
        }

        //設定Button組件
        if (TryGetComponent(out Button tButton))
        {
            m_Btn = tButton;
        }
        else
        {
            m_Btn = gameObject.AddComponent<Button>(); 
        }
        m_Btn.transition = Selectable.Transition.Animation;
        m_Btn.animationTriggers.normalTrigger = OnClick_Event.strNormal;
        m_Btn.animationTriggers.highlightedTrigger = OnClick_Event.strHover;
        m_Btn.animationTriggers.pressedTrigger = OnClick_Event.strPress;
        m_Btn.animationTriggers.selectedTrigger = OnClick_Event.strSelect;
        m_Btn.animationTriggers.disabledTrigger = OnClick_Event.strDisabled;

        //設定Animator組件
        if (TryGetComponent(out Animator tAnimator))
        {
            m_Animator = tAnimator;
        }
        else
        {
            MessageBox.DEBUG("換裝種類按鈕 : " + iDressID + " , 未設置按鈕動畫");
        }

        f_CheckLock();
    }

    /// <summary>確認按鈕是否鎖定</summary>
    public void f_CheckLock()
    {
        if (bLock)
        {
            m_Btn.interactable = false;
        }
    }

    /// <summary>設定按鈕是否鎖定</summary>
    public void f_SetLock(bool bSet)
    {
        bLock = bSet;
        f_CheckLock();
    }

    /// <summary>
    /// 設定UI圖像
    /// </summary>
    /// <param name="sprite">圖像</param>
    public void f_SetImage(Sprite sprite)
    {
        if (!sprite)
        {
            MessageBox.DEBUG("未設置按鈕相關圖片 : )" + m_Id);
            return;
        }
        m_Image.sprite = sprite;
    }

    /// <summary>取得時裝Type</summary>
    public int f_GetData()
    {
        return iDressID;
    }

    /// <summary>取得UI物件</summary>
    public GameObject f_GetObj()
    {
        return gameObject;
    }
}

public class OptionEvent
{
    public UnityEvent Normal;
    public UnityEvent HoverEvent;
    public UnityEvent PressEvent;
    public UnityEvent SelectEvent;
    public UnityEvent DisabledEvent;

    public string strNormal = "Normal";
    public string strHover = "Highlighted";
    public string strPress = "Pressed";
    public string strSelect = "Selected";
    public string strDisabled = "Disabled";
}
