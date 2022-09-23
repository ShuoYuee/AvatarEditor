using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
/// <summary>
/// 资源加载
/// </summary>
public class LoadResource
{
    private ccMachineManager<ccMachineParamentBase> _ResManager = null;
    private int _iLoadResourceTime = 0;
    private string _strResourceMd5;
    //public delegate void Callback_LoadHttp(HttpDataDT eHttpDataDT);

    /// <summary>
    /// 资源加载完回调
    /// </summary>
    private ccCallback _hCallBack;
    /// <summary>
    /// 资源开始加载
    /// </summary>
    /// <param name="hCallBack">资源加载完函数回调</param>
    public void f_StartLoad(ccCallback hCallBack)
    {
        _hCallBack = hCallBack;
        InitResManager();
    }
    
    private void InitResManager()
    {
        MessageBox.DEBUG("加载资源");

        _ResManager = new ccMachineManager<ccMachineParamentBase>(new ResManagerState_Loop());
        //GameObject tLoginPage = GameObject.Find("LoginPage");

        ccMachineStateBase<ccMachineParamentBase> tFstMachineStateBase = new ResManagerState_Ver();
        _ResManager.f_RegState(tFstMachineStateBase);
        _ResManager.f_RegState(new ResManagerState_LoadSC());
        _ResManager.f_RegState(new ResManagerState_DispSC());
        //_ResManager.f_RegState(new ResManagerState_UpdateRes());
        //_ResManager.f_RegState(new ResManagerState_UpdateResForLocal());
        //_ResManager.f_RegState(new ResManagerState_UpdateResForServer());
        _ResManager.f_RegState(new ResManagerState_Login(LoadResourceSuc));
        _ResManager.f_ChangeState(tFstMachineStateBase);

        _iLoadResourceTime = ccTimeEvent.GetInstance().f_RegEvent(0.1f, true, null, Callback_Update);
    }

    void Callback_Update(object Obj)
    {
        _ResManager.f_Update();
    }
    
    private void LoadResourceSuc(object Obj)
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(_iLoadResourceTime);
        _hCallBack(eMsgOperateResult.OR_Succeed);
    }
        





}