using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;

public class ResManagerState_Ver : ccMachineStateBase<ccMachineParamentBase>
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.Ver;
    private string _strResourceMd5;
    WWW w = null;
    bool _bInitWWW = false;
    private bool _bSaveCatchBuf = false;

    public ResManagerState_Ver()
        : base((int)m_EM_AIStatic)
    {
        
    }

    public override void f_Enter(object Obj)
    {
        InitWWW();
    }

    private void InitWWW()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            _bInitWWW = true;
            //WWWForm form = new WWWForm();
            //form.AddField("iUserId", 1);
            w = new WWW(GloData.glo_strLoadVer);

            //关掉网络未打开提示
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(MessageDef.LOGINEROINFOR, "SUC"); 
        }
        else
        {
            //弹出网络未打开提示
            //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(MessageDef.LOGINEROINFOR, "网络异常，请检查网络连接。"); 
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEMESSAGEBOX, (int)eMsgOperateResult.OR_Error_WIFIConnectTimeOut);
        }
    }

    public override void f_Execute()
    {
        if (w == null)
        {
            if (!_bInitWWW)
            {
                InitWWW();
            }
            return;
        }
        if (!w.isDone)
            return;

        if (w.error != null)
        {
            MessageBox.DEBUG("網路錯誤 " + w.error + " " + w.url);
            LoadFail();
        }
        else if (w.text.Length < 4)
        {
            MessageBox.DEBUG("網路錯誤2");
            LoadFail();
        }
        else
        {
            //LoadVerSuc(w.text);    
            LoadVerSuc(w.text);     
        }
        w.Dispose();
        w = null;

    }


    private void LoadFail()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEMESSAGEBOX, (int)eMsgOperateResult.OR_Error_WIFIConnectTimeOut);
    }

    private void LoadVerSuc(string strVerData)
    {
        string strVer = PlayerPrefs.GetString("Ver");
        if (strVer == "")
        {
            //GameSet.f_Reset();
        }

        string[] aData = ccMath.f_String2ArrayString(strVerData, ":");

        if (aData.Length == 4)
        {
            DispVer(strVer, aData[0]);
            DispServerInfor(aData[1]);
            GloData.glo_iAutoUpdateLog = ccMath.atoi(aData[2]);
            GloData.glo_iAutoUpdateLogTime = ccMath.atoi(aData[3]);
        }
        else
        {
            MessageBox.DEBUG("加載版本失敗");
        }
    }

    private void DispVer(string strLocalVer, string strServerVer)
    { 
        string[] aVerData = ccMath.f_String2ArrayString(strServerVer, "-");

        if (aVerData.Length == 1)
        {
            _strResourceMd5 = "";
        }
        else
        {
#if UNITY_WEBPLAYER
            _strResourceMd5 = aVerData[2];
#elif UNITY_ANDROID
            _strResourceMd5 = aVerData[4];
#elif UNITY_IPHONE
            _strResourceMd5 = aVerData[3];
#else
            _strResourceMd5 = aVerData[1];
#endif
        }

        bool bFileEro = false;
        if (!ccFile.f_ExistsFile(Application.persistentDataPath + "/" + GloData.glo_ProName + "/ccData.xlscc"))
        {
            bFileEro = true;
            MessageBox.DEBUG("脚本文件丢失强制更新");
        }

        MessageBox.DEBUG("V:" + strLocalVer);
        if (bFileEro == true || strLocalVer != aVerData[0])
        {
            _bSaveCatchBuf = true;
            PlayerPrefs.SetString("RVer", aVerData[0]);
            //HttpTools.f_HttpLoad(GloData.glo_strLoadAllSC, LoadSCSuc);
        }
        else
        {
            _bSaveCatchBuf = false;
            //LoadSCSuc(ccFile.f_ReadFileForByte(Application.persistentDataPath + "/" + GloData.glo_ProName + "/", "ccData.xlscc"));
        }

        f_SetComplete((int)EM_ResManagerStatic.LoadSC, _bSaveCatchBuf);        
        
    }

    private void DispServerInfor(string ppSQL)
    {
        //glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_LoadSCForData(ppSQL);
    }

    public string f_GetResourceMD5()
    {
        return _strResourceMd5;
    }

}