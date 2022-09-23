using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;


/// <summary>
/// 變形部位
/// </summary>
public class BoneDeformerItem
{
    /// <summary>變形器</summary>
    private BoneDeformer _BoneDeformer;
    /// <summary>角色控制器</summary>
    private DressRoleControl _BaseRoleControll;
    /// <summary>BlendShape資料(部位名稱,Index清單)</summary>
    private Dictionary<string, List<int>> BoneList = new Dictionary<string, List<int>>();
    /// <summary>當前變形數值</summary>
    private float _fCurValue = 0;

    public BoneDeformerItem(BoneDeformer tBoneDeformer, DressRoleControl tDressRoleControl)
    {
        _BoneDeformer = tBoneDeformer;
        _BaseRoleControll = tDressRoleControl;
    }

    /// <summary>
    /// 增加資料
    /// </summary>
    /// <param name="strEM">部位名稱</param>
    /// <param name="iIndex">Index</param>
    public void f_AddList(string strEM, int iIndex)
    {
        if (!BoneList.ContainsKey(strEM))
        {
            BoneList.Add(strEM, new List<int>());
        }
        if (!BoneList[strEM].Contains(iIndex))
        {
            BoneList[strEM].Add(iIndex);
        }
    }

    /// <summary>
    /// 變形
    /// </summary>
    /// <param name="fValue">變形數值(0~100)</param>
    public void f_Deformer(float fValue)
    {
        _fCurValue = fValue;
        foreach(string key in BoneList.Keys)
        {
            List<int> aList = BoneList[key];
            for(int i = 0; i < aList.Count; i++)
            {
                _BoneDeformer.f_Deformer(key, aList[i], fValue);
            }
        }
    }

    /// <summary>身高變形</summary>
    public void f_SetBodyHeight(string strPart, float fValue)
    {
#if UNITY_EDITOR
        //用更換Avatar的方式實現
        if (fValue < 50)
        {
            _BaseRoleControll.f_BoneDeformer(DressEditorManager.GetInstance().f_GetCurAvatar(0));
        }
        else
        {
            _BaseRoleControll.f_BoneDeformer(DressEditorManager.GetInstance().f_GetCurAvatar(1));
        }
#else
        //用更換Avatar的方式實現
        if (fValue < 50)
        {
            _BaseRoleControll.f_BoneDeformer(_BaseRoleControll.f_RoleBody(0));
        }
        else
        {
            _BaseRoleControll.f_BoneDeformer(_BaseRoleControll.f_RoleBody(1));
        }
#endif
        _fCurValue = fValue;
    }

    /// <summary>取得當前變形數值</summary>
    public float f_GetCurValue()
    {
        return _fCurValue;
    }
}
