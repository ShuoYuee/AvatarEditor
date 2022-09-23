using UnityEngine;
using System.Collections.Generic;

public class DressRoleActionController : BaseActionController
{
    public DressRoleActionController(BaseRoleControllV2 tBaseRoleControl) : base(tBaseRoleControl)
    {
        MessageBox.DEBUG("角色動作器初始化：" + _BaseRoleControllV2.name);
        _Animator = tBaseRoleControl.GetComponent<Animator>();
    }

    public void f_SetPlayerAnimClip(string strAnim, int iLayer = 0)
    {
        if (strAnim == "") { return; }
        GameTools.f_PlayAnimator(_Animator, strAnim, 0.001f, false, iLayer);
    }

    public void f_SetPlayerAnimSpeed(float fSpeed, string strValue)
    {
        _Animator.SetFloat(strValue, fSpeed);
    }

    private Vector3 vLeftHandEroPos = Vector3.zero, vRightHandEroPos = Vector3.zero, vLeftFootEroPos = Vector3.zero, vRightFootEroPos = Vector3.zero;
    private float[] fEroArray = { 0, 0, 0, 0, 0, 0 };
    public void f_SetIKPos(AvatarIKGoal tGoal, Vector3 vPos)
    {
        if (tGoal == AvatarIKGoal.LeftFoot)
        {
            vLeftFootEroPos = _Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position - _Animator.GetIKPosition(AvatarIKGoal.LeftFoot);
            _Animator.SetIKPosition(tGoal, vPos - vLeftFootEroPos - new Vector3(0, fEroArray[4], 0));
            //_Animator.SetIKPosition(tGoal, vPos);
            _Animator.SetIKPositionWeight(tGoal, 1);
        }
        else if (tGoal == AvatarIKGoal.RightFoot)
        {
            vRightFootEroPos = _Animator.GetBoneTransform(HumanBodyBones.RightFoot).position - _Animator.GetIKPosition(AvatarIKGoal.RightFoot);
            _Animator.SetIKPosition(tGoal, vPos - vRightFootEroPos - new Vector3(0, fEroArray[5], 0));
            //_Animator.SetIKPosition(tGoal, vPos);
            _Animator.SetIKPositionWeight(tGoal, 1);
        }
        else if (tGoal == AvatarIKGoal.LeftHand)
        {
            vLeftHandEroPos = _Animator.GetBoneTransform(HumanBodyBones.LeftHand).position - _Animator.GetIKPosition(AvatarIKGoal.LeftHand);
            _Animator.SetIKPosition(tGoal, vPos - vLeftHandEroPos - new Vector3(fEroArray[0], 0, 0) - new Vector3(0, fEroArray[1], 0));
            //_Animator.SetIKPosition(tGoal, vPos);
            _Animator.SetIKPositionWeight(tGoal, 1);
        }
        else if (tGoal == AvatarIKGoal.RightHand)
        {
            vRightHandEroPos = _Animator.GetBoneTransform(HumanBodyBones.RightHand).position - _Animator.GetIKPosition(AvatarIKGoal.RightHand);
            _Animator.SetIKPosition(tGoal, vPos - vRightHandEroPos - new Vector3(fEroArray[2], 0, 0) - new Vector3(0, fEroArray[3], 0));
            //_Animator.SetIKPosition(tGoal, vPos);
            _Animator.SetIKPositionWeight(tGoal, 1);
        }
    }

    public void f_SetIKRot(AvatarIKGoal tGoal, Quaternion qRot)
    {
        _Animator.SetIKRotation(tGoal, qRot);
        _Animator.SetIKRotationWeight(tGoal, 1);
    }

    public bool f_CheckAnim()
    {
        return _Animator != null;
    }

    public void f_SetEroPos(int iIndex, float fPos, bool bAdd = false)
    {
        if (_Animator == null) { return; }
        if (bAdd)
        {
            fEroArray[iIndex] += fPos;
        }
        else
        {
            fEroArray[iIndex] = fPos;
        }
    }

    public Transform f_GetAvatarBone(HumanBodyBones tBones)
    {
        if (_Animator == null) { return null; }
        return _Animator.GetBoneTransform(tBones);
    }

    public Vector3 f_GetAvatarIK(AvatarIKGoal tBones)
    {
        if (_Animator == null) { return Vector3.zero; }
        return _Animator.GetIKPosition(tBones);
    }
}
