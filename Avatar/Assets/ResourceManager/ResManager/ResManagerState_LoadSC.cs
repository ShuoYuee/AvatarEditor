using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;

public class ResManagerState_LoadSC : ccMachineStateBase<ccMachineParamentBase>
{
    static EM_ResManagerStatic m_EM_AIStatic = EM_ResManagerStatic.LoadSC;
    
    WWW w = null;
    private bool _bUpdate = false;

    public ResManagerState_LoadSC()
        : base((int)m_EM_AIStatic)
    {

    }

    public override void f_Enter(object Obj)
    {
        _bUpdate = (bool)Obj;
        if (_bUpdate)
        {
            MessageBox.DEBUG("更新脚本");
            string strUrl = "";

#if UNITY_WEBPLAYER
         strUrl = GloData.glo_strLoadAllSC + "ccData_W.bytes";
#elif UNITY_ANDROID
            strUrl = GloData.glo_strLoadAllSC + "ccData_A.bytes";
#elif UNITY_IPHONE
         strUrl = GloData.glo_strLoadAllSC + "ccData_I.bytes";
#else
            strUrl = GloData.glo_strLoadAllSC + "ccData_W.bytes";
#endif

            //long starttime = DateTime.Now.Ticks;
            w = new WWW(strUrl);
        }
        else
        {
            MessageBox.DEBUG("加载脚本");
            LoadSuc(ccFile.f_ReadFileForByte(Application.persistentDataPath + "/" + GloData.glo_ProName + "/", "ccData.xlscc"));
        }
        MessageBox.DEBUG(GloData.glo_strLoadAllSC);
    }


    public override void f_Execute()
    {
        if (w == null)
        {
            return;
        }
        if (!w.isDone)
            return;

        if (w.error != null)
        {
            MessageBox.DEBUG("網路錯誤");
            LoadFail();
        }
        else if (w.text.Length < 4)
        {
            MessageBox.DEBUG("網路錯誤2");
            LoadFail();
        }
        else
        {
            LoadSuc(w.bytes);
        }
        w.Dispose();
        w = null;

        MessageBox.DEBUG("下载脚本成功");
    }


    private void LoadFail()
    {

    }

    private void LoadSuc(byte[] aBytes)
    {
        if (_bUpdate)
        {
            ccFile.f_SaveFileForByte(Application.persistentDataPath + "/" + GloData.glo_ProName + "/", "ccData.xlscc", aBytes);
            string strServerVer = PlayerPrefs.GetString("RVer");
            PlayerPrefs.SetString("Ver", strServerVer);
            MessageBox.DEBUG("更新脚本版本为：" + strServerVer);
        }
        f_SetComplete((int)EM_ResManagerStatic.DispSC, aBytes);
    }

 


}