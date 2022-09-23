
using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI控制器基类
/// </summary>
public class BaseAIControl
{
    protected ccMachineManager<ccMachineParamentBase> _AIMachineManger = null;
    private BaseRoleControllV2 _RoleControl;

    public BaseAIControl(BaseRoleControllV2 tRoleControl)
    {
        _RoleControl = tRoleControl;

        //AI_Idle tAI_Idle = new AI_Idle(tRoleControl);
        //_AIMachineManger = new ccMachineManager(tAI_Idle);
    }

    /// <summary>
    /// 向控制器增加新的AI动作
    /// </summary>
    public void f_RegNewAIState(AI_BaseState tAI_BaseState)
    {
        if (_AIMachineManger == null)
        {
            _AIMachineManger = new ccMachineManager<ccMachineParamentBase>(tAI_BaseState);
        }
        else
        {
            _AIMachineManger.f_RegState(tAI_BaseState);
        }
    }

    public void f_ChangeAIState(AI_EM.EM_AIState tEM_AIState, object Obj = null)
    {
        ccMachineStateBase<ccMachineParamentBase> tccMachineStateBase = _AIMachineManger.f_GetStaticBase((int)tEM_AIState);
        if (tccMachineStateBase == null)
        {
            MessageBox.ASSERT("无此状态机 " + tEM_AIState.ToString());
        }
        else
        {
            _AIMachineManger.f_ChangeState((int)tEM_AIState, Obj);
            UpdateLog();
        }
    }

    public AI_EM.EM_AIState f_GetRoleCurState()
    {
        ccMachineStateBase<ccMachineParamentBase> tccMachineStateBase = _AIMachineManger.f_GetCurMachineState();
        return (AI_EM.EM_AIState)tccMachineStateBase.iId;
    }
       
    public ccMachineStateBase<ccMachineParamentBase> f_GetRoleState(AI_EM.EM_AIState tEM_AIState)
    {
        ccMachineStateBase<ccMachineParamentBase> tccMachineStateBase = _AIMachineManger.f_GetStaticBase((int)tEM_AIState);
        return tccMachineStateBase;
    }

    /// <summary>
    /// 更新RoleControl狀態字串
    /// </summary>
    private void UpdateLog()
    {
        ccMachineStateBase<ccMachineParamentBase> tccMachineStateBase = _AIMachineManger.f_GetCurMachineState();
        AI_EM.EM_AIState tState = (AI_EM.EM_AIState)tccMachineStateBase.iId;
        _RoleControl.ppSQL = tState.ToString();
    }

    public void f_Update()
    {
        if (_AIMachineManger != null)
        {
            _AIMachineManger.f_Update();
        }
    }

    #region AI動作數值
    /// <summary>忽視警戒時間限制(持續警戒)</summary>
    private bool _IgnoreWarnTime = false;
    /// <summary>警戒時長</summary>
    private float _fMaxAIWarnTime = 1.5f;
    /// <summary>追逐時長</summary>
    private float _fMaxAIChaseTime = 2f;
    /// <summary>自動巡路位置</summary>
    private List<Vector3> _aPathObj = new List<Vector3>();
    /// <summary>每個巡邏點停留時長</summary>
    private float _fAutoMoveSleepTime = 1f;
    /// <summary>各巡邏點動畫Id</summary>
    private List<int> _aPathAnimId = new List<int>();
    /// <summary>各巡邏點的嵌入觸發指令</summary>
    private List<int> _szVarId = new List<int>();
    #endregion

    #region 攻击相关 =================================================

    public virtual void f_Attack()
    {
        //AI_Attack2 tAI_Attack = (AI_Attack2)_AIMachineManger.f_GetStaticBase((int)AI_EM.EM_AIState.Attack);
        //tAI_Attack.f_Attack();
        UpdateLog();
    }

    public virtual void f_AttackComplete()
    {
        _AIMachineManger.f_ChangeState((int)AI_EM.EM_AIState.Idle);
        UpdateLog();
    }

    #region 警戒
    /// <summary>獲取是否無視警戒時間</summary>
    public bool f_GetIgnoreWarnTime()
    {
        return _IgnoreWarnTime;
    }

    /// <summary>
    /// 設定無視警戒時間
    /// </summary>
    /// <param name="bIgnoreWarnTime">是否無視</param>
    public void f_SetIgnoreWarnTime(bool bIgnoreWarnTime)
    {
        _IgnoreWarnTime = bIgnoreWarnTime;
    }

    /// <summary>獲取警戒時長</summary>
    public float f_GetMaxAIWarnTime()
    {
        return _fMaxAIWarnTime;
    }

    /// <summary>
    /// 設定警戒時長
    /// </summary>
    /// <param name="fMaxWarnTime">警戒時長</param>
    public void f_SetMaxAIWarnTime(float fMaxWarnTime)
    {
        _fMaxAIWarnTime = fMaxWarnTime;
    }
    #endregion

    #endregion


    #region 死亡相关 =================================================

    public void f_Die()
    {
        _AIMachineManger.f_ChangeState((int)AI_EM.EM_AIState.Die);
        UpdateLog();
    }

    public bool f_IsDie()
    {
        ccMachineStateBase<ccMachineParamentBase> tAI_Die = _AIMachineManger.f_GetCurMachineState();
        if (tAI_Die != null)
        {
            if (tAI_Die.iId == (int)AI_EM.EM_AIState.Die)
            {
                return true;
            }
        }
        return false;
    }
    #endregion


    #region 移动相关 =================================================
    
    #region 追逐
    /// <summary>獲取追逐時長</summary>
    public float f_GetMaxAIChaseTime()
    {
        return _fMaxAIChaseTime;
    }

    /// <summary>
    /// 設定追逐時長
    /// </summary>
    /// <param name="fMaxChaseTime">追逐時長</param>
    public void f_SetMaxAIChaseTime(float fMaxChaseTime)
    {
        _fMaxAIChaseTime = fMaxChaseTime;
    }
    #endregion

    #region 巡邏
    /// <summary>獲取巡邏點列表</summary>
    public List<Vector3> f_GetPathWalk()
    {
        return _aPathObj;
    }

    /// <summary>
    /// 設定巡路位置
    /// </summary>
    /// <param name="szPath">巡邏點列表</param>
    /// /// <param name="szAnimId">動畫ID列表</param>
    /// /// <param name="szVarName">觸發指令列表</param>
    public void f_SetPathWalk(string szPath, string szAnimId = null, string szVarName = null)
    {
        string[] _Path = szPath.Split(',');
        foreach (string path in _Path)
        {
            _aPathObj.Add(GameMain.GetInstance().f_GeMapObjPosition(path));
        }

        string[] _AnimId = szAnimId.Split(',');
        //將未設置的巡邏點設為預設
        while (_AnimId.Length < _Path.Length)
        {
            szAnimId += ",";
            _AnimId = szAnimId.Split(',');
        }

        //設置各巡邏點動畫和觸發指令
        for (int i = 0; i < _Path.Length; i++)
        {
            if (_AnimId[i] == null || _AnimId[i] == "")
            {
                _aPathAnimId.Add(0);
            }
            else
            {
                _aPathAnimId.Add(ccMath.atoi(_AnimId[i]));
            }
        }
    }

    /// <summary>
    /// 增加巡邏點
    /// </summary>
    /// <param name="Path">巡邏點位置</param>
    /// <param name="iAnimId">播放動畫ID</param>
    /// <param name="VarId">觸發指令</param>
    public void f_AddPathWalk(Vector3 Path, int iAnimId = 0, int VarId = -1)
    {
        _aPathObj.Add(Path);
        _aPathAnimId.Add(iAnimId);
    }

    /// <summary>獲取巡邏點動畫ID</summary>
    public List<int> f_GetPathAnimId()
    {
        return _aPathAnimId;
    }

    /// <summary>獲取嵌入觸發指令ID</summary>
    public List<int> f_GetVarId()
    {
        return _szVarId;
    }

    /// <summary>獲取巡邏點停留時長</summary>
    public float f_GetAutoMoveSleepTime()
    {
        return _fAutoMoveSleepTime;
    }

    /// <summary>
    /// 設定巡邏點停留時長
    /// </summary>
    /// <param name="fAutoMoveTime">停留時長</param>
    public void f_SetAutoMoveSleepTime(float fAutoMoveTime)
    {
        _fAutoMoveSleepTime = fAutoMoveTime;
    }
    #endregion

    #endregion

}