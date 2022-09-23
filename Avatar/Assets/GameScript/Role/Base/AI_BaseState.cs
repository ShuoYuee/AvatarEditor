using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AI状态机基类
/// </summary>
public class AI_BaseState : ccMachineStateBase<ccMachineParamentBase>
{
    protected BaseRoleControllV2 _BaseRoleControl;

    public AI_BaseState(int tiId, BaseRoleControllV2 tBaseRoleControl)
        : base(tiId)
    {
        _BaseRoleControl = tBaseRoleControl;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);

        _BaseRoleControl.f_PlayAction(f_GetAIAction());
    }

    private AI_EM.EM_AIState f_GetAIAction()
    {
        return (AI_EM.EM_AIState)_iId;
    }
    
    protected void ChangeFace(Vector3 Pos)
    {
        iTween.LookTo(_BaseRoleControl.gameObject, Pos, 1.5f);
    }
}