using System.Collections.Generic;
using ccU3DEngine;


/// <summary>
/// 變形器
/// </summary>
public class BoneDeformer
{
    /// <summary>角色控制器</summary>
    protected DressRoleControl _BaseRoleControll;
    /// <summary>變形資料</summary>
    protected Dictionary<string, BoneDeformerItem> _BoneDictionary = new Dictionary<string, BoneDeformerItem>();

    #region 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="tBaseRoleControll">角色控制器</param>
    public void f_Init(BaseRoleControllV2 tBaseRoleControll)
    {
        _BaseRoleControll = (DressRoleControl)tBaseRoleControll;
        f_InitBoneData();
    }

    /// <summary>初始化變形資料</summary>
    public void f_InitBoneData()
    {
        _BoneDictionary.Add(GameEM.EM_Deformer.BodyHeight.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.BodyHeight.ToString()].f_Deformer(50);

        _BoneDictionary.Add(GameEM.EM_Deformer.Waist.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Waist.ToString()].f_AddList(GameEM.EM_RolePart.jersey.ToString(), 3);

        _BoneDictionary.Add(GameEM.EM_Deformer.Chest.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Chest.ToString()].f_AddList(GameEM.EM_RolePart.jersey.ToString(), 2);

        _BoneDictionary.Add(GameEM.EM_Deformer.Hand.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Hand.ToString()].f_AddList(GameEM.EM_RolePart.jersey.ToString(), 1);
        _BoneDictionary[GameEM.EM_Deformer.Hand.ToString()].f_AddList(GameEM.EM_RolePart.arm.ToString(), 0);

        _BoneDictionary.Add(GameEM.EM_Deformer.LegWidth.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.LegWidth.ToString()].f_AddList(GameEM.EM_RolePart.shorts.ToString(), 1);
        _BoneDictionary[GameEM.EM_Deformer.LegWidth.ToString()].f_AddList(GameEM.EM_RolePart.leg.ToString(), 0);

        _BoneDictionary.Add(GameEM.EM_Deformer.Trapezius.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Trapezius.ToString()].f_AddList(GameEM.EM_RolePart.jersey.ToString(), 0);

        _BoneDictionary.Add(GameEM.EM_Deformer.Buttocks.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Buttocks.ToString()].f_AddList(GameEM.EM_RolePart.shorts.ToString(), 0);

        _BoneDictionary.Add(GameEM.EM_Deformer.Head.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Head.ToString()].f_AddList(GameEM.EM_RolePart.head.ToString(), 0);

        _BoneDictionary.Add(GameEM.EM_Deformer.Spine.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Spine.ToString()].f_Deformer(50);
        _BoneDictionary.Add(GameEM.EM_Deformer.HandHight.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.HandHight.ToString()].f_Deformer(50);
        _BoneDictionary.Add(GameEM.EM_Deformer.LegHight.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.LegHight.ToString()].f_Deformer(50);
        _BoneDictionary.Add(GameEM.EM_Deformer.Shoulder.ToString(), new BoneDeformerItem(this, _BaseRoleControll));
        _BoneDictionary[GameEM.EM_Deformer.Shoulder.ToString()].f_Deformer(50);
    }
    #endregion

    /// <summary>
    /// 變形
    /// </summary>
    /// <param name="strEM">部位名稱</param>
    /// <param name="fValue">變形數值</param>
    public void f_Deformer(string strEM, float fValue)
    {
        //尋找對應變形部位
        if (!_BoneDictionary.ContainsKey(strEM))
        {
            MessageBox.ASSERT("未設置相關變形資料 : " + strEM);
            return;
        }
        _BoneDictionary[strEM].f_Deformer(fValue);
    }

    /// <summary>
    /// 變形
    /// </summary>
    /// <param name="strPart">部位名稱</param>
    /// <param name="iIndex">BlendShape_Index</param>
    /// <param name="fValue">變形數值</param>
    public void f_Deformer(string strPart, int iIndex, float fValue)
    {
        _BaseRoleControll.f_BoneDeformer(strPart, iIndex, fValue);
    }

    #region 資料獲取
    /// <summary>
    /// 取得當前變形數值
    /// </summary>
    /// <param name="strEM">變形部位名稱</param>
    /// <returns></returns>
    public float f_GetValue(string strEM)
    {
        if (!_BoneDictionary.ContainsKey(strEM))
        {
            MessageBox.ASSERT("未設置相關變形資料 : " + strEM);
            return 0;
        }
        return _BoneDictionary[strEM].f_GetCurValue();
    }

    /// <summary>取得變形資料  12Int</summary>
    public byte[] f_GetDeformerBuf()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();

        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.BodyHeight.ToString()));//1
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Waist.ToString()));//2
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Chest.ToString()));//3
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Hand.ToString()));//4
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.LegWidth.ToString()));//5
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Trapezius.ToString()));//6
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Buttocks.ToString()));//7
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Head.ToString()));//8

        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Spine.ToString()));//9
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.LegHight.ToString()));//10
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.Shoulder.ToString()));//11
        tCreateSocketBuf.f_Add((int)f_GetValue(GameEM.EM_Deformer.HandHight.ToString()));//12

        return tCreateSocketBuf.f_GetBuf();
    }
    #endregion
}
