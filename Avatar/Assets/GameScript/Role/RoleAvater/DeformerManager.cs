using System.Collections.Generic;
using UnityEngine;

public class DeformerManager : MonoBehaviour
{
    private DressRoleControl _RoleControl;
    private DressRoleActionController _DressRoleActionController;
    private Transform _oModel = null, _Pelvis = null;

    /// <summary>變形器</summary>
    protected BoneDeformer _BoneDeformer = new BoneDeformer();

    public class OriginTransform
    {
        public Transform tBone = null;
        public Transform tParent = null;
        public Vector3 vOriginPos = Vector3.zero;
        public Quaternion vOriginRot = Quaternion.identity;
        public OriginTransform(Transform bone)
        {
            tBone = bone;
        }

        public void f_InitPos()
        {//紀錄原始座標
            tParent = tBone.parent;
            vOriginPos = tBone.localPosition;
            vOriginRot = tBone.localRotation;
        }

        public void f_RePos()
        {//重置座標
            tBone.localPosition = vOriginPos;
            tBone.localRotation = vOriginRot;
        }
    }
    private Dictionary<string, OriginTransform> _dicOriginTransform = new Dictionary<string, OriginTransform>();

    public void f_Init(DressRoleControl tRoleControllV2, DressRoleActionController tRoleActionController)
    {
        _RoleControl = tRoleControllV2;
        tCurBike = GameTools.f_GetGameObject(transform, GameEM.EM_RolePart.bike.ToString()).transform;
        _DressRoleActionController = tRoleActionController;

        _BoneDeformer.f_Init(_RoleControl);
        _oModel = transform.Find("oModel");
        f_InitDeformer();
    }

    private void f_InitDeformer()
    {
        _dicOriginTransform.Add("Bike", new OriginTransform(tCurBike));
        _dicOriginTransform.Add("Head", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.Head)));
        _dicOriginTransform.Add("Spine", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.Spine)));

        _dicOriginTransform.Add("LeftHand", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftHand)));
        _dicOriginTransform.Add("RightHand", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightHand)));
        _dicOriginTransform.Add("LeftUpArm", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftUpperArm)));
        _dicOriginTransform.Add("RightUpArm", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightUpperArm)));
        _dicOriginTransform.Add("LeftLowArm", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftLowerArm)));
        _dicOriginTransform.Add("RightLowArm", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightLowerArm)));
        _dicOriginTransform.Add("LeftShoulder", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftShoulder)));
        _dicOriginTransform.Add("RightShoulder", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightShoulder)));

        _dicOriginTransform.Add("LeftFoot", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftFoot)));
        _dicOriginTransform.Add("RightFoot", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightFoot)));
        _dicOriginTransform.Add("LeftUpLeg", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftUpperLeg)));
        _dicOriginTransform.Add("RightUpLeg", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightUpperLeg)));
        _dicOriginTransform.Add("LeftLowLeg", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftLowerLeg)));
        _dicOriginTransform.Add("RightLowLeg", new OriginTransform(_DressRoleActionController.f_GetAvatarBone(HumanBodyBones.RightLowerLeg)));
    }

    private Transform tCurBike = null;
    public void f_Deformer(string strEM, float fValue)
    {//身形變形(骨架縮放)
        if (strEM == GameEM.EM_Deformer.BodyHeight.ToString())
        {//身高變形(0.85-1.15)
            if (tCurBike == null) { tCurBike = GameTools.f_GetGameObject(transform, GameEM.EM_RolePart.bike.ToString()).transform; }
            if (tCurBike != null)
            {//將腳踏車移出外層
                _dicOriginTransform["Bike"].f_InitPos();
                tCurBike.SetParent(null);
            }
            //將頭部移出外層
            _dicOriginTransform["Head"].f_InitPos();
            _dicOriginTransform["Head"].tBone.SetParent(null);
            //縮放
            float fvalue = fValue / 100 * 0.3f;
            float fScale = fvalue + 0.85f;
            _oModel.localScale = new Vector3(fScale, fScale, fScale);
            //將腳踏車歸位
            if (tCurBike != null)
            {
                _dicOriginTransform["Bike"].tBone.SetParent(_dicOriginTransform["Bike"].tParent);
                _dicOriginTransform["Bike"].f_RePos();
                _RoleControl.f_InitBikePos();
                _RoleControl.f_InitPos();
            }
            //將頭部歸位
            _dicOriginTransform["Head"].tBone.SetParent(_dicOriginTransform["Head"].tParent);
            _dicOriginTransform["Head"].f_RePos();
        }
        else if (strEM == GameEM.EM_Deformer.Spine.ToString())
        {//脊椎長(0.8-1.2)
            //將手臂移出外層
            _dicOriginTransform["LeftUpArm"].f_InitPos();
            _dicOriginTransform["RightUpArm"].f_InitPos();
            _dicOriginTransform["LeftUpArm"].tBone.SetParent(null);
            _dicOriginTransform["RightUpArm"].tBone.SetParent(null);
            //將頭部移出外層
            _dicOriginTransform["Head"].f_InitPos();
            _dicOriginTransform["Head"].tBone.SetParent(null);
            //縮放
            float fX = fValue / 100 * 0.4f;
            float fY = fValue / 100 * 0.4f;
            float fScaleX = fX + 0.8f;
            float fScaleY = fY + 0.8f;
            _dicOriginTransform["Spine"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["Spine"].tBone.localScale.z);
            //將手臂歸位
            _dicOriginTransform["LeftUpArm"].tBone.SetParent(_dicOriginTransform["LeftUpArm"].tParent);
            _dicOriginTransform["LeftUpArm"].f_RePos();
            _dicOriginTransform["RightUpArm"].tBone.SetParent(_dicOriginTransform["RightUpArm"].tParent);
            _dicOriginTransform["RightUpArm"].f_RePos();
            //將頭部歸位
            _dicOriginTransform["Head"].tBone.SetParent(_dicOriginTransform["Head"].tParent);
            _dicOriginTransform["Head"].f_RePos();
        }
        else if (strEM == GameEM.EM_Deformer.LegHight.ToString())
        {//腿長(0.8-1.2)
            //將腳部移出外層
            _dicOriginTransform["LeftFoot"].f_InitPos();
            _dicOriginTransform["RightFoot"].f_InitPos();
            _dicOriginTransform["LeftFoot"].tBone.SetParent(null);
            _dicOriginTransform["RightFoot"].tBone.SetParent(null);
            //將小腿移出外層
            _dicOriginTransform["LeftLowLeg"].f_InitPos();
            _dicOriginTransform["RightLowLeg"].f_InitPos();
            _dicOriginTransform["LeftLowLeg"].tBone.SetParent(null);
            _dicOriginTransform["RightLowLeg"].tBone.SetParent(null);
            //將腳踏車移出外層
            if (tCurBike == null) { tCurBike = GameTools.f_GetGameObject(transform, GameEM.EM_RolePart.bike.ToString()).transform; }
            if (tCurBike != null)
            {
                _dicOriginTransform["Bike"].f_InitPos();
                tCurBike.SetParent(null);
            }
            //縮放
            float fX = fValue / 100 * 0.4f;
            float fY = fValue / 100 * 0.4f;
            float fScaleX = fX + 0.8f;
            float fScaleY = fY + 0.8f;
            _dicOriginTransform["LeftUpLeg"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["LeftUpLeg"].tBone.localScale.z);
            _dicOriginTransform["RightUpLeg"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["RightUpLeg"].tBone.localScale.z);
            _dicOriginTransform["LeftLowLeg"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["LeftLowLeg"].tBone.localScale.z);
            _dicOriginTransform["RightLowLeg"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["RightLowLeg"].tBone.localScale.z);
            //將腳踏車歸位
            if (tCurBike != null)
            {
                _dicOriginTransform["Bike"].tBone.SetParent(_dicOriginTransform["Bike"].tParent);
                _dicOriginTransform["Bike"].f_RePos();
                if (!tCurBike.gameObject.activeSelf)
                {
                    Transform tToe = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftToes);
                    float fDisToe = tToe.position.y - _RoleControl._vOriginRoleGet.y;
                    transform.position -= new Vector3(0, fDisToe, 0);
                }
            }
            else
            {
                Transform tToe = _DressRoleActionController.f_GetAvatarBone(HumanBodyBones.LeftToes);
                float fDisToe = tToe.position.y - _RoleControl._vOriginRoleGet.y;
                transform.position -= new Vector3(0, fDisToe, 0);
            }
            //將小腿歸位
            _dicOriginTransform["LeftLowLeg"].tBone.SetParent(_dicOriginTransform["LeftLowLeg"].tParent);
            _dicOriginTransform["LeftLowLeg"].f_RePos();
            _dicOriginTransform["RightLowLeg"].tBone.SetParent(_dicOriginTransform["RightLowLeg"].tParent);
            _dicOriginTransform["RightLowLeg"].f_RePos();
            //將腳部歸位
            _dicOriginTransform["LeftFoot"].tBone.SetParent(_dicOriginTransform["LeftFoot"].tParent);
            _dicOriginTransform["LeftFoot"].f_RePos();
            _dicOriginTransform["RightFoot"].tBone.SetParent(_dicOriginTransform["RightFoot"].tParent);
            _dicOriginTransform["RightFoot"].f_RePos();
            //計算IK誤差值
            _DressRoleActionController.f_SetEroPos(4, -fY / 10);
            _DressRoleActionController.f_SetEroPos(5, fY / 10);
        }
        else if (strEM == GameEM.EM_Deformer.HandHight.ToString())
        {//手臂長(0.85-1.15)
            //將手掌移出外層
            _dicOriginTransform["LeftHand"].f_InitPos();
            _dicOriginTransform["RightHand"].f_InitPos();
            _dicOriginTransform["LeftHand"].tBone.SetParent(null);
            _dicOriginTransform["RightHand"].tBone.SetParent(null);
            //將上臂移出外層
            _dicOriginTransform["LeftLowArm"].f_InitPos();
            _dicOriginTransform["RightLowArm"].f_InitPos();
            _dicOriginTransform["LeftLowArm"].tBone.SetParent(null);
            _dicOriginTransform["RightLowArm"].tBone.SetParent(null);
            //縮放
            float fX = fValue / 100 * 0.3f;
            float fY = fValue / 100 * 0.3f;
            float fScaleX = fX + 0.85f;
            float fScaleY = fY + 0.85f;
            _dicOriginTransform["LeftUpArm"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["LeftUpArm"].tBone.localScale.z);
            _dicOriginTransform["LeftLowArm"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["LeftLowArm"].tBone.localScale.z);
            _dicOriginTransform["RightUpArm"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["RightUpArm"].tBone.localScale.z);
            _dicOriginTransform["RightLowArm"].tBone.localScale = new Vector3(fScaleX, fScaleY, _dicOriginTransform["RightLowArm"].tBone.localScale.z);
            //將上臂歸位
            _dicOriginTransform["LeftLowArm"].tBone.SetParent(_dicOriginTransform["LeftLowArm"].tParent);
            _dicOriginTransform["LeftLowArm"].f_RePos();
            _dicOriginTransform["RightLowArm"].tBone.SetParent(_dicOriginTransform["RightLowArm"].tParent);
            _dicOriginTransform["RightLowArm"].f_RePos();
            //將手掌歸位
            _dicOriginTransform["LeftHand"].tBone.SetParent(_dicOriginTransform["LeftHand"].tParent);
            _dicOriginTransform["LeftHand"].f_RePos();
            _dicOriginTransform["RightHand"].tBone.SetParent(_dicOriginTransform["RightHand"].tParent);
            _dicOriginTransform["RightHand"].f_RePos();
            //計算IK誤差值
            _DressRoleActionController.f_SetEroPos(1, (fY - 0.15f) / 10f);
            _DressRoleActionController.f_SetEroPos(3, (fY - 0.15f) / 10f);
        }
        else if (strEM == GameEM.EM_Deformer.Shoulder.ToString())
        {//肩寬(0.85-1.15)
            //將手臂移出外層
            _dicOriginTransform["LeftUpArm"].f_InitPos();
            _dicOriginTransform["RightUpArm"].f_InitPos();
            _dicOriginTransform["LeftUpArm"].tBone.SetParent(null);
            _dicOriginTransform["RightUpArm"].tBone.SetParent(null);
            //縮放
            float fX = fValue / 100 * 0.3f;
            float fY = fValue / 100 * 0.3f;
            float fScaleX = fX + 0.85f;
            float fScaleY = fY + 0.85f;
            _dicOriginTransform["LeftShoulder"].tBone.localScale = new Vector3(fScaleX, _dicOriginTransform["LeftShoulder"].tBone.localScale.y, _dicOriginTransform["LeftShoulder"].tBone.localScale.z);
            _dicOriginTransform["RightShoulder"].tBone.localScale = new Vector3(fScaleX, _dicOriginTransform["RightShoulder"].tBone.localScale.y, _dicOriginTransform["RightShoulder"].tBone.localScale.z);
            //將手臂歸位
            _dicOriginTransform["LeftUpArm"].tBone.SetParent(_dicOriginTransform["LeftUpArm"].tParent);
            _dicOriginTransform["LeftUpArm"].f_RePos();
            _dicOriginTransform["RightUpArm"].tBone.SetParent(_dicOriginTransform["RightUpArm"].tParent);
            _dicOriginTransform["RightUpArm"].f_RePos();
            //計算IK誤差值
            _DressRoleActionController.f_SetEroPos(0, (fX - 0.15f) * 0.03f);
            _DressRoleActionController.f_SetEroPos(2, -(fX - 0.15f) * 0.03f);
        }
        _BoneDeformer.f_Deformer(strEM, fValue);
    }

    /// <summary>取得變形資料  12Int</summary>
    public virtual byte[] f_GetDeformerBuf()
    {
        return _BoneDeformer.f_GetDeformerBuf();
    }

    /// <summary>取得當前變形數值</summary>
    public virtual float f_GetDeformerValue(string strEM)
    {
        return _BoneDeformer.f_GetValue(strEM);
    }
}
