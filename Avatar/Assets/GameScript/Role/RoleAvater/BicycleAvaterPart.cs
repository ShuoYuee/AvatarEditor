using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 腳踏車部位
/// </summary>
public class BicycleAvaterPart : AvaterPort
{
    private string _strPart = "";
    public BicycleAvaterPart(BaseRoleControllV2 tBaseRoleControl, Transform oRoleHips, Dictionary<string, Transform> dirHips, Transform oPart) : base(tBaseRoleControl, oRoleHips, dirHips, oPart)
    {
        if (oPart != null)
        {
            _strPart = oPart.name;
        }
    }

    #region 換裝
    public override void f_ChangeAvatar(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        //因為腳踏車是散裝的，先用此方式換Mesh
        Transform[] tData = oNewObj.transform.Find("oModel").Find("Hips").GetComponentsInChildren<Transform>();
        for (int i = 0; i < tData.Length; i++)
        {
            Transform newBones = tData[i];
            if (!_dirHips.ContainsKey(tData[i].name))
            {
                f_SetNewBone(newBones);
            }
            else if (_dirHips[tData[i].name] == null)
            {
                _dirHips.Remove(tData[i].name);
                f_SetNewBone(newBones);
            }

            newBones = _dirHips[tData[i].name];
            if (newBones != null && tData[i].TryGetComponent(out MeshRenderer mesh))
            {
                //將新裝扮物的資料複製到基礎模型上(網格)
                MeshRenderer NewPartMR = newBones.GetComponentInChildren<MeshRenderer>();
                MeshFilter NewPartMRFilter = newBones.GetComponentInChildren<MeshFilter>();
                NewPartMRFilter.mesh = mesh.GetComponentInChildren<MeshFilter>().mesh;
            }
            if (newBones.name != GameEM.EM_RolePart.bike.ToString() && newBones.name != GameEM.EM_RolePart.dummy_all.ToString())
            {
                //將每一骨架的旋轉和位移資料複製
                newBones.localPosition = tData[i].localPosition;
                newBones.localRotation = tData[i].localRotation;
                newBones.localScale = tData[i].localScale;
            }
        }
        f_DeleteBone(tData, _oPartModel);

        ////更換腳踏車的動畫
        //Animator animator = oNewObj.GetComponentInChildren<Animator>();
        //if (animator != null)
        //{
        //    Animator tAnimator = ((DressRoleControl)_BaseRoleControllV2).f_GetBikeAnimator();
        //    if (tAnimator != null)
        //    {
        //        tAnimator.runtimeAnimatorController = animator.runtimeAnimatorController;
        //        tAnimator.avatar = animator.avatar;
        //    }
        //    else
        //    {
        //        Animator m_animator = _oRoleHips.gameObject.AddComponent<Animator>();
        //        m_animator.runtimeAnimatorController = animator.runtimeAnimatorController;
        //        m_animator.avatar = animator.avatar;
        //    }
        //}

        //更新資料並刪除參考物
        _AvaterPartDT = tAvaterPartDT;
        glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
    }

    /// <summary>
    /// 新增MR物件
    /// </summary>
    /// <param name="NewBone">新骨架(新MR物件))</param>
    private void f_SetNewBone(Transform NewBone)
    {
        Transform tChild = NewBone;
        Transform tParent = NewBone.parent;
        Vector3 vOriginPos = tChild.localPosition;
        Quaternion qOriginPos = tChild.localRotation;
        Vector3 sOriginPos = tChild.localScale;

        while (tParent)
        {//若能找到同名的父物件，將新物件移至該父物件底部
            if (_dirHips.ContainsKey(tParent.name))
            {//重設新物件的座標
                tChild.SetParent(_dirHips[tParent.name]);
                tChild.localPosition = vOriginPos;
                tChild.localRotation = qOriginPos;
                tChild.localScale = sOriginPos;
                _dirHips.Add(tChild.name, tChild);
                break;
            }
            else
            {
                tChild = tParent;
                tParent = tChild.parent;
            }
        }
    }

    /// <summary>
    /// 清除多餘MR物件
    /// </summary>
    /// <param name="Bones">對應用的新時裝骨架</param>
    /// <param name="tRoleHips">骨架起點</param>
    private void f_DeleteBone(Transform[] Bones, Transform tRoleHips)
    {
        if (tRoleHips == null)
        {
            tRoleHips = _dirHips[_strPart];
        }

        //新時裝骨架、骨架起點下的所有子骨架
        List<Transform> aList = new List<Transform>(Bones);
        List<Transform> tList = new List<Transform>(tRoleHips.GetComponentsInChildren<Transform>());

        for (int i = 0; i < tList.Count; i++)
        {//依序確認是否有多餘骨架
            Transform aData = aList.Find(p => p.name == tList[i].name);
            if (!aData)
            {//若為新時裝內不存在的骨架，則刪除該骨架
                Transform tDeleteObj = tList[i];
                _dirHips.Remove(tList[i].name);
                tList.Remove(tDeleteObj);
                glo_Main.GetInstance().f_DoDestroyObj(tDeleteObj.gameObject);
                i--;
            }
        }
    }
    #endregion

    public override void f_TakeOff()
    {
        if (_oPartModel != null)
        {
            _oPartModel.gameObject.SetActive(false);
            if (_AvaterPartDT == null)
            {
                _AvaterPartDT = new AvaterPartDT();
            }
            _AvaterPartDT.iId = 0;
        }
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
    }

    #region 換色
    public override void f_ChangeMaterial(AvaterPartDT tAvaterPartDT)
    {
        //Load材質球
        if (tAvaterPartDT.iResMaterials == 0)
        {//Resource
            Material tMaterial = glo_Main.GetInstance().m_ResourceManager.f_LoadResourceMaterial(tAvaterPartDT.szMaterials);
            _PartRender.sharedMaterial = new Material(tMaterial);
            f_ChangeColorHue(_PartRender, tAvaterPartDT.szHue);

            Texture tTexture = glo_Main.GetInstance().m_ResourceManager.f_LoadResourceTexture(tAvaterPartDT.szTexture);
            _PartRender.sharedMaterial.mainTexture = tTexture;
        }
        else if(tAvaterPartDT.iResMaterials == 1)
        {//AB
            if (!string.IsNullOrEmpty(tAvaterPartDT.szMaterials))
            {
                glo_Main.GetInstance().m_ResourceManager.f_LoadABMaterialSign(tAvaterPartDT.szMaterials, ";", f_ChangeMaterial, tAvaterPartDT.szHue);
            }
            if (!string.IsNullOrEmpty(tAvaterPartDT.szTexture))
            {
                glo_Main.GetInstance().m_ResourceManager.f_LoadABTextureSign(tAvaterPartDT.szTexture, ";", f_ChangeTexture, null);
            }
        }
    }

    public override void f_ChangeMaterial(GameObject go, object obj1, object obj2)
    {
        if (_oPartModel == null)
        {
            _oPartModel = _dirHips[_strPart];
        }
        Material tMaterial = new Material((Material)obj1);
        MeshRenderer[] tData = _oPartModel.GetComponentsInChildren<MeshRenderer>();
        string strHue = (string)obj2;
        int[] iHue = null;
        if (strHue != null && strHue != "")
        {
            iHue = ccU3DEngine.ccMath.f_String2ArrayInt(strHue, "-");
        }

        for (int i = 0; i < tData.Length; i++)
        {
            tData[i].sharedMaterial = tMaterial;
            if (iHue != null)
            {//根據長度做Color不同設定
                if (iHue.Length == 4)
                {
                    Color color = new Color(iHue[0] / 255f, iHue[1] / 255f, iHue[2] / 255f, iHue[3] / 255f);
                    tData[i].sharedMaterial.color = color;
                }
                else if (iHue.Length == 3)
                {
                    Color color = new Color(iHue[0] / 255f, iHue[1] / 255f, iHue[2] / 255f);
                    tData[i].sharedMaterial.color = color;
                }
                MessageBox.DEBUG("更改色票：" + tData[i].name);
            }
            else
            {
                tData[i].sharedMaterial.SetColor("_Color", Color.white);
            }
        }
    }

    public override void f_ChangeTexture(GameObject go, object obj1, object obj2)
    {
        if (_oPartModel == null)
        {
            _oPartModel = _dirHips[_strPart];
        }
        Texture tTexture = (Texture)obj1;
        MeshRenderer[] tData = _oPartModel.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < tData.Length; i++)
        {
            tData[i].sharedMaterial.mainTexture = tTexture;
        }
    }

    public override void f_ChangeMaterial(Material material, Texture texture, Color color)
    {
        f_ChangeMaterial(null, material, null);
        f_ChangeTexture(null, texture, null);

        if (_oPartModel == null)
        {
            _oPartModel = _dirHips[_strPart];
        }
        MeshRenderer[] tData = _oPartModel.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < tData.Length; i++)
        {
            f_ChangeColorHue(tData[i], color);
        }
    }
    #endregion
}
