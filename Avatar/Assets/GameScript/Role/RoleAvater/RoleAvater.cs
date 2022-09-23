using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;

/// <summary>
/// 角色时装管理
/// </summary>
public class RoleAvater : BaseAvater
{
    /// <summary>是否顯示時裝</summary>
    private bool _bDisPlay = true;
    /// <summary>膚色ID</summary>
    private int _iSkin = 0;

    protected override void InitHipsPort()
    {
        Transform oModel = _BaseRoleControllV2.f_GetRoleModel().Find("oModel");
        Transform oRoleHips = oModel.transform.Find("Hips");
        getBone(oRoleHips, _dirHips);
        Transform Mesh = oModel.transform.Find("Mesh");

        //設定常駐裝扮部位
        f_AddPart(GameEM.EM_RolePart.eyes.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.face.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.hair.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.hair_color.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.jersey.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.shorts.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.head.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.shoes.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.socks.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.arm.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.leg.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.finger.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.gloves.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.goggles.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.helmet.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.beard.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.accessories.ToString(), oRoleHips, Mesh);
        f_AddPart(GameEM.EM_RolePart.cosplay.ToString(), oRoleHips, Mesh);
        base.InitHipsPort();
    }

    /// <summary>是否顯示時裝</summary>
    public override void f_DisPlayAvatar(bool bSet)
    {
        base.f_DisPlayAvatar(bSet);
        if (_bDisPlay == bSet) { return; }
        _bDisPlay = bSet;
        _dirData[GameEM.EM_RolePart.eyes.ToString()].f_DisPlayAvatar(bSet);//1
        _dirData[GameEM.EM_RolePart.face.ToString()].f_DisPlayAvatar(bSet);//2
        _dirData[GameEM.EM_RolePart.hair.ToString()].f_DisPlayAvatar(bSet);//3
        _dirData[GameEM.EM_RolePart.hair_color.ToString()].f_DisPlayAvatar(bSet);//4
        _dirData[GameEM.EM_RolePart.jersey.ToString()].f_DisPlayAvatar(bSet);//5
        _dirData[GameEM.EM_RolePart.shorts.ToString()].f_DisPlayAvatar(bSet);//6
        _dirData[GameEM.EM_RolePart.head.ToString()].f_DisPlayAvatar(bSet);//7
        _dirData[GameEM.EM_RolePart.shoes.ToString()].f_DisPlayAvatar(bSet);//8
        _dirData[GameEM.EM_RolePart.socks.ToString()].f_DisPlayAvatar(bSet);//9
        _dirData[GameEM.EM_RolePart.arm.ToString()].f_DisPlayAvatar(bSet);//10
        _dirData[GameEM.EM_RolePart.leg.ToString()].f_DisPlayAvatar(bSet);//11
        _dirData[GameEM.EM_RolePart.finger.ToString()].f_DisPlayAvatar(bSet);//12
        _dirData[GameEM.EM_RolePart.gloves.ToString()].f_DisPlayAvatar(bSet);//13
        _dirData[GameEM.EM_RolePart.goggles.ToString()].f_DisPlayAvatar(bSet);//14
        _dirData[GameEM.EM_RolePart.helmet.ToString()].f_DisPlayAvatar(bSet);//15
        _dirData[GameEM.EM_RolePart.beard.ToString()].f_DisPlayAvatar(bSet);//16
        _dirData[GameEM.EM_RolePart.accessories.ToString()].f_DisPlayAvatar(bSet);//17
    }

    public void f_ChangeSkin(int iSkin)
    {
        _iSkin = iSkin;
    }

    public override void f_Chanage(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        base.f_Chanage(tAvaterPartDT, oNewObj);
        if (tAvaterPartDT.iRolePart == (int)GameEM.EM_RolePart.hair)
        {
            int iId = _dirData[GameEM.EM_RolePart.hair_color.ToString()].f_GetAvater();
            if (iId > 0)
            {
                ((DressRoleControl)_BaseRoleControllV2).f_ChangeAvater((AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(iId));
            }
        }
    }

    public override void f_SetAvatar(int iId, string strEM)
    {
        if (strEM == GameEM.EM_RolePart.skin.ToString() || strEM == GameEM.EM_RolePart.hair_color.ToString())
        {
            _dirData[strEM].f_SetAvatar(iId);
        }
    }

    #region 資料獲取
    /// <summary>
    /// 获得当前时装Buf 18Int
    /// </summary>
    /// <returns></returns>
    public override byte[] f_GetAvaterBuf()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.eyes.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.face.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.hair.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.hair_color.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.jersey.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.shorts.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.head.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.shoes.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.socks.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.arm.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.leg.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.finger.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.gloves.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.goggles.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.helmet.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.beard.ToString()));
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.accessories.ToString()));
        tCreateSocketBuf.f_Add(_iSkin);
        return tCreateSocketBuf.f_GetBuf();
    }

    public byte[] f_GetSpecialAvaterBuf()
    {//特殊造型額外取得
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.cosplay.ToString()));
        return tCreateSocketBuf.f_GetBuf();
    }
    #endregion

    protected override void getBone(Transform bone, Dictionary<string, Transform> skeletonBones)
    {
        if (bone == null)
        {
            return;
        }
        if (bone.name == GameEM.EM_BikePart.bike.ToString()) { return; }
        skeletonBones[bone.name] = bone;
        foreach (Transform childBone in bone)
        {
            getBone(childBone, skeletonBones);
        }
    }

    /// <summary>角色變形</summary>
    public void f_BoneDeformer(string strPart, int iIndex, float fValue)
    {
        if (_dirData.ContainsKey(strPart))
        {
            _dirData[strPart].f_BoneDeformer(iIndex, fValue);
        }
    }

    //測試身體變形
    private string[] strBodyDeform = { "Spine", "Spine1", "Spine2", "Spine3" };
    private string[] strLegDeform = { "LeftUpLeg", "LeftLeg", "LeftFoot", "RightUpLeg", "RightLeg", "RightFoot" };
    private string[] strHandDeform = { "LeftShoulder", "LeftArm", "LeftForeArm", "LeftHand", "RightShoulder", "RightArm", "RightForeArm", "RightHand" };
    public void f_TestBody(float fValue)
    {
        for (int i = 0; i < strLegDeform.Length; i++)
        {
            if (_dirHips.ContainsKey(strLegDeform[i]))
            {
                Transform tTarget = _dirHips[strLegDeform[i]];
                tTarget.localScale = new Vector3(fValue, tTarget.localScale.y, tTarget.localScale.z);
            }
        }
        for (int i = 0; i < strHandDeform.Length; i++)
        {
            if (_dirHips.ContainsKey(strHandDeform[i]))
            {
                Transform tTarget = _dirHips[strHandDeform[i]];
                tTarget.localScale = new Vector3(fValue, tTarget.localScale.y, tTarget.localScale.z);
            }
        }
    }

    public void f_TestBody2(float fValue)
    {
        for (int i = 0; i < strBodyDeform.Length; i++)
        {
            if (_dirHips.ContainsKey(strBodyDeform[i]))
            {
                Transform tTarget = _dirHips[strBodyDeform[i]];
                tTarget.localScale = new Vector3(fValue, tTarget.localScale.y, tTarget.localScale.z);
            }
        }
    }
}