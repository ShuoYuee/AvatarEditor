using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动作控制器
/// </summary>
public class BaseActionController
{
    /// <summary>
    /// 动作控制事件回调
    /// </summary>
    protected RoleModelCallbackEvent m_RoleModelCallbackEvent;
    protected Animator _Animator = null;
    protected AI_EM.EM_AIState m_CurAIStatic = AI_EM.EM_AIState.None;
    private ccList<ccCallback> _aCallBack_AttackEnd = new ccList<ccCallback>();

    protected BaseRoleControllV2 _BaseRoleControllV2;

    public BaseActionController(BaseRoleControllV2 tBaseRoleControl)
    {
        _BaseRoleControllV2 = tBaseRoleControl;
        GameObject oModel = tBaseRoleControl.f_GetRoleModel().Find("oModel").gameObject;
        _Animator = oModel.GetComponent<Animator>();
        InitRole(tBaseRoleControl);      
    }

    protected virtual void InitRole(BaseRoleControllV2 tBaseRoleControl)
    {

    }


    public AI_EM.EM_AIState f_GetCurAIStatic()
    {
        return m_CurAIStatic;
    }

    protected void RegAttackEndEvent(ccCallback tccCallBack)
    {
        if (_Animator == null)
        {
            MessageBox.ASSERT("Animator控制器未找到");
        }
        _aCallBack_AttackEnd.f_Add(tccCallBack);
        //AttackEndEvent[] aAttackEndEvent = _Animator.GetBehaviours<AttackEndEvent>();
        //if (aAttackEndEvent != null && aAttackEndEvent.Length > 0)
        //{
        //    for (int i = 0; i < aAttackEndEvent.Length; i++)
        //    {
        //        aAttackEndEvent[i].f_Reg_CallBack_AttackEnd(tccCallBack);
        //    }
        //}
    }

    public void OnAnimator(string strAnimator)
    {
        if (strAnimator == "Attack" || strAnimator == "Die")
        {
            for (int i = 0; i < _aCallBack_AttackEnd.Count; i++)
            {
                if (_aCallBack_AttackEnd[i] != null)
                {
                    ccCallback CallBackFun = _aCallBack_AttackEnd[i];
                    _aCallBack_AttackEnd.f_RemoveAt(i);
                    CallBackFun(null);
                }
            }
        }
    }


    //private void InitOtherRole(BaseRoleControllV2 tBaseRoleControl)
    //{ 
    //    animator = tBaseRoleControl.m_RoleModel.GetComponent<Animator>();
    //    m_RoleModelCallbackEvent = tBaseRoleControl.m_RoleModel.GetComponent<RoleModelCallbackEvent>();
    //    if (animator == null)
    //    {
    //        MessageBox.ASSERT("Animator控制器未找到");
    //    } else
    //    {
    //        animator.enabled = true;
    //    }
    //    if (m_RoleModelCallbackEvent == null)
    //    {
    //        MessageBox.ASSERT(tBaseRoleControl.m_iId + "的RoleModelCallbackEvent动作控制事件回调未找到");
    //    }
    //    m_RoleModelCallbackEvent.f_RegCallbackEvent(
    //        tBaseRoleControl.f_CallBack_Attacking,
    //        tBaseRoleControl.f_CallBack_AttacComplete,
    //        tBaseRoleControl.f_CallBack_ThrowStart,
    //        tBaseRoleControl.f_CallBack_ThrowEnd
    //        );

    //}

    //--------------------------------------------------------------
    public virtual void f_PlayAction(AI_EM.EM_AIState tAIState, ccCallback CallBackForActionEnd = null)
    {
        m_CurAIStatic = tAIState;

    }

    public virtual void f_PlayAnim(object Obj, ccCallback CallBack_Complete = null)
    {

    }

    public virtual void f_StopAttack()
    {

    }

    public virtual void f_ApplyRootMotion(bool bOpen)
    {
        _Animator.applyRootMotion = bOpen;
    }
    
    public virtual void f_Update()
    {

    }

    //--------------------------------------------------------------

    private float _fNormalizedTime = 0;

    /// <summary>
    /// 目前動畫播放完畢後執行動作
    /// </summary>
    /// <param name="ccCallback">待執行動作</param>
    /// <returns></returns>
    public bool OnAnimComplete(ccCallback ccCallback)
    {
        AnimatorStateInfo tAnimatorStateInfo;
        tAnimatorStateInfo = _Animator.GetCurrentAnimatorStateInfo(0);
        if (tAnimatorStateInfo.normalizedTime <= 0.5)
        {
            _fNormalizedTime = 0;
        }
        if (_fNormalizedTime < tAnimatorStateInfo.normalizedTime)
        {
            if (tAnimatorStateInfo.normalizedTime >= 1.0f)
            {
                _fNormalizedTime = 9999;
                ccTimeEvent.GetInstance().f_RegEvent(0f, false, null, ccCallback);
                return true;
            }
        }
        return false;
    }

    //--------------------------------------------------------------

    public void f_ChangeAnimator(Animator tAnimator)
    {
        if (tAnimator != null && _Animator != null)
        {
            _Animator.runtimeAnimatorController = tAnimator.runtimeAnimatorController;
            _Animator.avatar = tAnimator.avatar;
        }
    }
}

