using ccU3DEngine;
using UnityEngine;
using UnityEngine.UI;
using SR;

public class GameUITools
{

    #region 快速初始化UI

    public static Text f_InitCommon_Text(GameObject UIObj)
    {
        return UIObj.GetComponent<Text>();
    }

    public static Image f_InitCommon_Image(GameObject UIObj)
    {
        return UIObj.GetComponent<Image>();
    }

    #endregion

    /// <summary>
    /// 设置按钮的多语言,切换多语言UI时由程序自己维护多语言
    /// </summary>
    /// <param name="oBtn">按钮对象</param>
    /// <param name="szLanuageKey">多语言Key</param>
    public static void f_SetButtonText(GameObject oBtn, string szLanuageKey)
    {
        Text tText = oBtn.transform.Find("Text").GetComponent<Text>();
        tText.text = LanguageManager.GetInstance().f_GetText(szLanuageKey);
    }

    /// <summary>
    /// 设置按钮的多语言，切换多语言UI时由系统自动维护多语言
    /// </summary>
    /// <param name="oBtn">按钮对象</param>
    /// <param name="szLanuageKey">多语言Key</param>
    public static void f_SetButtonTextForUpdateText(GameObject oBtn, string szLanuageKey)
    {
        Text tText = oBtn.transform.Find("Text").GetComponent<Text>();
        UpdateText tUpdateText = tText.gameObject.GetComponent<UpdateText>();
        if (tUpdateText == null)
        {
            tUpdateText = tText.gameObject.AddComponent<UpdateText>();
        }
        tUpdateText.m_strLanuageKey = szLanuageKey;
        tUpdateText.f_Update();
    }

    #region Image
    /// <summary>
    /// 開關圖片組件
    /// </summary>
    /// <param name="tImage">圖片組件</param>
    /// <param name="bSet">開關</param>
    /// <returns></returns>
    public static bool f_SetImage(Image tImage, bool bSet)
    {
        if (tImage)
        {
            tImage.enabled = bSet;
        }
        return tImage;
    }

    public static bool f_SetImageRay(Image tImage, bool bSet)
    {
        if (tImage)
        {
            tImage.raycastTarget = bSet;
        }
        return tImage;
    }

    public static bool f_SetImage(Image tImage, Sprite sprite)
    {
        if (tImage)
        {
            tImage.sprite = sprite;
        }
        return tImage;
    }
    #endregion

    /// <summary>
    /// 設定文字組件
    /// </summary>
    /// <param name="tText">文字組件</param>
    /// <param name="strSet">文字內容</param>
    /// <returns></returns>
    public static bool f_SetText(Text tText, string strSet)
    {
        if (tText != null)
        {
            tText.text = strSet;
        }
        return tText != null;
    }

    #region CreateUI
    public static ScrollDlg f_CreateScrollDlg(GameObject Obj)
    {
        ScrollDlg tScrollDlg = Obj.GetComponent<ScrollDlg>();
        if (tScrollDlg == null)
        {
            tScrollDlg = Obj.AddComponent<ScrollDlg>();
        }
        return tScrollDlg;
    }

    public static ScrollDlg_Multilayer f_CreateScrollDlgMultilayer(GameObject Obj)
    {
        ScrollDlg_Multilayer tScrollDlg_Multilayer = Obj.GetComponent<ScrollDlg_Multilayer>();
        if (tScrollDlg_Multilayer == null)
        {
            tScrollDlg_Multilayer = Obj.AddComponent<ScrollDlg_Multilayer>();
        }
        return tScrollDlg_Multilayer;
    }

    public static GridPageView f_CreateGridPage(GameObject Obj)
    {
        GridPageView tGridPageView = Obj.GetComponent<GridPageView>();
        if (!tGridPageView)
        {
            tGridPageView = Obj.AddComponent<GridPageView>();
        }
        return tGridPageView;
    }
    #endregion

    #region 攝影機
    public static GameObject f_Set3DBG(GameObject oBG)
    {//設定攝影機狀態
        if (oBG == null) { return null; }
        oBG.SetActive(false);

        GameObject _BGUI = null;
        _BGUI = GameObject.Instantiate(oBG, GameMain.GetInstance().f_GetGameTable());
        _BGUI.SetActive(true);
        Canvas canvas = _BGUI.GetComponentInChildren<Canvas>();
        if (canvas)
        {
            canvas.worldCamera = GameMain.GetInstance().f_GetMapObj(EM_Camera.BGCamera.ToString()).GetComponent<Camera>();
        }
        return _BGUI;
    }

    /// <summary>
    /// 設定SRCamera
    /// </summary>
    /// <param name="Target">目標座標</param>
    public static void f_SetSRCamera(Transform Target)
    {
        SR_Camera _Camera = GameMain.GetInstance().f_GetMapObj(EM_Camera.SRCamera.ToString()).GetComponent<SR_Camera>();
        if (Target == null)
        {
            MessageBox.ASSERT("SR_Camera_Target為空！");
            return;
        }
        if (_Camera)
        {
            _Camera.SetTarget(Target);
        }
        else
        {
            MessageBox.ASSERT("主攝影機未設置SR_Camera組件");
        }
    }

    public static void f_SetSRCamera(SR_Camera Camera, Transform Target)
    {
        if (Target == null)
        {
            MessageBox.ASSERT("SR_Camera_Target為空！");
            return;
        }
        if (Camera != null)
        {
            Camera.SetTarget(Target);
        }
        else
        {
            MessageBox.ASSERT("主攝影機未設置SR_Camera組件");
        }
    }
    #endregion
}