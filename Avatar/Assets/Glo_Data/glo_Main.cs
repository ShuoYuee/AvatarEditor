using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using ccU3DEngine;
using System.Collections.Generic;
using ccILR;
using UnityEngine.SceneManagement;

public class glo_Main : MonoBehaviour
{
    /// <summary>
    /// 声音控制器
    /// </summary>
    public AudioManager m_AudioManager;
    public string BgmName = "";

    public EM_GameStatic m_EM_GameStatic = EM_GameStatic.Waiting;


    [HideInInspector]
    public ccMessagePoolV2 m_GameMessagePool = new ccMessagePoolV2();
    public ccMessagePoolV2 m_UIMessagePool = new ccMessagePoolV2();

    ccLog m_ccLog = null;
    public SC_Pool m_SC_Pool;

    //public MainLogic m_MainLogic;
    /// <summary>
    /// 游戏资源管理器
    ///// </summary>
    public ResourceManager m_ResourceManager;

    private static glo_Main _Instance = null;
    public static glo_Main GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = (glo_Main)FindObjectOfType(typeof(glo_Main));
            if (null == _Instance)
            {
                MessageBox.ASSERT("init glo_Main Fail");
                return null;
            }

        }
        return _Instance;
    }

    void Awake()
    {
        m_ccLog = new ccLog("Log", true, true);
        //m_ccLog.f_SetUserId(SystemInfo.deviceUniqueIdentifier);
        //m_ccLog.f_Start();

        GloData.glo_iVer = ccMath.atoi(Application.version);
        GloData.glo_strVer = GloData.glo_strVer + GloData.glo_iVer;
        ResourceTools.f_UpdateURL();

        m_SC_Pool = new SC_Pool();
        m_ResourceManager = new ResourceManager();
    }


    void Start()
    {
        MessageBox.f_UseUnityDebug();
        MessageBox.DEBUG("初始游戏");
        MessageBox.DEBUG(Application.persistentDataPath);

        DontDestroyOnLoad(this);    //自己不消失

        /// 初始引擎工作参数
        InitEngineParam();
        /// 初始化Unity工作环境
        InitGameUnityParams();
        InitPath();
        InitComponent();

        InitMessage();
        //InitAudio();
        InitResManager();
    }

    /// <summary>
    /// 初始引擎工作参数
    /// </summary>
    private void InitEngineParam()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = 90;

        //打开模拟器模式 ，此模式下资源不需要经过打包输出和上传到资源服务器就可以直接使用
        //平时开发都采用模拟器模式来进行，后期整合时再切换回正常模式
        ccU3DEngineParam.m_bIsLocalAB = true;

        //设置当前编辑器及最终打包输出的目标平台
#if UNITY_EDITOR
        UnityEditor.BuildTarget tBuildTarget = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
        if (tBuildTarget == UnityEditor.BuildTarget.Android)
        {
            ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.Android;
        }
        else if (tBuildTarget == UnityEditor.BuildTarget.iOS)
        {
            ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.iOS;
        }
        else if (tBuildTarget == UnityEditor.BuildTarget.StandaloneWindows || tBuildTarget == UnityEditor.BuildTarget.StandaloneWindows64)
        {
            ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.StandaloneWindows;
        }
        //else if (tBuildTarget == UnityEditor.BuildTarget.StandaloneOSXIntel || tBuildTarget == UnityEditor.BuildTarget.StandaloneOSXIntel64 ||
        //    tBuildTarget == UnityEditor.BuildTarget.StandaloneOSXUniversal)
        //{ 
        //    ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.Mac;
        //}
#elif UNITY_IOS
            ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.iOS;
#elif UNITY_ANDROID
           ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.Android;
#elif UNITY_STANDALONE
            ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.StandaloneWindows;
#elif UNITY_STANDALONE_OSX
            ccU3DEngineParam.m_CurBuildTarget = ccBuildTarget.StandaloneWindows;
#endif

        ccU3DEngineParam.m_bIsDebug = true;
    }

    /// <summary>
    /// 初始化Unity工作环境
    /// </summary>
    private void InitGameUnityParams()
    {
        //GameSetting.InitGameQuality();
        //QualitySettings.SetQualityLevel(GameSetting.Quality);
        //GameSetting.SetResolution(GameSetting.Quality);

        //Shader.SetGlobalColor("_GlobalColor", Color.white);
    }

    private void InitPath()
    {
        //string strCatchFilePath = PlayerPrefs.GetString("CATHFILEPATH");

        //if (string.IsNullOrEmpty(strCatchFilePath))
        //{
        //    Glo_Data.glo_strCatchFilePath = Environment.CurrentDirectory + "\\AvgRun\\AvgData";
        //}
        //else
        //{
        //    Glo_Data.glo_strCatchFilePath = strCatchFilePath;
        //}
        ////if (!Directory.Exists(Glo_Data.glo_strCatchFilePath))
        ////{
        ////    Directory.CreateDirectory(Glo_Data.glo_strCatchFilePath);
        ////}
    }

    /// <summary>
    /// 初始化核心组件
    /// </summary>
    private void InitComponent()
    {
        ccUIManage.GetInstance().f_SaveFactoryHandler(ccILR_ClassFactory.GetInstance());

        gameObject.AddComponent<UpdateManager>();
        //gameObject.AddComponent<ccSceneMgr>();
        UpdateManager tUpdateManager = UpdateManager.Get();
        UpdateManager.Get().SetClientVersion(CurrentBundleVersion.version);
        gameObject.AddComponent<AssetLoader>();

        GameDataLoad.f_LoadGameSystemMemory();
    }

    /// <summary>
    /// 初始化资源管理
    /// 开始加载资源
    /// </summary>
    private void InitResManager()
    {
        MessageBox.DEBUG("InitResManager...");
        //m_GameMessagePool.f_RemoveListener(MessageDef.LOADSCSUC, CallBack_InitResManager);

        startLoadResourceTime = Time.realtimeSinceStartup;
        LoadResource _LoadResource = new LoadResource();
        _LoadResource.f_StartLoad(Callback_LoadResSuc);
    }

    private float startLoadResourceTime = 0;
    /// <summary>
    /// 资源成功加载完回调、
    /// 初始化玩家数据结构、
    /// 广播资源加载完成（通知显示logo页面可以进入游戏）
    /// </summary>
    /// <param name="Obj"></param>
    void Callback_LoadResSuc(object Obj)
    {
        //Data_Pool.f_InitPool();

#if Control
        //Control;
        MessageBox.DEBUG("使用控制台开始游戏");
        GameControllLogin();
#else
        MessageBox.DEBUG("不使用控制台直接开始游戏");
        //GameMain.GetInstance().f_StartGame();   
#endif

        ccTimeEvent.GetInstance().f_RegEvent(0f, false, null, CallBack_StartUpdateRes);
    }


    #region 资源更新及切换到登陆场景

    void CallBack_StartUpdateRes(object Obj)
    {
        UpdateManager.Get().m_remoteUri = new[] { GloData.glo_strABServerURL };
        UpdateManager.Get().BeginInitialize(m_SC_Pool.f_GetABVer(), OnUpdateInitialize, false);
    }


    void OnUpdateInitialize()
    {
        MessageBox.DEBUG("开始更新资源。");
        UpdateManager.Get().BeginDownload(OnUpdateComplete);
        //OnUpdateComplete();
    }

    private void OnUpdateComplete()
    {
        //ccTimeEvent.GetInstance().f_RegEvent(30, false, null, TTTTT);
        UpdateManager.Get().f_ZipBaseRes(CallBack_ZipBaseResProgress, CallBack_ZipBaseResSuc);

    }

    void CallBack_ZipBaseResProgress(object Obj)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UpdateInitProgress, Obj);
    }

    private void CallBack_ZipBaseResSuc(object Obj)
    {

        ccILR_ClassFactory.GetInstance().f_LoadHotFixDLL(true);
        MessageBox.DEBUG("资源更新成功...");

        LanguageManager.GetInstance().f_ChangeLanguage(Locale.enUS);

        f_InitGame();
    }

    #endregion


    private void InitAudio()
    {
        //設定背景音樂
        m_AudioManager._Bgm = gameObject.AddComponent<AudioSource>();
        m_AudioManager._Bgm.clip = m_ResourceManager.f_CreateAudio(BgmName);
        m_AudioManager.f_SetBGMVolume(StaticValue.m_fBgmVolume);
        m_AudioManager.f_PlayAudioMusic();
    }


    private void InitMessage()
    {
        ccTimeEvent.GetInstance().f_ChangePingTime(0.02f);
    }

    void Update()
    {
        m_GameMessagePool.f_Update();
        m_UIMessagePool.f_Update();
        GameSocket.GetInstance().f_Update();
    }

    private void OnDestroy()
    {

    }

    public void f_Destroy()
    {
        MessageBox.DEBUG("强制结束游戏 QuitGame");

        MessageBox.DEBUG("................................................");

        ccTimeEvent.GetInstance().f_RegEvent(1, false, null, ApplicationQuit);
    }

    //void OnApplicationQuit()
    void ApplicationQuit(object Obj)
    {
        GameSocket.GetInstance().f_Close();
        //m_ccLog.f_Quit();
        Application.Quit();
    }


    /// <summary>
    /// 登陆游戏成功，初始游戏数据准备进入游戏
    /// </summary>
    public void f_InitGame()
    {
        InitGameData();
        InitSocket();

        ccUIManage.GetInstance().f_SendMsgV3("ui_gamelogin.bundle", "UI_GameLogin", BaseUIMessageDef.UI_OPEN);
    }

    void InitSocket()
    {
       GameSocket.GetInstance();
    }

    private void InitGameData()
    {
        Data_Pool.GetInstance().f_InitPool();

    }


    public Coroutine f_StartCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    public void f_DoDestroyObj(GameObject Obj)
    {
        Destroy(Obj);
    }


}
