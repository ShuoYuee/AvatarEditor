using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// UI消息定义
/// </summary>
public class UIMessageDef : BaseUIMessageDef
{

    public static string StartGame = "StartGame";

    public static string PAUSEGAME = "PAUSEGAME";
    public static string RESUMEGAME = "RESUMEGAME";

    //public static string BATTLEUIUPDATE = "BATTLEUIUPDATE";

    public static string PlayerJionGame = "PlayerJionGame";
    public static string PlayerLeaveGame = "PlayerLeaveGame";

    /// <summary>
    /// 资源加载进度
    /// </summary>
    public static string UI_RESOURCELOADPROGRESS = "UI_RESOURCELOADPROGRESS";
    /// <summary>
    /// 资源加载完消息
    /// </summary>
    public static string UI_RESOURCECOMPLETE = "UI_RESOURCECOMPLETE";

    public static string UI_UpdateInitProgress = "UI_UpdateInitProgress";
    /// <summary>
    /// 開關GameMain主UI
    /// </summary>
    public static string UI_GameMainUI = "UI_GameMainUI";
    /// <summary>
    /// 開關Load畫面
    /// </summary>
    public static string UI_GameLoad = "UI_GameLoad";
    /// <summary>
    /// 開關Wait畫面
    /// </summary>
    public static string UI_GameWait = "UI_GameWait";
    /// <summary>
    /// 開關GameTable
    /// </summary>
    public static string UI_GameMap = "UI_GameMap";
    /// <summary>
    /// 開關車隊頁面背景
    /// </summary>
    public static string UI_Guild_MainBG = "UI_Guild_MainBG";
    /// <summary>
    /// 跳轉車隊頁面菜單
    /// </summary>
    public static string UI_Guild_MainMenu = "UI_Guild_MainMenu";

    //按鈕回調
    public static string UI_MenuBtn = "UI_MenuBtn";
    public static string UI_ReturnMenuBtn = "UI_ReturnMenuBtn";
    public static string UI_ShopBuyBtn = "UI_ShopBuyBtn";
    public static string UI_ShopInforBtn = "UI_ShopInforBtn";
    public static string UI_DeformerBtn = "UI_DeformerBtn";

#if UNITY_EDITOR
    public static string UI_SceneView = "UI_SceneView";
#endif
}
