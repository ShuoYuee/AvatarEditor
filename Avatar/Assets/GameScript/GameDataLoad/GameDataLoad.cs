using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataLoad
{

    /// <summary>
    /// 加载游戏资料
    /// </summary>
    public static void f_LoadGameSystemMemory()
    {

        //加载游戏进度
        //f_LoadGameStep();
        f_LoadGameSystem();

        //加载游戏Session值
    }

    public static void f_LoadGameSystem()
    {
        StaticValue.m_fBgmVolume = LocalDataManager.f_GetLocalData<float>("m_fBgmVolume", 0.5f);
        StaticValue.m_fSoundVolume = LocalDataManager.f_GetLocalData<float>("m_fSoundVolume", 0.5f);
        StaticValue.m_fEffectVolume = LocalDataManager.f_GetLocalData<float>("m_fEffectVolume", 0.5f);
    }

    public static bool f_LoadGameStep()
    {
        int iGameStepSection = LocalDataManager.f_GetLocalData<int>("iGameStepSection", 0);
        int iGameStepAvgGameStory = LocalDataManager.f_GetLocalData<int>("iGameStepAvgGameStory", 0);
        if (iGameStepSection == 0 || iGameStepAvgGameStory == 0)
        {
            MessageBox.DEBUG("未发现存档信息开始新游戏");
            //iGameStepSection = StaticValue.m_GameInforDT.m_iStartId;
            //iGameStepAvgGameStory = 0;
            return false;
        }
        else
        {
            MessageBox.DEBUG("发现存档信息:" + iGameStepSection + "-" + iGameStepAvgGameStory);
            return true;
        }
        //StaticValue.m_iSectionId = iGameStepSection;
        //StaticValue.m_iAvgGameStoryId = iGameStepAvgGameStory;
    }

    public static void f_LoadGameLocal()
    {
        StaticValue.m_fBgmVolume = LocalDataManager.f_GetLocalData<float>("m_fBgmVolume");
        StaticValue.m_fSoundVolume = LocalDataManager.f_GetLocalData<float>("m_fSoundVolume");
        StaticValue.m_fEffectVolume = LocalDataManager.f_GetLocalData<float>("m_fEffectVolume");
        Data_Pool.GetInstance().f_LoadPool();
    }

    public static void f_SaveGameSystemMemory()
    {
        LocalDataManager.f_SetLocalData<float>("m_fBgmVolume", StaticValue.m_fBgmVolume);
        LocalDataManager.f_SetLocalData<float>("m_fSoundVolume", StaticValue.m_fSoundVolume);
        LocalDataManager.f_SetLocalData<float>("m_fEffectVolume", StaticValue.m_fEffectVolume);
    }

    public static int f_MemroySessionLoad(string strSession, int iDefault = -99999)
    {
        return LocalDataManager.f_GetLocalData<int>(strSession, iDefault);
    }

    public static void f_MemroySessionSave(string strSession, int iData)
    {
        LocalDataManager.f_SetLocalData<int>(strSession, iData);
    }

    public static void f_MemroySessionDelete(string strSession)
    {
        LocalDataManager.f_DeleteLocalData(strSession);
    }


}