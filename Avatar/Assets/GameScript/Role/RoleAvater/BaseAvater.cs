using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>時裝組件</summary>
public class BaseAvater
{
    /// <summary>角色控制器</summary>
    protected BaseRoleControllV2 _BaseRoleControllV2;
    /// <summary>保存部件</summary>
    protected Dictionary<string, AvaterPort> _dirData = new Dictionary<string, AvaterPort>();
    /// <summary>骨架資料庫(依EM_BodyPart做分類)</summary>
    public Dictionary<string, Transform> _dirHips = new Dictionary<string, Transform>();
    /// <summary>Avatar動畫機</summary>
    protected Animator _Animator = null;
    /// <summary>是否初始化</summary>
    protected bool _bInit = false;

    #region 初始化
    /// <summary>
    /// 初始化时装
    /// </summary>
    /// <param name="tBaseRoleControl">角色控制器</param>
    public virtual void f_Init(BaseRoleControllV2 tBaseRoleControl)
    {
        _BaseRoleControllV2 = tBaseRoleControl;
        InitHipsPort();
    }

    /// <summary>初始化骨架部位</summary>
    protected virtual void InitHipsPort()
    {
        MessageBox.DEBUG("骨架初始化完成");
        _bInit = true;
    }

    /// <summary>
    /// 增加換裝部位
    /// </summary>
    /// <param name="strPart">換裝部位名稱</param>
    /// <param name="oRoleHips">全身骨架資料</param>
    /// <param name="tHips">Mesh父物件</param>
    /// <param name="bFindBone">是否為尋找MeshRender物件</param>
    protected virtual void f_AddPart(string strPart, Transform oRoleHips, Transform tHips, bool bFindBone = false)
    {
        //若是尋找MeshRender物件，則從全身骨架資料中去尋找Mesh
        if (bFindBone)
        {
            if (!_dirHips.ContainsKey(strPart))
            {
                _dirData.Add(strPart, new AvaterPort(_BaseRoleControllV2, oRoleHips, _dirHips, null));
                return;
            }
            _dirData.Add(strPart, new AvaterPort(_BaseRoleControllV2, oRoleHips, _dirHips, _dirHips[strPart]));
            return;
        }

        _dirData.Add(strPart, new AvaterPort(_BaseRoleControllV2, oRoleHips, _dirHips, tHips.Find(strPart)));
    }

    /// <summary>是否初始化完成</summary>
    public virtual bool f_GetInit()
    {
        return _bInit;
    }
    #endregion

    /// <summary>是否顯示時裝</summary>
    public virtual void f_DisPlayAvatar(bool bSet)
    {
        if (!_bInit) { return; }
    }

    #region 換裝
    /// <summary>
    /// 換裝
    /// </summary>
    /// <param name="tAvaterPart">時裝資料</param>
    /// <param name="oNewObj">時裝物件</param>
    public virtual void f_Chanage(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        if (!_bInit)
        {
            Transform tModel = oNewObj.transform.Find("oModel");
            Transform tHips = tModel.transform.Find("Hips");
            Transform tMesh = tModel.transform.Find("Mesh");
            Transform tParentHips = _BaseRoleControllV2.transform.Find("oModel").Find("Hips");
            Transform tParentMesh = _BaseRoleControllV2.transform.Find("oModel").Find("Mesh");

            tHips.SetParent(tParentHips);
            tHips.name = GameEM.EM_RolePart.pet.ToString();
            while (tMesh.childCount != 0)
            {
                tMesh.GetChild(0).SetParent(tParentMesh);
            }
            glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
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
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
        }
    }

    /// <summary>脫掉指定部位時裝</summary>
    public virtual void f_TakeOff(string strEm)
    {
        AvaterPort tAvaterPort = null;
        if (_dirData.TryGetValue(strEm, out tAvaterPort))
        {
            tAvaterPort.f_TakeOff();
        }
        else
        {
            MessageBox.ASSERT("卸裝失敗，對應的身體部位未找到:" + strEm);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
        }
    }

    #region 資料獲取
    /// <summary>获得当前时装Buf</summary>
    public virtual byte[] f_GetAvaterBuf()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();

        return tCreateSocketBuf.f_GetBuf();
    }

    protected int f_GetAvaterBuf(string strEM)
    {
        if (_dirData.ContainsKey(strEM))
        {
            return _dirData[strEM].f_GetAvater();
        }
        return 0;
    }
    #endregion
    #endregion

    #region 換色

    /// <summary>
    /// 改變材質球
    /// </summary>
    /// <param name="tAvaterPartDT">材質球資料</param>
    /// <param name="strEm">部位Type</param>
    public void f_ChangeMaterial(AvaterPartDT tAvaterPartDT, string strEm)
    {
        if (_dirData.TryGetValue(strEm, out AvaterPort tAvaterPort))
        {
            tAvaterPort.f_ChangeMaterial(tAvaterPartDT);
        }
        else
        {
            MessageBox.ASSERT("未設置相關部位，無法修改材質 : " + strEm);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
        }
    }

    public virtual void f_ChangeMaterial(string strEm, Material material, Texture texture, Color color)
    {
        if (_dirData.TryGetValue(strEm, out AvaterPort tAvaterPort))
        {
            tAvaterPort.f_ChangeMaterial(material, texture, color);
        }
        else
        {
            MessageBox.ASSERT("未設置相關部位，無法修改材質 : " + strEm);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
        }
    }
    #endregion

    public virtual void f_SetAvatar(int iId, string strEM)
    {
        
    }

    #region 骨架
    /// <summary>
    /// 取得所有骨架
    /// </summary>
    /// <param name="bone">骨架父物件</param>
    /// <param name="skeletonBones">物件資料庫</param>
    protected virtual void getBone(Transform bone, Dictionary<string, Transform> skeletonBones)
    {
        if (bone == null)
        {
            return;
        }

        skeletonBones[bone.name] = bone;
        foreach (Transform childBone in bone)
        {
            getBone(childBone, skeletonBones);
        }
    }

    /// <summary>取得對應名稱的骨架</summary>
    public virtual Transform f_GetBone(string szName)
    {
        if (_dirHips.ContainsKey(szName))
        {
            if (_dirHips[szName] != null)
            {
                return _dirHips[szName];
            }
            else
            {
                MessageBox.ASSERT("對應骨架已遺失 : " + szName);
            }
        }
        else
        {
            MessageBox.ASSERT("未找到對應骨架 : " + szName);
        }
        return null;
    }

    /// <summary>取得骨架資料數量</summary>
    public int f_GetBoneCount()
    {
        return _dirHips.Count;
    }
    #endregion

    public virtual Animator f_GetAnim()
    {
        if (!_bInit) { return null; }
        return _Animator;
    }

    #region Editor
#if UNITY_EDITOR
    public virtual void f_Chanage(GameObject oModel, DressEditorData data)
    {
        GameObject oNewObj = GameObject.Instantiate(oModel);
        if (!_bInit)
        {
            Transform tModel = oNewObj.transform.Find("oModel");
            Transform tHips = tModel.transform.Find("Hips");
            Transform tMesh = tModel.transform.Find("Mesh");
            Transform tParentHips = _BaseRoleControllV2.transform.Find("oModel").Find("Hips");
            Transform tParentMesh = _BaseRoleControllV2.transform.Find("oModel").Find("Mesh");

            tHips.SetParent(tParentHips);
            tHips.name = GameEM.EM_RolePart.pet.ToString();
            while (tMesh.childCount != 0)
            {
                tMesh.GetChild(0).SetParent(tParentMesh);
            }
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
    }
#endif
    #endregion
}
