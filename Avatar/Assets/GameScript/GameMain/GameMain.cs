using UnityEngine;
using ccU3DEngine;
using System.Collections.Generic;

public class GameMain : MonoBehaviour
{
    [Header("角色相關")]
    /// <summary>扮裝角色控制器</summary>
    public DressRoleControlV3 m_DressRoleControl;
    /// <summary>角色初始位置</summary>
    public Transform DressRole_InitPos;

    [Header("主攝影機")]
    /// <summary>主場景攝影機</summary>
    public Transform m_CameraObj;

    [Header("主場景物件")]
    public GameObject m_GameTable;
    [Header("Unicorn_Work")]
    public GameObject m_Unicorn_Work;

    [Header("啟用編輯器模擬")]
    [Tooltip("若啟用則時裝讀取DressEditorData數據，\n否則讀取AvaterPart.csv數據")]
    public bool m_UseEditor = false;
    [Header("啟用測試登入")]
    [Tooltip("啟用後跳過登入階段")]
    public bool m_UseAutoLogin = false;
    [Header("啟用測試數據")]
    [Tooltip("啟用後使用假數據")]
    public bool m_UseTestData = false;
    private GameLevelControll _GameLevelControll = new GameLevelControll();
    public Data_Pool m_DataPool = new Data_Pool();

    /// <summary>場景物件</summary>
    private Dictionary<string, GameObject> _dirGameObj = new Dictionary<string, GameObject>();

    private static GameMain _Instance = null;
    public static GameMain GetInstance()
    {
        return _Instance;
    }


    void Awake()
    {
        _Instance = this;
        ccUIManage.GetInstance().f_SendMsg("UI_GameMain", BaseUIMessageDef.UI_OPEN, null, true);
    }

    void Start()
    {
        InitMessage();
        InitManager();
    }

    #region 基礎功能
    private void InitManager()
    {
        MessageBox.DEBUG("加载资源");
        ResManagerState_Loop tResManagerState_Loop = new ResManagerState_Loop();
    }

    void InitMessage()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.GameStart, OnGameStart);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.GAMEOVER, OnGameOver);
    }

    //private void LoadGameDataForMemory()
    //{
    //    if (ccLocalDataManager.f_HasLocalData(EM_eLocalData.iCurGameControllIndex.ToString()))
    //    {
    //        StaticValue.m_iCurGameControllIndex = ccLocalDataManager.f_GetLocalData<int>(EM_eLocalData.iCurGameControllIndex.ToString());
    //    }
    //    else
    //    {
    //        StaticValue.m_iCurGameControllIndex = 1000;
    //    }

    //}
    
    void OnGameStart(object Obj)
    {
        int iStartDayId = (int)Obj;

        m_GameTable.SetActive(true);
        _GameLevelControll.f_Start(iStartDayId);
    }

    /// <summary>
    /// 更改遊戲狀態
    /// </summary>
    /// <param name="Obj">遊戲狀態</param>
    public void OnCangeGameState(EM_GameLevelAction Obj, object e = null)
    {
        _GameLevelControll.f_CangeGameState((int)Obj, e);
    }

    public bool f_IsGaming()
    {
        return _GameLevelControll.f_IsGaming();
    }

    void OnGameOver(object Obj)
    {
        MessageBox.DEBUG("OnGameOver");
    }
    #endregion

    private void Update()
    {
        _GameLevelControll.f_Update();
    }

    public void f_SetGameUI(string strName, string tMessage, object obj = null)
    {
        ccUIManage.GetInstance().f_SendMsgV3(strName.ToLower() + ".bundle", strName, tMessage, obj);
    }

    #region GameObj

    public Transform f_GetGameTable()
    {
        return m_GameTable.transform;
    }

    private void InitGamePos(Transform GamePos)
    {
        for (int i = 0; i < GamePos.childCount; i++)
        {
            GameObject Obj = GamePos.GetChild(i).gameObject;
            if (_dirGameObj.ContainsKey(Obj.name))
            {
                MessageBox.ASSERT("场景里的位置物件存在重复的命名:" + Obj.name);
            }
            _dirGameObj.Add(Obj.name, Obj);
        }
    }

    public void f_SetGameMap(GameObject oMap)
    {
        oMap.transform.SetParent(f_GetGameTable());
        Transform GamePos = oMap.transform.Find("GamePos");
        InitGamePos(GamePos);
        GamePos.gameObject.SetActive(false);
    }

    public Vector3 f_GeMapObjPosition(string strObj)
    {
        GameObject GamePos = null;
        if (_dirGameObj.TryGetValue(strObj, out GamePos))
        {
            return GamePos.transform.position;
        }
        return Vector3.zero;
    }

    public Transform f_GetMapObj(string strObj)
    {
        GameObject MapObj = null;
        if (_dirGameObj.TryGetValue(strObj, out MapObj))
        {
            return MapObj.transform;
        }
        return null;
    }

    public GameObject f_GetCharacterObj(string strId)
    {
        int _Id = ccMath.atoi(strId);
        for (int i = 0; i < m_GameTable.transform.childCount; i++)
        {
            if (m_GameTable.transform.GetChild(i).GetComponent<BaseRoleControllV2>() != null)
            {
                int _GameObjId = m_GameTable.transform.GetChild(i).GetComponent<BaseRoleControllV2>()._BaseRoleProtecy.m_iId;
                if (_GameObjId == _Id)
                {
                    return m_GameTable.transform.GetChild(i).gameObject;
                }
            }
        }

        return null;
    }
    #endregion

    #region 時裝讀取相關
    public void f_LoadRoleAvatar(object obj)
    {//測試數據
        PlayerDataTools.f_ChangePlayerData(EM_UserAttr.eUserAttr_Sex, DressEditorPoolDT._iGender);
        PlayerDataTools.f_ChangePlayerData(EM_UserAttr.eUserAttr_Race, DressEditorPoolDT._iRace);
        Data_Pool.GetInstance().m_PlayerData.m_aAvaterBuf = f_DefualAvatarBuf();
        Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf = f_DefualSpecialBuf();
    }

    /// <summary>
    /// 初始化角色Prefab
    /// </summary>
    /// <param name="tRoleControl">角色時裝控制器Prefab</param>
    /// <param name="tOriginPos">初始化位置(若為null，則不初始化位置)</param>
    public DressRoleControl f_InitRole(DressRoleControl tRoleControl, Transform tOriginPos)
    {
        //複製控制器Prefab並做初始化
        DressRoleControl aRoleControl = GameObject.Instantiate(tRoleControl.gameObject).GetComponent<DressRoleControl>();
        aRoleControl.f_Init((int)GameEM.emRoleType.Player, new DressRoleActionController(tRoleControl), tOriginPos);
        aRoleControl.f_InitUserData(Data_Pool.GetInstance().m_PlayerData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sex), Data_Pool.GetInstance().m_PlayerData.f_GetProperty((int)EM_UserAttr.eUserAttr_Race));
        if (PlayerDataTools.f_CheckUseSpecial())
        {//特殊時裝取用
            aRoleControl.f_LoadSpecialBuf(f_CreateBuf(), Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf);
        }
        else
        {//一般時裝取用
            aRoleControl.f_LoadAvatarBuf(f_CreateBuf());
        }
        if (!PlayerDataTools.f_CheckAvatar())
        {
            aRoleControl.f_SaveAvater();
        }
        return aRoleControl;
    }
    #endregion

    #region 換裝相關
    /// <summary>
    /// 換裝
    /// </summary>
    /// <param name="iId">時裝ID</param>
    /// <param name="RoleControl">角色時裝控制器</param>
    public void f_ChangeAvatar(int iId, DressRoleControl RoleControl)
    {
        AvaterPartDT tAvaterPartDT = (AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(iId);
        f_ChangeAvatar(tAvaterPartDT, RoleControl);
    }

    /// <summary>換裝</summary>
    public void f_ChangeAvatar(AvaterPartDT tAvaterPartDT, DressRoleControl RoleControl)
    {
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressStart);
        RoleControl.f_ChangeAvater(tAvaterPartDT);
    }

    #region 卸下時裝
    /// <summary>卸下時裝</summary>
    /// <param name="iId">時裝ID</param>
    public void f_TakeOff(int iId, DressRoleControl RoleControl)
    {
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressStart);
        AvaterPartDT tAvaterPartDT = (AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(iId);
        RoleControl.f_TakeOff(tAvaterPartDT.iRolePart, tAvaterPartDT.szTakeOff);
    }

    public void f_TakeOff(AvaterPartDT tAvaterPartDT, DressRoleControl RoleControl)
    {
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressStart);
        RoleControl.f_TakeOff(tAvaterPartDT.iRolePart, tAvaterPartDT.szTakeOff);
    }
    #endregion

    #region 組合包
    /// <summary>更換套裝</summary>
    public void f_ChangeSuitAvater(string strData, DressRoleControl RoleControl)
    {
        string[] tData = ccMath.f_String2ArrayString(strData, ";");
        for (int i = 0; i < tData.Length; i++)
        {
            int[] iAvatar = ccMath.f_String2ArrayInt(tData[i], "-");
            f_ChangeAvatar(iAvatar[0], RoleControl);
        }
    }
    public void f_TakeOffSuit(string strData, DressRoleControl RoleControl)
    {//卸下套裝
        string[] tData = ccMath.f_String2ArrayString(strData, ";");
        for (int i = 0; i < tData.Length; i++)
        {
            int[] iAvatar = ccMath.f_String2ArrayInt(tData[i], "-");
            f_TakeOff(iAvatar[0], RoleControl);
        }
    }
    #endregion

    #region 換色

    public void f_ChangeMaterial(AvaterPartDT tAvaterPartDT, string strEm, DressRoleControl RoleControl)
    {
        RoleControl.f_ChangeMaterial(tAvaterPartDT, strEm);
    }
    #endregion

    #endregion

    #region 模型相關
    public string _RoleAvater;
    private RoleAvaterList _RoleAvaterList = null;
    /// <summary>
    /// 取得模型預設數據
    /// </summary>
    /// <param name="iRace">人種</param>
    /// <param name="iGender">性別</param>
    /// <returns></returns>
    public RoleAvaterDT f_GetAvaterDT(int iRace, int iGender)
    {
        if (_RoleAvaterList == null)
        {//從RoleAvaterList取得預設數據
            _RoleAvaterList = glo_Main.GetInstance().m_ResourceManager.f_LoadRoleAvaterList(_RoleAvater.ToLower() + ".bundle", _RoleAvater);
        }
        for (int i = 0; i < _RoleAvaterList._RoleAvaterList.Count; i++)
        {//尋找對應人種和性別的數據(詳看RoleAvaterDT.cs)
            if (_RoleAvaterList._RoleAvaterList[i].f_CheckRole(iRace, iGender))
            {
                return _RoleAvaterList._RoleAvaterList[i];
            }
        }
        return null;
    }
    #endregion

    #region 動畫相關
    /// <summary>
    /// 設定IK
    /// </summary>
    /// <param name="RoleControl">角色時裝控制器</param>
    /// <param name="bSet">是否啟用IK功能</param>
    /// <param name="iIK">1 腳  2 手</param>
    public void f_SetIK(DressRoleControl RoleControl, bool bSet, int iIK)
    {
        RoleControl.f_SetIK(bSet, iIK);
    }

    /// <summary>
    /// 調節動畫速度
    /// </summary>
    /// <param name="RoleControl">角色時裝控制器</param>
    /// <param name="fSpeed">速度</param>
    public void f_SetBikeSpeed(DressRoleControl RoleControl, float fSpeed)
    {
        RoleControl.f_SetBikeSpeed(fSpeed);
    }

    /// <summary>
    /// 切換指定動畫
    /// </summary>
    /// <param name="RoleControl">角色時裝控制器</param>
    /// <param name="strAnim">角色動畫名</param>
    /// <param name="strBikeAnim">腳踏車動畫名</param>
    /// <param name="bIK">是否啟用IK功能</param>
    public void f_SetAnimClip(DressRoleControl RoleControl, string strAnim, string strBikeAnim, bool bIK1 = true, bool bIK2 = true, bool bBike = true)
    {
        RoleControl.f_SetPlayerAnimClip(strAnim);
        RoleControl.f_SetBikeAnimClip(strBikeAnim);
        RoleControl.f_SetIK(bIK1, 1);
        RoleControl.f_SetIK(bIK2, 2);
        RoleControl.f_SetDisplayBike(bBike);
    }

    /// <summary>
    /// 切換寵物指定動畫
    /// </summary>
    /// <param name="RoleControl">角色時裝控制器</param>
    /// <param name="Anim">動畫名</param>
    public void f_SetPetAnimClip(DressRoleControl RoleControl, string strAnim)
    {
        RoleControl.f_SetPetAnimClip(strAnim);
    }
    #endregion

    #region 取得時裝Byte
    public byte[] f_CreateBuf()
    {//取得一般時裝
        if (PlayerDataTools.f_CheckAvatar())
        {//取用內存資料
            MessageBox.DEBUG("載入已儲存的時裝");
            return Data_Pool.GetInstance().m_PlayerData.m_aAvaterBuf;
        }
        else
        {//取用預設資料
            MessageBox.DEBUG("時裝初始化");
            return f_DefualAvatarBuf();
        }
    }

    public byte[] f_CreateSpecialBuf()
    {//取得特殊造型
        if (PlayerDataTools.f_CheckUseSpecial())
        {//取用內存資料
            MessageBox.DEBUG("載入已儲存的特殊造型");
            return Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf;
        }
        else
        {//取用預設資料
            return f_DefualSpecialBuf();
        }
    }

    public byte[] f_DefualAvatarBuf()
    {//此處要改成預設時裝ID
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);//眼睛
        tCreateSocketBuf.f_Add(0);//臉
        tCreateSocketBuf.f_Add(0);//頭髮
        tCreateSocketBuf.f_Add(0);//髮色
        tCreateSocketBuf.f_Add(41);//上半身
        tCreateSocketBuf.f_Add(51);//下半身
        tCreateSocketBuf.f_Add(0);//頭部
        tCreateSocketBuf.f_Add(71);//鞋子
        tCreateSocketBuf.f_Add(81);//襪子
        tCreateSocketBuf.f_Add(0);//手臂
        tCreateSocketBuf.f_Add(0);//腿
        tCreateSocketBuf.f_Add(0);//手指
        tCreateSocketBuf.f_Add(91);//手套
        tCreateSocketBuf.f_Add(0);//眼鏡
        tCreateSocketBuf.f_Add(0);//頭盔
        tCreateSocketBuf.f_Add(0);//鬍子
        tCreateSocketBuf.f_Add(0);//配件
        tCreateSocketBuf.f_Add(1);//皮膚
        tCreateSocketBuf.f_Add(0);//寵物

        tCreateSocketBuf.f_Add(501);//腳踏車
        tCreateSocketBuf.f_Add(0);//車輪
        tCreateSocketBuf.f_Add(0);//車架

        tCreateSocketBuf.f_Add(50);//身高
        tCreateSocketBuf.f_Add(0);//腰圍
        tCreateSocketBuf.f_Add(0);//胸部
        tCreateSocketBuf.f_Add(0);//手臂
        tCreateSocketBuf.f_Add(0);//腿寬
        tCreateSocketBuf.f_Add(0);//斜方肌
        tCreateSocketBuf.f_Add(0);//臀部
        tCreateSocketBuf.f_Add(0);//頭頸
        tCreateSocketBuf.f_Add(50);//身長
        tCreateSocketBuf.f_Add(50);//腿長
        tCreateSocketBuf.f_Add(50);//肩寬
        tCreateSocketBuf.f_Add(50);//手臂長
        return tCreateSocketBuf.f_GetBuf();
    }

    public byte[] f_DefualSpecialBuf()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(0);//特殊造型
        return tCreateSocketBuf.f_GetBuf();
    }
    #endregion
}