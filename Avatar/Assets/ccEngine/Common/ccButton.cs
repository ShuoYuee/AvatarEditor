using UnityEngine;
using UnityEngine.UI;
using ccU3DEngine;

public class ccButton : MonoBehaviour
{
    public bool m_bHover = true;

    public Sprite m_HoverSprite = null;
    public Sprite m_PressSprite = null;

    public Image m_HoverImage;
    public Image m_SelectImage;
    public Image m_LogoImage;
    public Text _Text;
    public Transform _BG;

    protected bool _bSel = false;
    protected bool _bInteractable = true;
    public bool m_Select { get { return _bSel; } }

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (m_HoverImage != null)
        {
            m_HoverImage.gameObject.SetActive(false);
        }
        if (m_SelectImage != null)
        {
            m_SelectImage.gameObject.SetActive(false);
        }
    }

    public virtual void f_Init(Sprite Logo, string strText, bool bHover = true)
    {
        f_Init(strText, bHover);
        GameUITools.f_SetImage(m_LogoImage, Logo);
    }

    public virtual void f_InitInfor(Sprite Logo, string strText)
    {
        f_Init(Logo);
        f_InitText(strText);
    }

    public virtual void f_Init(Sprite Logo)
    {
        GameUITools.f_SetImage(m_LogoImage, Logo);
    }

    public virtual void f_InitText(string strText)
    {
        if (_Text != null)
        {
            _Text.text = strText;
        }
    }

    public virtual void f_Init(string strText, bool bHover = true)
    {
        Init();
        m_bHover = bHover;
        if (bHover)
        {
            ccUIEventListener.Get(gameObject).f_RegHover(CallBack_OnHaver, null);
        }
        if (_Text != null)
        {
            _Text.text = strText;
        }
    }

    public virtual void CallBack_OnHaver(GameObject go, object obj1, object obj2)
    {
        /*if (m_HoverImage != null)
        {
            //bool bHover = (bool)obj1;
            m_HoverImage.gameObject.SetActive(false);
        }*/
        if (!_bInteractable) { return; }
        bool bHover = (bool)obj1;
        if (m_HoverImage != null)
        {
            m_HoverImage.gameObject.SetActive(bHover);
        }
    }

    public virtual void f_UnHover()
    {
        if (m_HoverImage != null)
        {
            m_HoverImage.gameObject.SetActive(false);
        }
    }

    public virtual void f_Select()
    {
        _bSel = true;
        if (m_SelectImage != null)
        {
            m_SelectImage.gameObject.SetActive(true);
        }
    }

    public virtual void f_UnSelect()
    {
        _bSel = false;
        if (m_SelectImage != null)
        {
            m_SelectImage.gameObject.SetActive(false);
        }
    }

    public virtual void f_Show()
    {
        GameUITools.f_SetImage(m_LogoImage, true);
    }

    public virtual void f_UnShow()
    {
        GameUITools.f_SetImage(m_LogoImage, false);
    }

    public virtual void f_ShowBG()
    {
        _BG.gameObject.SetActive(true);
    }

    public virtual void f_UnShowBG()
    {
        _BG.gameObject.SetActive(false);
    }

    public virtual string f_GetText()
    {
        return _Text.text;
    }

    public virtual void f_Interactable()
    {
        _bInteractable = true;
    }

    public virtual void f_Disteractable()
    {
        _bInteractable = false;
    }
}