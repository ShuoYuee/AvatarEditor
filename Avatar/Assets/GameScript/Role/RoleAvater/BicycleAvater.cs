using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

/// <summary>腳踏車時裝組件</summary>
public class BicycleAvater : BaseAvater
{
    /// <summary>腳踏車物件</summary>
    private GameObject _CurBicycle = null;
    /// <summary>是否顯示腳踏車</summary>
    private bool _bDisplay = false;

    public override void f_Init(BaseRoleControllV2 tBaseRoleControl)
    {
        _BaseRoleControllV2 = tBaseRoleControl;
        Transform Mesh = _BaseRoleControllV2.transform.Find("oModel").Find("Hips");
        //從Hips下尋找腳踏車物件
        GameObject tBicycleMesh = GameTools.f_GetGameObject(Mesh, GameEM.EM_RolePart.bike.ToString());
        MessageBox.DEBUG("初始化自行車_" + GameEM.EM_RolePart.bike.ToString());
        if (tBicycleMesh != null)
        {
            _CurBicycle = tBicycleMesh;
            _bDisplay = true;
            InitHipsPort();
            f_InitBikePos();
        }
    }

    #region 初始化
    protected override void InitHipsPort()
    {
        //若未找到自行車物件則初始化失敗
        if (_CurBicycle == null)
        {
            MessageBox.ASSERT("未找到自行車物件，初始化失敗");
            return;
        }
        //尋找Hips作為起點，設定骨架資料
        Transform oRoleHips = GameTools.f_GetGameObjectParent(_CurBicycle.transform, "oModel").transform.Find("Hips");
        getBone(oRoleHips, _dirHips);

        //設定常駐裝扮部位
        f_AddPart(GameEM.EM_BikePart.bike.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.dummy_wheel_front.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.dummy_wheel_back.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.dummy_brakeL.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.dummy_brakeR.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.mesh_bike_frame.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.mesh_bike_stem.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.mesh_bike_sadle.ToString(), oRoleHips);
        f_AddPart(GameEM.EM_BikePart.dummy_crank.ToString(), oRoleHips);

        _Animator = oRoleHips.GetComponentInChildren<Animator>();
        f_InitBikePos();
        base.InitHipsPort();
    }

    protected override void getBone(Transform bone, Dictionary<string, Transform> skeletonBones)
    {
        if (bone == null)
        {
            return;
        }
        if (!skeletonBones.ContainsKey(bone.name))
        {
            skeletonBones[bone.name] = bone;
        }
        foreach (Transform childBone in bone)
        {
            getBone(childBone, skeletonBones);
        }
    }

    public override bool f_GetInit()
    {//是否初始化完成
        if (!_bInit) { return false; }
        return _bDisplay;
    }

    /// <summary>
    /// 增加換裝部位
    /// </summary>
    /// <param name="strPart">換裝部位名稱</param>
    /// <param name="oRoleHips">全身骨架資料</param>
    private void f_AddPart(string strPart, Transform oRoleHips)
    {
        if (_dirHips.ContainsKey(strPart))
        {
            _dirData.Add(strPart, new BicycleAvaterPart(_BaseRoleControllV2, oRoleHips, _dirHips, _dirHips[strPart]));
        }
    }
    #endregion

    /// <summary>初始化腳踏車位置</summary>
    public void f_InitBikePos()
    {
        if (_CurBicycle == null) { return; }
        //計算腳踏車位置，將角色和腳踏車移動到適合高度(確保腳踏車著地、角色臀部位於指定點上)
        Transform SeatBone = f_GetBone(GameEM.EM_BikeIK.Seat.ToString());
        Transform BikeBone = f_GetBone(GameEM.EM_BikePart.bike.ToString());
        Transform RoleHips = _BaseRoleControllV2.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);

        if (SeatBone == null || BikeBone == null || RoleHips == null) { return; }
        //計算腳踏車底部和坐墊的高度差，推算腳踏車底部的Y軸
        //Vector3 vDistance = SeatBone.position - BikeBone.position;
        BikeBone.position = _BaseRoleControllV2.transform.position;
        //BikeBone.localRotation = Quaternion.Euler(new Vector3(BikeBone.localRotation.x, 0, BikeBone.localRotation.z));
        float DistanceY = SeatBone.position.y - RoleHips.position.y;
        float DistanceZ = SeatBone.position.z - RoleHips.position.z;
        BikeBone.position -= new Vector3(0, DistanceY, DistanceZ / 2);
    }

    #region 換裝
    /// <summary>改變時裝</summary>
    public override void f_Chanage(AvaterPartDT tAvaterPartDT, GameObject oNewObj)
    {
        if (!_bInit)
        {//若腳踏車還未初始化則不進行零件換裝
            if (tAvaterPartDT.iRolePart == (int)GameEM.EM_RolePart.bike)
            {
                Transform Mesh = _BaseRoleControllV2.transform.Find("oModel").Find("Hips");
                Transform tHips = oNewObj.transform.Find("oModel").Find("Hips");
                Transform tMesh = GameTools.f_GetGameObject(tHips, GameEM.EM_RolePart.bike.ToString()).transform;
                tMesh.transform.SetParent(Mesh.Find(GameEM.EM_RolePart.dummy_all.ToString()));

                //_Animator = Mesh.GetComponent<Animator>();
                //if (_Animator == null)
                //{
                //    //_Animator = Mesh.transform.Find(GameEM.EM_RolePart.dummy_all.ToString()).gameObject.AddComponent<Animator>();
                //    _Animator = Mesh.gameObject.AddComponent<Animator>();
                //}
                //_Animator.Update(0);
                f_Init(_BaseRoleControllV2);
                _dirData[GameEM.EM_RolePart.bike.ToString()].f_SetAvatar(tAvaterPartDT.iId);
                glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            }
            else
            {
                glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            }
            return;
        }

        List<string> EMList = f_GetEMList(tAvaterPartDT.iRolePart);
        if (EMList.Count == 0)
        {
            glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            return;
        }
        if(tAvaterPartDT.iId == (int)GameEM.EM_RolePart.bike)
        {
            f_DisPlayAvatar(true);
            Transform tBike = GameTools.f_GetGameObject(oNewObj.transform.Find("oModel").Find("Hips"), GameEM.EM_RolePart.bike.ToString()).transform;
            tBike.SetParent(oNewObj.transform.Find("oModel").Find("Hips"));
        }
        for (int i = 0; i < EMList.Count; i++)
        {//迴圈換裝，將清單內的時裝皆換上
            AvaterPort tAvaterPort = null;
            if (_dirData.TryGetValue(EMList[i], out tAvaterPort))
            {
                //根據類別進行時裝或材質的更換
                if (tAvaterPartDT.iType == (int)GameEM.EM_DressType.Avatar)
                {
                    tAvaterPort.f_Change(tAvaterPartDT, oNewObj);
                }
                else if(tAvaterPartDT.iType == (int)GameEM.EM_DressType.Color)
                {
                    tAvaterPort.f_ChangeMaterial(tAvaterPartDT);
                }
            }
            else
            {
                MessageBox.ASSERT("换装失败，对应的身体部件未找到:" + ((GameEM.EM_RolePart)tAvaterPartDT.iRolePart).ToString());
                glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
            }
        }
        if (_CurBicycle == null)
        {
            _CurBicycle = GameTools.f_GetGameObject(_BaseRoleControllV2.transform, GameEM.EM_BikePart.bike.ToString());
        }
        f_InitBikePos();
    }

    #region 取得散裝EM
    private List<string> f_GetEMList(int iPart)
    {//由於腳踏車是散裝，每一部位由多個Mesh組成，便將特定骨架加入清單做迴圈換裝，避免換材質時重複換到
        List<string> EMList = new List<string>();
        if (iPart == (int)GameEM.EM_RolePart.frame && _CurBicycle != null)
        {//車架
            EMList.Add(GameEM.EM_BikePart.dummy_brakeL.ToString());
            EMList.Add(GameEM.EM_BikePart.dummy_brakeR.ToString());
            EMList.Add(GameEM.EM_BikePart.dummy_crank.ToString());
            EMList.Add(GameEM.EM_BikePart.mesh_bike_stem.ToString());
            EMList.Add(GameEM.EM_BikePart.mesh_bike_sadle.ToString());
            EMList.Add(GameEM.EM_BikePart.mesh_bike_frame.ToString());
        }
        else if (iPart == (int)GameEM.EM_RolePart.wheel && _CurBicycle != null)
        {//車輪
            EMList.Add(GameEM.EM_BikePart.dummy_wheel_front.ToString());
            EMList.Add(GameEM.EM_BikePart.dummy_wheel_back.ToString());
        }
        else if (iPart == (int)GameEM.EM_RolePart.bike)
        {//腳踏車(整體)
            EMList.Add(GameEM.EM_BikePart.bike.ToString());
            f_SetAvatar(0, GameEM.EM_BikePart.dummy_wheel_front.ToString());
            f_SetAvatar(0, GameEM.EM_BikePart.mesh_bike_frame.ToString());
        }
        return EMList;
    }

    private List<string> f_GetEMList(string strPart)
    {//由於腳踏車是散裝，每一部位由多個Mesh組成，便將特定骨架加入清單做迴圈換裝，避免換材質時重複換到
        List<string> EMList = new List<string>();
        if (strPart == GameEM.EM_RolePart.frame.ToString())
        {//車架
            EMList.Add(GameEM.EM_BikePart.dummy_brakeL.ToString());
            EMList.Add(GameEM.EM_BikePart.mesh_bike_stem.ToString());
        }
        else if (strPart == GameEM.EM_RolePart.wheel.ToString())
        {//車輪
            EMList.Add(GameEM.EM_BikePart.dummy_wheel_front.ToString());
            EMList.Add(GameEM.EM_BikePart.dummy_wheel_back.ToString());
        }
        else if (strPart == GameEM.EM_RolePart.bike.ToString())
        {//腳踏車(整體)
            EMList.Add(GameEM.EM_BikePart.bike.ToString());
            f_SetAvatar(0, GameEM.EM_BikePart.dummy_wheel_front.ToString());
            f_SetAvatar(0, GameEM.EM_BikePart.mesh_bike_frame.ToString());
        }
        return EMList;
    }
    #endregion

    public override void f_DisPlayAvatar(bool bSet)
    {
        base.f_DisPlayAvatar(bSet);
        GameTools.f_SetGameObject(_CurBicycle, bSet);
        _bDisplay = bSet;
    }

    public override void f_TakeOff(string strEm)
    {
        if (strEm == GameEM.EM_BikePart.bike.ToString())
        {
            f_DisPlayAvatar(false);
        }
        else
        {
            AvaterPort tAvaterPort = null;
            List<string> tEMList = f_GetEMList(strEm);
            for (int i = 0; i < tEMList.Count; i++)
            {
                if (_dirData.TryGetValue(tEMList[i], out tAvaterPort))
                {
                    tAvaterPort.f_TakeOff();
                }
                else
                {
                    MessageBox.ASSERT("卸裝失敗，對應的身體部位未找到:" + strEm);
                    glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
                }
            }
        }
    }
    #endregion

    public override void f_SetAvatar(int iId, string strEM)
    {
        if (_dirData.ContainsKey(strEM))
        {
            _dirData[strEM].f_SetAvatar(iId);
        }
    }

    /// <summary>取得腳踏車動畫機</summary>
    public override Animator f_GetAnim()
    {
        if (!_bInit) { return null; }
        if (_Animator == null)
        {
            _Animator = _dirHips[GameEM.EM_RolePart.bike.ToString()].parent.gameObject.AddComponent<Animator>();
            MessageBox.DEBUG("自行車增加動畫機");
        }
        return _Animator;
    }

    #region Editor
#if UNITY_EDITOR
    public override void f_Chanage(GameObject oModel, DressEditorData data)
    {
        GameObject oNewObj = GameObject.Instantiate(oModel);
        if (!_bInit)
        {//若腳踏車還未初始化則不進行零件換裝
            if (data._AvatarData._iPart == (int)GameEM.EM_RolePart.bike)
            {
                Transform Mesh = _BaseRoleControllV2.transform.Find("oModel").Find("Hips");
                Transform tHips = oNewObj.transform.Find("oModel").Find("Hips");
                Transform tMesh = GameTools.f_GetGameObject(tHips, GameEM.EM_RolePart.bike.ToString()).transform;
                tMesh.transform.SetParent(Mesh.Find(GameEM.EM_RolePart.dummy_all.ToString()));
                //_Animator = Mesh.GetComponentInChildren<Animator>();
                //if (_Animator == null)
                //{
                //    //_Animator = Mesh.transform.Find(GameEM.EM_RolePart.dummy_all.ToString()).gameObject.AddComponent<Animator>();
                //    _Animator = Mesh.gameObject.AddComponent<Animator>();
                //}
                //_Animator.Update(0);
                f_Init(_BaseRoleControllV2);
                _dirData[GameEM.EM_RolePart.bike.ToString()].f_SetAvatar(data._ID);
                glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            }
            else
            {
                glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            }
            return;
        }

        List<string> EMList = f_GetEMList(data._AvatarData._iPart);
        if (EMList.Count == 0)
        {
            glo_Main.GetInstance().f_DoDestroyObj(oNewObj);
            return;
        }
        else
        {
            if (data._AvatarData._iPart == (int)GameEM.EM_RolePart.bike)
            {
                f_DisPlayAvatar(true);
                Transform tBike = GameTools.f_GetGameObject(oNewObj.transform.Find("oModel").Find("Hips"), GameEM.EM_RolePart.bike.ToString()).transform;
                tBike.SetParent(oNewObj.transform.Find("oModel").Find("Hips"));
            }
            for (int i = 0; i < EMList.Count; i++)
            {//迴圈換裝，將清單內的時裝皆換上
                AvaterPort tAvaterPort = null;
                if (_dirData.TryGetValue(EMList[i], out tAvaterPort))
                {
                    tAvaterPort.f_Change(oNewObj, data);
                }
                else
                {
                    MessageBox.ASSERT("换装失败，对应的身体部件未找到:" + ((GameEM.EM_RolePart)data._AvatarData._iPart).ToString());
                }
            }
        }
        if (_CurBicycle == null)
        {
            _CurBicycle = GameTools.f_GetGameObject(_BaseRoleControllV2.transform, GameEM.EM_BikePart.bike.ToString());
        }
        f_InitBikePos();
    }
#endif
    #endregion

    #region 資料獲取
    /// <summary>取得時裝資料 3Int</summary>
    public override byte[] f_GetAvaterBuf()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_BikePart.bike.ToString()));//1
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_BikePart.dummy_wheel_front.ToString()));//2
        tCreateSocketBuf.f_Add(f_GetAvaterBuf(GameEM.EM_BikePart.mesh_bike_frame.ToString()));//3
        return tCreateSocketBuf.f_GetBuf();
    }

    public override Transform f_GetBone(string szName)
    {
        if (!_CurBicycle.activeSelf)
        {
            return null;
        }
        return base.f_GetBone(szName);
    }

    /// <summary>
    /// 取得指定IK參考點
    /// </summary>
    /// <param name="strEm">IK名稱(對應EM_RolePart)</param>
    /// <returns></returns>
    public Transform f_GetIK(string strEm)
    {
        //從腳踏車模型下的oModel裡的Hips尋找對應IK名稱
        if(_dirHips.TryGetValue(strEm, out Transform IK))
        {
            return IK;
        }
        else
        {
            MessageBox.ASSERT("當前自行車 : " + _CurBicycle.name + "  未設置指定IK : " + strEm);
            return null;
        }
    }

    public Transform f_GetCurBike()
    {
        return _CurBicycle.transform;
    }
#endregion
}
