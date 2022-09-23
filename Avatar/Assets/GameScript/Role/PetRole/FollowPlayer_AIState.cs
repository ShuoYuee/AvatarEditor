using System;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using DG.Tweening;

/// <summary>
/// 跟隨玩家狀態
/// </summary>
public class FollowPlayer_AIState : AI_BaseState
{
    private Transform _Target = null;
    private Vector3 _TargetPos;
    private Quaternion _TargetRot;

    public FollowPlayer_AIState(BaseRoleControllV2 tBaseRoleControl) : base((int)AI_EM.EM_AIState.Follow, tBaseRoleControl)
    {

    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        _TargetPos = _BaseRoleControl.transform.position;
        _TargetRot = _BaseRoleControl.transform.rotation;
        _Target = (Transform)Obj;
    }

    public override void f_Execute()
    {
        base.f_Execute();
        if (_Target == null) { return; }
        
        float fDistance = Vector3.Distance(_Target.position, _BaseRoleControl.transform.position);
        if (fDistance > 0.1f)
        {
            _TargetPos = _Target.position;
            _BaseRoleControl.transform.DOMove(_TargetPos, 0.5f);
        }

        float fRotDistance = Vector4.Distance(_Target.rotation.eulerAngles, _BaseRoleControl.transform.rotation.eulerAngles);
        if (fRotDistance > 0.1f)
        {
            _TargetRot = _Target.rotation;
            _BaseRoleControl.transform.DORotateQuaternion(_TargetRot, 0.5f);
        }
    }

    public override void f_Exit(object Obj = null)
    {
        base.f_Exit(Obj);
        _TargetPos = _BaseRoleControl.transform.position;
        _TargetRot = _BaseRoleControl.transform.rotation;
    }
}
