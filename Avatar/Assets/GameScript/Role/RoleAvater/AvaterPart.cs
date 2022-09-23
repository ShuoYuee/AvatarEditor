using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>
/// 時裝部件
/// </summary>
public class AvaterPort
{
    /// <summary>
    /// 角色对象
    /// </summary>
    protected BaseRoleControllV2 _BaseRoleControllV2 = null;
    /// <summary>
    /// 角色骨骼主节点
    /// </summary>
    protected Transform _oRoleHips = null;
    /// <summary>
    /// 保存当前时装的模型 null表示为原始模型
    /// </summary>
    protected Transform _oPartModel = null;
    /// <summary>當前時裝的材質球</summary>
    protected Material _oMaterial = null;
    /// <summary>
    /// 时装被移动的骨骼节点 null表示未发生，
    /// </summary>
    protected Dictionary<string, Transform> _aChangeHips = new Dictionary<string, Transform>();
    //protected Transform _oChangeHips = null;
    /// <summary>時裝Render</summary>
    protected Renderer _PartRender;

    /// <summary>當前時裝資料</summary>
    protected AvaterPartDT _AvaterPartDT;
    /// <summary>是否顯示</summary>
    protected bool _bDisPlayer = true;

    /// <summary>部件骨架字典</summary>
    protected Dictionary<string, Transform> _dirHips;
    /// <summary>變形資料(Index, 變形數值)</summary>
    protected Dictionary<int, float> _dirDeformerData = new Dictionary<int, float>();

    public AvaterPort(BaseRoleControllV2 tBaseRoleControl, Transform oRoleHips, Dictionary<string, Transform> dirHips, Transform oPart)
    {
        _BaseRoleControllV2 = tBaseRoleControl;
        _oRoleHips = oRoleHips;
        _dirHips = dirHips;

        _oPartModel = oPart;
        if (_oPartModel)
        {
            _PartRender = oPart.GetComponent<Renderer>();
        }
    }

    /// <summary>顯示部位時裝</summary>
    public void f_DisPlayAvatar(bool bSet)
    {
        if (_PartRender)
        {
            _PartRender.enabled = bSet;
        }
        _bDisPlayer = bSet;
    }

    #region 換裝

    public virtual void f_Change(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        f_ChangeAvatar(tAvaterPartDT, oNewObj);
        f_ChangeMaterial(tAvaterPartDT);
        f_DisPlayAvatar(_bDisPlayer);

        if (_oPartModel == null)
        {
            _oPartModel = _BaseRoleControllV2.transform.Find("oModel").Find("Mesh").Find(((GameEM.EM_RolePart)tAvaterPartDT.iRolePart).ToString());
        }
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
    }

    #region 角色換裝
    /// <summary>
    /// 换装(SMR)
    /// </summary>
    /// <param name="tAvaterPartDT">時裝資料</param>
    /// <param name="oNewObj">時裝物件</param>
    public virtual void f_ChangeAvatar(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        if (!_BaseRoleControllV2)
        {
            _BaseRoleControllV2 = GameObject.Instantiate(GameMain.GetInstance().m_DressRoleControl.gameObject).GetComponent<DressRoleControl>();
        }
        if (!_BaseRoleControllV2)
        {
            MessageBox.ASSERT("換裝模型還未載入");
            glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            return;
        }

        //开始加载新时装
        oNewObj.transform.parent = _BaseRoleControllV2.transform;
        oNewObj.transform.localPosition = Vector3.zero;
        oNewObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        oNewObj.transform.localScale = Vector3.one;

        //確認Render類型
        SkinnedMeshRenderer _PartSMR = oNewObj.GetComponentInChildren<SkinnedMeshRenderer>();
        MeshRenderer _PartMR = oNewObj.GetComponentInChildren<MeshRenderer>();
        if (_PartMR)
        {
            f_ChangeMesh(tAvaterPartDT, _PartMR, oNewObj);
            return;
        }
        else if (_PartSMR == null)
        {
            MessageBox.ASSERT("未找到Render設定 : " + oNewObj.name);
            return;
        }

        //開始進行SMR蒙皮更換
        if (_oPartModel == null)//若基礎模型未包含該類型裝扮，則將該裝扮物加入基礎模型
        {
            f_AddPart(tAvaterPartDT, oNewObj);
        }
        
        //複製骨架資料
        if (_PartSMR.rootBone != null && _dirHips.ContainsKey(_PartSMR.rootBone.name))
        {
            _PartSMR.rootBone = _dirHips[_PartSMR.rootBone.name];
        }
        
        Transform[] newBones = _PartSMR.bones;
        Transform FstChangeHips = null;
        int newBoneCount = newBones != null ? newBones.Length : 0;
        for (int newBoneIndex = 0; newBoneIndex < newBoneCount; newBoneIndex++)
        {
            Transform newBone = newBones[newBoneIndex];
            if (newBone != null && _dirHips.ContainsKey(newBone.name))
            {
                newBones[newBoneIndex] = _dirHips[newBone.name];
            }
            else if (newBone != null && _aChangeHips.ContainsKey(newBone.name))
            {
                newBones[newBoneIndex] = _aChangeHips[newBone.name];
            }
            else
            {
                if (FstChangeHips == null)
                {
                    FstChangeHips = newBone;
                }
            }

            if (FstChangeHips != null)
            {
                //修改时装的骨骼节点
                ChangeHipsChild(FstChangeHips);
                //_oChangeHips = FstChangeHips;
            }
        }
        //將新裝扮物的資料複製到基礎模型上(蒙皮、骨架、網格、材質)
        SkinnedMeshRenderer newPartSMR = _oPartModel.GetComponentInChildren<SkinnedMeshRenderer>();
        if (newPartSMR == null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oPartModel.gameObject);
            f_AddPart(tAvaterPartDT, oNewObj);
            newPartSMR = _oPartModel.GetComponentInChildren<SkinnedMeshRenderer>();
        }
        newPartSMR.bones = newBones;
        newPartSMR.sharedMesh = _PartSMR.sharedMesh;
        newPartSMR.sharedMaterials = _PartSMR.sharedMaterials;
        _oMaterial = newPartSMR.sharedMaterial;

        //更新資料
        _PartSMR = null;
        _AvaterPartDT = tAvaterPartDT;
        //資料已複製完成，刪除新裝扮物
        glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
    }

    /// <summary>
    /// 換裝(MR配件)
    /// </summary>
    /// <param name="tAvaterPartDT">時裝資料</param>
    private void f_ChangeMesh(AvaterPartDT tAvaterPartDT, MeshRenderer _PartMR, GameObject oNewModel)
    {
        bool bDeleteNewModel = true;
        if (!_oPartModel)
        {
            _oPartModel = oNewModel.transform;
            bDeleteNewModel = false;
            _PartRender = _oPartModel.GetComponentInChildren<MeshRenderer>();
        }

        GameEM.EM_RolePart tEm = (GameEM.EM_RolePart)tAvaterPartDT.iRolePart;
        Transform newBones = null;
        if (_dirHips.ContainsKey(tEm.ToString()))
        {
            newBones = _dirHips[tEm.ToString()];
        }
        else
        {
            //若未存在原始物件，則依照骨架重新抓取位置
            Transform[] child = _oRoleHips.GetComponentsInChildren<Transform>();
            string strBone = _oPartModel.Find("oModel").Find("Hips").GetChild(0).name;
            Transform oPart = null;
            for (int i = 0; i < child.Length; i++)
            {
                if (child[i].name == strBone)
                {
                    oPart = child[i];
                    break;
                }
            }

            if (oPart)
            {
                //將抓到的骨架儲存
                _dirHips.Add(tEm.ToString(), oPart);
                newBones = oPart;
            }
            else
            {
                MessageBox.ASSERT("未找到指定骨架 : " + ((GameEM.EM_RolePart)tAvaterPartDT.iRolePart).ToString());
                glo_Main.GetInstance().f_DoDestroyObj(_oPartModel.gameObject);
                _oPartModel = null;
                return;
            }
        }
        //將新裝扮物的資料複製到基礎模型上(網格、材質)
        MeshRenderer NewPartMR = _oPartModel.GetComponentInChildren<MeshRenderer>();
        MeshFilter NewPartMRFilter = _oPartModel.GetComponentInChildren<MeshFilter>();
        NewPartMR.sharedMaterials = _PartMR.sharedMaterials;
        NewPartMRFilter.mesh = _PartMR.GetComponentInChildren<MeshFilter>().mesh;

        //更新資料和MR物件位置
        _PartMR = null;
        _AvaterPartDT = tAvaterPartDT;
        if (!bDeleteNewModel)
        {
            //將MR配件移動到骨架下當子物件
            Transform oModel = _oPartModel.transform.Find("oModel").Find("Mesh").GetComponentInChildren<MeshRenderer>().transform;
            Vector3 vScale = oModel.localScale;
            oModel.SetParent(newBones);
            oModel.localPosition = Vector3.zero;
            oModel.localRotation = Quaternion.Euler(Vector3.zero);
            oModel.localScale = vScale;
            _oPartModel = oModel;
        }
        glo_Main.GetInstance().f_DoDestroyObj(oNewModel);
    }
    
    protected virtual void f_AddPart(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        _oPartModel = oNewObj.GetComponentInChildren<Renderer>().transform;
        _oPartModel.SetParent(_BaseRoleControllV2.transform.Find("oModel").Find("Mesh"));
        _oPartModel.name = ((GameEM.EM_RolePart)tAvaterPartDT.iRolePart).ToString();
        _PartRender = _oPartModel.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    #endregion

    /// <summary>脫掉當前物件</summary>
    public virtual void f_TakeOff()
    {
        //清除時裝
        if (_oPartModel != null)
        {
            _oPartModel.gameObject.SetActive(false);
            glo_Main.GetInstance().f_DoDestroyObj(_oPartModel.gameObject);
            _oPartModel = null;

            if (_AvaterPartDT == null)
            {
                _AvaterPartDT = new AvaterPartDT();
            }
            _AvaterPartDT.iId = 0;
        }
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
    }

    /// <summary>
    /// 把時裝部件的骨骼节点移动到主骨骼中(SMR處理多餘骨架)
    /// </summary>
    /// <param name="TargetHips">目標物的骨架頂點</param>
    private void ChangeHipsChild(Transform TargetHips)
    {
        string strParentHips = TargetHips.name;
        Transform tParent = TargetHips.parent;
        Transform tRoleParent = null;

        if (tParent == null)
        {
            //重要问题！！！！
            //时装所有节点为空的情况暂时没有资源可以测试，需要找资源时行进行
            //或者禁止美术制作此类型结构的模型骨骼
            tRoleParent = _oRoleHips;
        }
        else if (_dirHips.ContainsKey(tParent.name))
        {
            tRoleParent = _dirHips[tParent.name];
        }
        else if (_aChangeHips.ContainsKey(tParent.name))
        {
            tRoleParent = _aChangeHips[tParent.name];
        }
        else
        {
            MessageBox.ASSERT("主骨骼父节点错误，未找到对应的父节点 " + tParent.name);
        }

        if (tRoleParent == null)
        {
            MessageBox.ASSERT("主骨骼父节点错误，未找到对应的父节点 " + tParent.name);
        }
        while (tParent.childCount > 0)
        {
            if (tParent == tRoleParent) { break; }
            if (tParent.GetChild(0) == tRoleParent) { break; }
            Transform tTransform = tParent.GetChild(0);

            Transform[] tChild = tTransform.GetComponentsInChildren<Transform>();
            for(int i = 0; i < tChild.Length; i++)
            {
                if (!_aChangeHips.ContainsKey(tChild[i].name))
                {
                    _aChangeHips.Add(tChild[i].name, tChild[i]);
                    MessageBox.DEBUG("增加Bone：" + tChild[i].name);
                }
            }
            Vector3 position = tTransform.localPosition;
            Quaternion rotation = tTransform.localRotation;
            Vector3 scale = tTransform.localScale;
            tTransform.parent = tRoleParent.transform;
            tTransform.localPosition = position;
            tTransform.localRotation = rotation;
            tTransform.localScale = scale;
        }
    }
    #endregion

    #region 換色
    /// <summary>
    /// 更換材質球
    /// </summary>
    /// <param name="tAvaterPartDT">材質球資料</param>
    public virtual void f_ChangeMaterial(AvaterPartDT tAvaterPartDT)
    {
        if (!_PartRender) { return; }
        if (tAvaterPartDT.szMaterials == "")
        {
            MessageBox.DEBUG("沒有材質球資料");
            return;
        }
        //Load材質球
        if (tAvaterPartDT.iResMaterials == 0)
        {//Resource
            Material tMaterial = glo_Main.GetInstance().m_ResourceManager.f_LoadResourceMaterial(tAvaterPartDT.szMaterials);
            _PartRender.sharedMaterial = new Material(tMaterial);
            f_ChangeColorHue(_PartRender, tAvaterPartDT.szHue);

            Texture tTexture = glo_Main.GetInstance().m_ResourceManager.f_LoadResourceTexture(tAvaterPartDT.szTexture);
            _PartRender.sharedMaterial.mainTexture = tTexture;
        }
        else if (tAvaterPartDT.iResMaterials == 1)
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

    public virtual void f_ChangeMaterial(GameObject go, object obj1, object obj2)
    {//更改材質
        try
        {
            MessageBox.DEBUG("更改材質球：" + _PartRender.name);
            _PartRender.sharedMaterial = new Material((Material)obj1);
            f_ChangeColorHue(_PartRender, (string)obj2);
        }
        catch
        {
            MessageBox.DEBUG("材質球不得為空：" + _PartRender.name);
        }
    }

    public virtual void f_ChangeTexture(GameObject go, object obj1, object obj2)
    {//更改材質貼圖
        try
        {
            MessageBox.DEBUG("更改材質貼圖：" + _PartRender.name);
            _PartRender.sharedMaterial.mainTexture = (Texture)obj1;
            _PartRender.sharedMaterial.SetTexture("_ColorMask", (Texture)obj1);

        }
        catch
        {
            MessageBox.DEBUG("材質貼圖不得為空：" + _PartRender.name);
        }
    }

    public virtual void f_ChangeMaterial(Material material, Texture texture, Color color)
    {//更改材質
        if (_PartRender == null) { return; }
        f_ChangeMaterial(null, material, null);
        f_ChangeTexture(null, texture, null);

        if (_oPartModel != null)
        {
            f_ChangeColorHue(_oPartModel.GetComponentInChildren<Renderer>(), color);
        }
    }

    protected virtual void f_ChangeColorHue(Renderer tRenderer, string strHue)
    {//更改色票
        if(strHue!=null && strHue != "")
        {
            int[] iHue = ccMath.f_String2ArrayInt(strHue, "-");
            if (iHue.Length == 4)
            {
                Color color = new Color(iHue[0] / 255f, iHue[1] / 255f, iHue[2] / 255f, iHue[3] / 255f);
                tRenderer.sharedMaterial.SetColor("_Color", color);
                tRenderer.sharedMaterial.SetColor("_Color0", color);
            }
            else if (iHue.Length == 3)
            {
                Color color = new Color(iHue[0] / 255f, iHue[1] / 255f, iHue[2] / 255f);
                tRenderer.sharedMaterial.SetColor("_Color", color);
                tRenderer.sharedMaterial.SetColor("_Color0", color);
            }
            MessageBox.DEBUG("更改色票：" + tRenderer.name);
        }
        else
        {
            tRenderer.sharedMaterial.SetColor("_Color", Color.white);
        }
    }

    protected virtual void f_ChangeColorHue(Renderer tRenderer, Color color)
    {//更改色票
        try
        {
            if (color != null)
            {
                tRenderer.sharedMaterial.SetColor("_Color", color);
                tRenderer.sharedMaterial.SetColor("_Color0", color);
            }
            else
            {
                tRenderer.sharedMaterial.SetColor("_Color", Color.white);
                tRenderer.sharedMaterial.SetColor("_Color0", Color.white);
            }
        }
        catch
        {
            MessageBox.ASSERT("未找到相關Render");
        }
    }
    #endregion

    public void f_SetAvatar(int iId)
    {
        if (_AvaterPartDT == null) { _AvaterPartDT = new AvaterPartDT(); }
        _AvaterPartDT.iId = iId;
    }

    #region 資料獲取
    /// <summary>獲取當前裝備的ID</summary>
    public int f_GetAvater()
    {
        if (_AvaterPartDT == null)
        {
            return 0;
        }
        return _AvaterPartDT.iId;
    }
    #endregion

    #region 變形
    /// <summary>
    /// 變形
    /// </summary>
    /// <param name="iIndex">Index</param>
    /// <param name="fValue">變形數值</param>
    public void f_BoneDeformer(int iIndex, float fValue)
    {
        if (!_PartRender) { return; }
        if(_PartRender.TryGetComponent(out SkinnedMeshRenderer tSkinnedMesh))
        {
            try
            {
                tSkinnedMesh.SetBlendShapeWeight(iIndex, fValue);
                if (!_dirDeformerData.ContainsKey(iIndex))
                {
                    _dirDeformerData.Add(iIndex, 0);
                }
                _dirDeformerData[iIndex] = fValue;
            }
            catch
            {
                MessageBox.ASSERT("該模型沒有設置相關變形數據 : " + _oPartModel.name + "_" + iIndex);
            }
        }
    }

    /// <summary>初始化變形器</summary>
    private void f_InitDeformer()
    {
        foreach(int iIndex in _dirDeformerData.Keys)
        {
            f_BoneDeformer(iIndex, _dirDeformerData[iIndex]);
        }
    }
    #endregion

    #region Editor
#if UNITY_EDITOR
    public void f_Change(GameObject oModel, DressEditorData data)
    {
        AvaterPartDT tAvaterPartDT = new AvaterPartDT();
        tAvaterPartDT.iId = data._ID;
        tAvaterPartDT.szName = data._AvatarName;
        tAvaterPartDT.iGender = data._AvatarData._iGender;
        tAvaterPartDT.iRolePart = data._AvatarData._iPart;

        f_ChangeAvatar(tAvaterPartDT, oModel);
        f_DisPlayAvatar(_bDisPlayer);
        f_ChangeMaterial(data._AvatarData._AvatarMaterial, data._AvatarData._AvatarTexture, data._AvatarData._Color);
    }
#endif
    #endregion

}
