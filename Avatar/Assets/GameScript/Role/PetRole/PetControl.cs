using System;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>
/// 寵物控制器
/// </summary>
public class PetControl : BaseRoleControllV2
{
    private AvaterPartDT _AvaterPartDT;
    private Transform _Target;

    public void f_Init(BaseActionController tBaseActionController, AvaterPartDT tAvaterPartDT)
    {
        _BaseActionController = tBaseActionController;
        _BaseAI = new PetAIControll(this);
        _AvaterPartDT = tAvaterPartDT;

        InitAI();
        InitMessage();
        _bInitOK = true;
    }

    protected override void InitAI()
    {
        base.InitAI();
        RegNewAIState(new FollowPlayer_AIState(this));
    }

    public void f_SetTarget(Transform Target)
    {
        _Target = Target;
        transform.position = _Target.position;
        transform.SetParent(_Target.parent);
        f_ChangeAIState(AI_EM.EM_AIState.Follow, _Target);
    }

    public void f_SetPetAnimClip(string strAnim)
    {
        ((PetActionController)_BaseActionController).f_SetPlayerAnimClip(strAnim);
    }

    public byte[] f_GetAvaterBuf()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(_AvaterPartDT.iId);
        return tCreateSocketBuf.f_GetBuf();
    }
}