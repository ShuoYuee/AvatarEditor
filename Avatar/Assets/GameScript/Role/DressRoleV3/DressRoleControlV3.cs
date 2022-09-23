using ccU3DEngine;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 換裝角色控制器
/// </summary>
public class DressRoleControlV3 : DressRoleControl
{
    public DeformerManager _DeformerManager;
    private Transform _oModel = null, _Pelvis = null;
    public override void f_Init(int iId, BaseActionController tBaseActionController, Transform InitPos)
    {
        base.f_Init(iId, tBaseActionController, InitPos);
        _oModel = transform.Find("oModel");
        if (_DeformerManager == null)
        {
            _DeformerManager = gameObject.AddComponent<DeformerManager>();
        }
        _DeformerManager.f_Init(this, _DressRoleActionController);

        _Pelvis = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Destroy(_Pelvis.gameObject.GetComponent<Collider>());
        Destroy(_Pelvis.gameObject.GetComponent<Renderer>());
        Destroy(_Pelvis.gameObject.GetComponent<MeshFilter>());
        _Pelvis.name = "PelvisControl";
        Transform tPelvis = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.Hips);
        _Pelvis.SetParent(tPelvis.parent);
        _Pelvis.position = tPelvis.position;
        _Pelvis.localRotation = Quaternion.Euler(Vector3.zero);
        tPelvis.SetParent(_Pelvis);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //IK動畫
        if (!_bInitOK) { return; }
        if (!_BicycleAvater.f_GetInit()) { return; }
        if (_bHandIK)
        {
            _DressRoleActionController.f_SetIKPos(AvatarIKGoal.RightHand, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.RightHandle.ToString()).position);
            _DressRoleActionController.f_SetIKRot(AvatarIKGoal.RightHand, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.RightHandle.ToString()).rotation);
            _DressRoleActionController.f_SetIKPos(AvatarIKGoal.LeftHand, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.LeftHandle.ToString()).position);
            _DressRoleActionController.f_SetIKRot(AvatarIKGoal.LeftHand, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.LeftHandle.ToString()).rotation);
        }
        if (_bFootIK)
        {
            _DressRoleActionController.f_SetIKPos(AvatarIKGoal.RightFoot, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.RightFootBoard.ToString()).position);
            _DressRoleActionController.f_SetIKRot(AvatarIKGoal.RightFoot, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.RightFootBoard.ToString()).rotation);
            _DressRoleActionController.f_SetIKPos(AvatarIKGoal.LeftFoot, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.LeftFootBoard.ToString()).position);
            _DressRoleActionController.f_SetIKRot(AvatarIKGoal.LeftFoot, _BicycleAvater.f_GetBone(GameEM.EM_BikeIK.LeftFootBoard.ToString()).rotation);
        }
    }

    //private Transform tCurBike = null;
    //private Transform tBikeParent = null, tHeadParent, tLeftHandParent, tRightHandParent, tLeftArmParent, tRightArmParent, tLeftFootParent, tRightFootParent, tLeftLowLegParent, tRightLowLegParent;
    //private Vector3 vOriginBikePos = Vector3.zero, vOriginHeadPos = Vector3.zero, vOriginLeftHandPos = Vector3.zero, vOriginRightHandPos = Vector3.zero, vOriginLeftArmPos = Vector3.zero, vOriginRightArmPos = Vector3.zero, vOriginLeftFootPos = Vector3.zero, vOriginRightFootPos = Vector3.zero, vOriginLeftLowLegPos = Vector3.zero, vOriginRightLowLegPos = Vector3.zero;
    //private Quaternion vOriginBikeRot = Quaternion.identity, vOriginHeadRot = Quaternion.identity, vOriginLeftHandRot = Quaternion.identity, vOriginRightHandRot = Quaternion.identity, vOriginLeftArmRot = Quaternion.identity, vOriginRightArmRot = Quaternion.identity, vOriginLeftFootRot = Quaternion.identity, vOriginRightFootRot = Quaternion.identity, vOriginLeftLowLegRot = Quaternion.identity, vOriginRightLowLegRot = Quaternion.identity;
    //private string[] _strBikeIK = { "LeftFootBoard2", "RightFootBoard2", "LeftHandle2", "RightHandle2" };
    //public override void f_Deformer(string strEM, float fValue)
    //{//身形變形(骨架縮放)
    //    if (strEM == GameEM.EM_Deformer.BodyHeight.ToString())
    //    {//身高變形(0.85-1.15)
    //        tCurBike = _BicycleAvater.f_GetCurBike();
    //        if (tCurBike != null)
    //        {//將腳踏車移出外層
    //            vOriginBikePos = tCurBike.localPosition;
    //            vOriginBikeRot = tCurBike.localRotation;
    //            tBikeParent = tCurBike.parent;
    //            tCurBike.SetParent(null);
    //        }
    //        //將頭部移出外層
    //        Transform tHead = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.Head);
    //        vOriginHeadPos = tHead.localPosition;
    //        vOriginHeadRot = tHead.localRotation;
    //        tHeadParent = tHead.parent;
    //        tHead.SetParent(null);
    //        //縮放
    //        float fX = fValue / 100 * 0.3f;
    //        float fY = fValue / 100 * 0.3f;
    //        float fScaleX = fX + 0.85f;
    //        float fScaleY = fY + 0.85f;
    //        _oModel.localScale = new Vector3(fScaleX, fScaleY, fScaleY);
    //        //_BoneManager.m_Pelvis.transform.parent.localScale = new Vector3(fScaleX, fScaleY, fScaleY);
    //        //將腳踏車歸位
    //        if (tCurBike != null)
    //        {
    //            tCurBike.SetParent(tBikeParent);
    //            tCurBike.localPosition = vOriginBikePos;
    //            tCurBike.localRotation = vOriginBikeRot;
    //            f_InitPos();
    //            f_InitPos(_vOriginRole);
    //        }
    //        //將頭部歸位
    //        tHead.SetParent(tHeadParent);
    //        tHead.localPosition = vOriginHeadPos;
    //        tHead.localRotation = vOriginHeadRot;
    //    }
    //    else if (strEM == GameEM.EM_Deformer.Spine.ToString())
    //    {//脊椎長(0.8-1.2)
    //        //將手臂移出外層
    //        Transform tLeftArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftUpperArm);
    //        Transform tRightArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightUpperArm);
    //        tLeftArmParent = tLeftArm.parent;
    //        vOriginLeftArmPos = tLeftArm.localPosition;
    //        vOriginLeftArmRot = tLeftArm.localRotation;
    //        tRightArmParent = tRightArm.parent;
    //        vOriginRightArmPos = tRightArm.localPosition;
    //        vOriginRightArmRot = tRightArm.localRotation;
    //        tLeftArm.SetParent(null);
    //        tRightArm.SetParent(null);
    //        //將頭部移出外層
    //        Transform tHead = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.Head);
    //        vOriginHeadPos = tHead.localPosition;
    //        vOriginHeadRot = tHead.localRotation;
    //        tHeadParent = tHead.parent;
    //        tHead.SetParent(null);
    //        //縮放
    //        float fX = fValue / 100 * 0.4f;
    //        float fY = fValue / 100 * 0.4f;
    //        float fScaleX = fX + 0.8f;
    //        float fScaleY = fY + 0.8f;
    //        Transform tSpine = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.Spine);
    //        tSpine.localScale = new Vector3(fScaleX, fScaleY, tSpine.localScale.z);
    //        //_BoneManager.m_Spine.transform.parent.localScale = new Vector3(fScaleX, fScaleY, _BoneManager.m_Spine.transform.parent.localScale.z);
    //        //將手臂歸位
    //        tLeftArm.SetParent(tLeftArmParent);
    //        tLeftArm.localPosition = vOriginLeftArmPos;
    //        tLeftArm.localRotation = vOriginLeftArmRot;
    //        tRightArm.SetParent(tRightArmParent);
    //        tRightArm.localPosition = vOriginRightArmPos;
    //        tRightArm.localRotation = vOriginRightArmRot;
    //        //將頭部歸位
    //        tHead.SetParent(tHeadParent);
    //        tHead.localPosition = vOriginHeadPos;
    //        tHead.localRotation = vOriginHeadRot;
    //    }
    //    else if (strEM == GameEM.EM_Deformer.LegHight.ToString())
    //    {//腿長(0.8-1.2)
    //        //縮放
    //        //float fX = fValue / 100 * 0.4f;
    //        //float fY = fValue / 100 * 0.4f;
    //        //float fScaleX = fX + 0.8f;
    //        //float fScaleY = fY + 0.8f;
    //        //Transform tLeftLowLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftLowerLeg);
    //        //Transform tRightLowLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightLowerLeg);
    //        //Transform tLeftLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftUpperLeg);
    //        //Transform tRightLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightUpperLeg);
    //        //tLeftLeg.localScale = new Vector3(fScaleX, fScaleY, tLeftLeg.localScale.z);
    //        //tRightLeg.localScale = new Vector3(fScaleX, fScaleY, tRightLeg.localScale.z);
    //        //tLeftLowLeg.localScale = new Vector3(fScaleX, fScaleY, tLeftLowLeg.localScale.z);
    //        //tRightLowLeg.localScale = new Vector3(fScaleX, fScaleY, tRightLowLeg.localScale.z);

    //        //將腳部移出外層
    //        Transform tLeftFoot = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftFoot);
    //        Transform tRightFoot = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightFoot);
    //        tLeftFootParent = tLeftFoot.parent;
    //        vOriginLeftFootPos = tLeftFoot.localPosition;
    //        vOriginLeftFootRot = tLeftFoot.localRotation;
    //        tRightFootParent = tRightFoot.parent;
    //        vOriginRightFootPos = tRightFoot.localPosition;
    //        vOriginRightFootRot = tRightFoot.localRotation;
    //        tLeftFoot.SetParent(null);
    //        tRightFoot.SetParent(null);
    //        //將小腿移出外層
    //        Transform tLeftLowLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftLowerLeg);
    //        Transform tRightLowLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightLowerLeg);
    //        tLeftLowLegParent = tLeftLowLeg.parent;
    //        vOriginLeftLowLegPos = tLeftLowLeg.localPosition;
    //        vOriginLeftLowLegRot = tLeftLowLeg.localRotation;
    //        tRightLowLegParent = tRightLowLeg.parent;
    //        vOriginRightLowLegPos = tRightLowLeg.localPosition;
    //        vOriginRightLowLegRot = tRightLowLeg.localRotation;
    //        tLeftLowLeg.SetParent(null);
    //        tRightLowLeg.SetParent(null);
    //        //將腳踏車移出外層
    //        tCurBike = _BicycleAvater.f_GetCurBike();
    //        if (tCurBike != null)
    //        {
    //            vOriginBikePos = tCurBike.localPosition;
    //            vOriginBikeRot = tCurBike.localRotation;
    //            tBikeParent = tCurBike.parent;
    //            tCurBike.SetParent(null);
    //        }
    //        //縮放
    //        float fX = fValue / 100 * 0.4f;
    //        float fY = fValue / 100 * 0.4f;
    //        float fScaleX = fX + 0.8f;
    //        float fScaleY = fY + 0.8f;
    //        Transform tLeftLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftUpperLeg);
    //        Transform tRightLeg = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightUpperLeg);
    //        //Transform tLeftLeg = _BoneManager.m_LThigh.transform.parent;
    //        //Transform tRightLeg = _BoneManager.m_RThigh.transform.parent;
    //        tLeftLeg.localScale = new Vector3(fScaleX, fScaleY, tLeftLeg.localScale.z);
    //        tRightLeg.localScale = new Vector3(fScaleX, fScaleY, tRightLeg.localScale.z);
    //        tLeftLowLeg.localScale = new Vector3(fScaleX, fScaleY, tLeftLowLeg.localScale.z);
    //        tRightLowLeg.localScale = new Vector3(fScaleX, fScaleY, tRightLowLeg.localScale.z);
    //        //將腳踏車歸位
    //        if (tCurBike != null)
    //        {
    //            tCurBike.SetParent(tBikeParent);
    //            tCurBike.localPosition = vOriginBikePos;
    //            tCurBike.localRotation = vOriginBikeRot;
    //            if (!tCurBike.gameObject.activeSelf)
    //            {
    //                Transform tToe = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftToes);
    //                float fDisToe = tToe.position.y - _vOriginRole.y;
    //                transform.position -= new Vector3(0, fDisToe, 0);
    //            }
    //        }
    //        else
    //        {
    //            Transform tToe = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftToes);
    //            float fDisToe = tToe.position.y - _vOriginRole.y;
    //            transform.position -= new Vector3(0, fDisToe, 0);
    //        }
    //        //將小腿歸位
    //        tLeftLowLeg.SetParent(tLeftLowLegParent);
    //        tLeftLowLeg.localPosition = vOriginLeftLowLegPos;
    //        tLeftLowLeg.localRotation = vOriginLeftLowLegRot;
    //        tRightLowLeg.SetParent(tRightLowLegParent);
    //        tRightLowLeg.localPosition = vOriginRightLowLegPos;
    //        tRightLowLeg.localRotation = vOriginRightLowLegRot;
    //        //將腳部歸位
    //        tLeftFoot.SetParent(tLeftFootParent);
    //        tLeftFoot.localPosition = vOriginLeftFootPos;
    //        tLeftFoot.localRotation = vOriginLeftFootRot;
    //        tRightFoot.SetParent(tRightFootParent);
    //        tRightFoot.localPosition = vOriginRightFootPos;
    //        tRightFoot.localRotation = vOriginRightFootRot;
    //        //計算IK誤差值
    //        _DressRoleActionController.f_SetEroPos(4, -fY / 10);
    //        _DressRoleActionController.f_SetEroPos(5, fY / 10);
    //    }
    //    else if (strEM == GameEM.EM_Deformer.HandHight.ToString())
    //    {//手臂長(0.85-1.15)
    //        //將手掌移出外層
    //        Transform tLeftHand = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftHand);
    //        Transform tRightHand = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightHand);
    //        tLeftHandParent = tLeftHand.parent;
    //        vOriginLeftHandPos = tLeftHand.localPosition;
    //        vOriginLeftHandRot = tLeftHand.localRotation;
    //        tRightHandParent = tRightHand.parent;
    //        vOriginRightHandPos = tRightHand.localPosition;
    //        vOriginRightHandRot = tRightHand.localRotation;
    //        tLeftHand.SetParent(null);
    //        tRightHand.SetParent(null);
    //        //將上臂移出外層
    //        Transform tLeftLowArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftLowerArm);
    //        Transform tRightLowArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightLowerArm);
    //        tLeftArmParent = tLeftLowArm.parent;
    //        vOriginLeftArmPos = tLeftLowArm.localPosition;
    //        vOriginLeftArmRot = tLeftLowArm.localRotation;
    //        tRightArmParent = tRightLowArm.parent;
    //        vOriginRightArmPos = tRightLowArm.localPosition;
    //        vOriginRightArmRot = tRightLowArm.localRotation;
    //        tLeftLowArm.SetParent(null);
    //        tRightLowArm.SetParent(null);
    //        //縮放
    //        float fX = fValue / 100 * 0.3f;
    //        float fY = fValue / 100 * 0.3f;
    //        float fScaleX = fX + 0.85f;
    //        float fScaleY = fY + 0.85f;
    //        Transform tLeftUpArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftUpperArm);
    //        Transform tRightUpArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightUpperArm);
    //        //Transform tLeftUpArm = _BoneManager.m_LArm.transform.parent;
    //        //Transform tRightUpArm = _BoneManager.m_RArm.transform.parent;
    //        tLeftUpArm.localScale = new Vector3(fScaleX, fScaleY, tLeftUpArm.localScale.z);
    //        tLeftLowArm.localScale = new Vector3(fScaleX, fScaleY, tLeftLowArm.localScale.z);
    //        tRightUpArm.localScale = new Vector3(fScaleX, fScaleY, tRightUpArm.localScale.z);
    //        tRightLowArm.localScale = new Vector3(fScaleX, fScaleY, tRightLowArm.localScale.z);
    //        //將上臂歸位
    //        tLeftLowArm.SetParent(tLeftArmParent);
    //        tLeftLowArm.localPosition = vOriginLeftArmPos;
    //        tLeftLowArm.localRotation = vOriginLeftArmRot;
    //        tRightLowArm.SetParent(tRightArmParent);
    //        tRightLowArm.localPosition = vOriginRightArmPos;
    //        tRightLowArm.localRotation = vOriginRightArmRot;
    //        //將手掌歸位
    //        tLeftHand.SetParent(tLeftHandParent);
    //        tLeftHand.localPosition = vOriginLeftHandPos;
    //        tLeftHand.localRotation = vOriginLeftHandRot;
    //        tRightHand.SetParent(tRightHandParent);
    //        tRightHand.localPosition = vOriginRightHandPos;
    //        tRightHand.localRotation = vOriginRightHandRot;
    //        //計算IK誤差值
    //        _DressRoleActionController.f_SetEroPos(1, (fY - 0.15f) / 10f);
    //        _DressRoleActionController.f_SetEroPos(3, (fY - 0.15f) / 10f);
    //    }
    //    else if (strEM == GameEM.EM_Deformer.Shoulder.ToString())
    //    {//肩寬(0.85-1.15)
    //        //將手臂移出外層
    //        Transform tLeftArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftUpperArm);
    //        Transform tRightArm = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightUpperArm);
    //        tLeftArmParent = tLeftArm.parent;
    //        vOriginLeftArmPos = tLeftArm.localPosition;
    //        vOriginLeftArmRot = tLeftArm.localRotation;
    //        tRightArmParent = tRightArm.parent;
    //        vOriginRightArmPos = tRightArm.localPosition;
    //        vOriginRightArmRot = tRightArm.localRotation;
    //        tLeftArm.SetParent(null);
    //        tRightArm.SetParent(null);
    //        //縮放
    //        float fX = fValue / 100 * 0.3f;
    //        float fY = fValue / 100 * 0.3f;
    //        float fScaleX = fX + 0.85f;
    //        float fScaleY = fY + 0.85f;
    //        Transform tLeftShoulder = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftShoulder);
    //        Transform tRightShoulder = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightShoulder);
    //        //Transform tLeftShoulder = _BoneManager.m_LClavicle.transform.parent;
    //        //Transform tRightShoulder = _BoneManager.m_RClavicle.transform.parent;
    //        tLeftShoulder.localScale = new Vector3(fScaleX, tLeftShoulder.localScale.y, tLeftShoulder.localScale.z);
    //        tRightShoulder.localScale = new Vector3(fScaleX, tRightShoulder.localScale.y, tLeftShoulder.localScale.z);
    //        //將手臂歸位
    //        tLeftArm.SetParent(tLeftArmParent);
    //        tLeftArm.localPosition = vOriginLeftArmPos;
    //        tLeftArm.localRotation = vOriginLeftArmRot;
    //        tRightArm.SetParent(tRightArmParent);
    //        tRightArm.localPosition = vOriginRightArmPos;
    //        tRightArm.localRotation = vOriginRightArmRot;
    //        //計算IK誤差值
    //        _DressRoleActionController.f_SetEroPos(0, (fX - 0.15f) * 0.03f);
    //        _DressRoleActionController.f_SetEroPos(2, -(fX - 0.15f) * 0.03f);
    //    }
    //    base.f_Deformer(strEM, fValue);
    //}

    public override void f_Deformer(string strEM, float fValue)
    {
        _DeformerManager.f_Deformer(strEM, fValue);
        base.f_Deformer(strEM, fValue);
    }

    public override byte[] f_GetDeformerBuf()
    {
        return _DeformerManager.f_GetDeformerBuf();
    }

    public override float f_GetDeformerValue(string strEM)
    {
        return _DeformerManager.f_GetDeformerValue(strEM);
    }
}