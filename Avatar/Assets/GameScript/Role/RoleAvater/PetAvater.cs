using System;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>寵物時裝組件</summary>
public class PetAvater : BaseAvater
{
    /// <summary>寵物物件</summary>
    private GameObject _CurPet = null;

    public void f_Init(BaseRoleControllV2 tBaseRoleControl, Transform tModel)
    {
        _BaseRoleControllV2 = tBaseRoleControl;
        Transform oRoleHips = tModel.transform.Find("Hips");

        //從Hips下尋找寵物骨架
        GameObject tPetHips = GameTools.f_GetGameObject(oRoleHips, GameEM.EM_RolePart.pet.ToString());
        if (tPetHips != null)
        {
            _CurPet = tPetHips;
            InitHipsPort();
        }
    }

    #region 初始化
    public override void f_Init(BaseRoleControllV2 tBaseRoleControl)
    {
        _BaseRoleControllV2 = tBaseRoleControl;
        Transform oModel = _BaseRoleControllV2.f_GetRoleModel().Find("oModel");
        Transform oRoleHips = oModel.transform.Find("Hips");

        //從Hips下尋找寵物骨架
        GameObject tPetHips = GameTools.f_GetGameObject(oRoleHips, GameEM.EM_RolePart.pet.ToString());
        if (tPetHips != null)
        {
            _CurPet = tPetHips;
            InitHipsPort();
        }
    }

    protected override void InitHipsPort()
    {
        //若未找到物件則初始化失敗
        if (_CurPet == null)
        {
            MessageBox.ASSERT("未找到寵物物件，初始化失敗");
            return;
        }
        Transform oModel = GameTools.f_GetGameObject(_BaseRoleControllV2.transform, "oModel").transform;
        Transform Mesh = oModel.transform.Find("Mesh");
        Transform oRoleHips = _CurPet.transform;
        getBone(oRoleHips, _dirHips);

        //設定常駐裝扮部位
        f_AddPart(GameEM.EM_RolePart.pet.ToString(), oRoleHips, oModel.Find("Mesh"));

        _Animator = oRoleHips.GetComponent<Animator>();
        base.InitHipsPort();
    }
    #endregion

    public override void f_Chanage(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        GameObject oModel = GameObject.Instantiate(oNewObj);
        Transform tModel = oModel.transform.Find("oModel");
        Transform tHips = tModel.transform.Find("Hips");
        if (!_bInit)
        {
            Transform tMesh = tModel.transform.Find("Mesh");
            Transform tParentHips = _BaseRoleControllV2.transform.Find("oModel").Find("Hips");
            Transform tParentMesh = _BaseRoleControllV2.transform.Find("oModel").Find("Mesh");

            tHips.SetParent(tParentHips);
            tHips.name = GameEM.EM_RolePart.pet.ToString();
            while (tMesh.childCount != 0)
            {
                tMesh.GetChild(0).SetParent(tParentMesh);
            }
            f_SetAnimator(oNewObj, tHips.gameObject);

            glo_Main.GetInstance().f_DoDestroyObj(oModel);
            f_Init(_BaseRoleControllV2);
            return;
        }

        string strEM = ((GameEM.EM_RolePart)tAvaterPartDT.iRolePart).ToString();
        AvaterPort tAvaterPort = null;
        if (_dirData.TryGetValue(strEM, out tAvaterPort))
        {
            tAvaterPort.f_Change(tAvaterPartDT, oNewObj);
        }
        else
        {
            MessageBox.ASSERT("換裝失敗，對應的身體部件未找到：" + strEM);
        }
        f_SetAnimator(oNewObj, tHips.gameObject);
    }

    public override byte[] f_GetAvaterBuf()
    {//Buf 1 Int
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_RolePart.pet.ToString()));
        return tCreateSocketBuf.f_GetBuf();
    }

    public override void f_DisPlayAvatar(bool bSet)
    {
        base.f_DisPlayAvatar(bSet);
        _dirData[GameEM.EM_RolePart.pet.ToString()].f_DisPlayAvatar(bSet);
    }

    public override Animator f_GetAnim()
    {
        if (!_bInit) { return null; }
        if (_Animator == null)
        {
            _Animator = _CurPet.AddComponent<Animator>();
            MessageBox.DEBUG("寵物增加動畫機");
        }
        return _Animator;
    }

    private void f_SetAnimator(GameObject oModel, GameObject tHips)
    {
        Animator tAnimator = oModel.GetComponentInChildren<Animator>();
        if (tAnimator != null)
        {
            if (_Animator == null)
            {
                _Animator = tHips.gameObject.AddComponent<Animator>();
            }
            _Animator.runtimeAnimatorController = tAnimator.runtimeAnimatorController;
            _Animator.avatar = tAnimator.avatar;
        }
    }

    #region Editor
#if UNITY_EDITOR
    public override void f_Chanage(GameObject oModel, DressEditorData data)
    {
        GameObject oNewObj = GameObject.Instantiate(oModel);
        Transform tModel = oNewObj.transform.Find("oModel");
        Transform tHips = tModel.transform.Find("Hips");
        if (!_bInit)
        {
            Transform tMesh = tModel.transform.Find("Mesh");
            Transform tParentHips = _BaseRoleControllV2.transform.Find("oModel").Find("Hips");
            Transform tParentMesh = _BaseRoleControllV2.transform.Find("oModel").Find("Mesh");

            tHips.SetParent(tParentHips);
            tHips.name = GameEM.EM_RolePart.pet.ToString();
            while (tMesh.childCount != 0)
            {
                tMesh.GetChild(0).SetParent(tParentMesh);
            }
            f_SetAnimator(oNewObj, tHips.gameObject);

            glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            f_Init(_BaseRoleControllV2);
            return;
        }

        string strEM = ((GameEM.EM_RolePart)data._AvatarData._iPart).ToString();
        AvaterPort tAvaterPort = null;
        if (_dirData.TryGetValue(strEM, out tAvaterPort))
        {
            tAvaterPort.f_Change(oNewObj, data);
        }
        else
        {
            MessageBox.ASSERT("換裝失敗，對應的身體部件未找到：" + strEM);
        }
        f_SetAnimator(oNewObj, tHips.gameObject);
    }
#endif
    #endregion
}
