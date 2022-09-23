using ccU3DEngine;
using System;
using UnityEngine;

/// <summary>
/// 換裝角色控制器
/// </summary>
public class DressRoleControl : BaseRoleControllV2
{
    private Animator _Animator = null;

    /// <summary>角色時裝管理器</summary>
    protected RoleAvater _RoleAvater = new RoleAvater();
    /// <summary>腳踏車時裝管理器</summary>
    protected BicycleAvater _BicycleAvater = new BicycleAvater();
    /// <summary>變形器</summary>
    protected BoneDeformer _BoneDeformer = new BoneDeformer();
    /// <summary>IK功能是否啟用</summary>
    protected bool _bFootIK = true, _bHandIK = true;

    protected Vector3 _vOriginRole = Vector3.zero;
    public Vector3 _vOriginRoleGet { get { return _vOriginRole; } }
    protected int _iGender = (int)GameEM.EM_Gender.Male;
    protected int _iRace = (int)GameEM.EM_Race.Asian;

    protected DressRoleActionController _DressRoleActionController;
    protected PetControl _PetControl;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="iId">角色ID(自取)</param>
    /// <param name="tCharacterDT">角色資料</param>
    public virtual void f_Init(int iId, BaseActionController tBaseActionController, Transform InitPos)
    {
        MessageBox.DEBUG("角色初始化：" + name);
        _BaseActionController = tBaseActionController;
        _DressRoleActionController = (DressRoleActionController)_BaseActionController;
        if (InitPos != null)
        {
            transform.SetParent(InitPos);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }
        _vOriginRole = transform.position;
        _Animator = GetComponent<Animator>();

        _RoleAvater.f_Init(this);
        _BoneDeformer.f_Init(this);
        _BicycleAvater.f_Init(this);

        InitAI();
        InitMessage();
        _bInitOK = true;
    }

    #region 初始化
    /// <summary>
    /// 初始化換裝模型的設定
    /// </summary>
    /// <param name="iGender">性別</param>
    /// <param name="iRace">人種</param>
    public void f_InitUserData(int iGender, int iRace)
    {
        _iGender = iGender;
        _iRace = iRace;
    }

    public void f_InitUserData(int iGender)
    {
        _iGender = iGender;
    }

    public void f_InitUserDataRace(int iRace)
    {
        _iRace = iRace;
    }
    #endregion

    #region RoleAvater
    #region 加載
    public void f_LoadAvatarBufAll(byte[] aAvaterBuf)
    {
        ReadBuf tReadBuf = new ReadBuf();
        tReadBuf.f_Save(aAvaterBuf, aAvaterBuf.Length);
        int iEyes = tReadBuf.f_ReadInt();
        int iFace = tReadBuf.f_ReadInt();
        int iHair = tReadBuf.f_ReadInt();
        int iHairColor = tReadBuf.f_ReadInt();
        int iTops = tReadBuf.f_ReadInt();
        int iBottoms = tReadBuf.f_ReadInt();
        int iHead = tReadBuf.f_ReadInt();
        int iShose = tReadBuf.f_ReadInt();
        int iSock = tReadBuf.f_ReadInt();
        int iArm = tReadBuf.f_ReadInt();
        int iLeg = tReadBuf.f_ReadInt();
        int iFinger = tReadBuf.f_ReadInt();
        int iGloves = tReadBuf.f_ReadInt();
        int iGlasses = tReadBuf.f_ReadInt();
        int iHelmet = tReadBuf.f_ReadInt();
        int iBeard = tReadBuf.f_ReadInt();
        int iAccessories = tReadBuf.f_ReadInt();
        int iSkin = tReadBuf.f_ReadInt();
        int iPet = tReadBuf.f_ReadInt();
        //載入自行車
        int iBicycle = tReadBuf.f_ReadInt();
        int iFrontWheel = tReadBuf.f_ReadInt();
        int iFrontRoad = tReadBuf.f_ReadInt();
        f_LoadAvater(iBicycle, GameEM.EM_RolePart.bike.ToString());
        f_LoadAvater(iFrontWheel, GameEM.EM_RolePart.frame.ToString());
        f_LoadAvater(iFrontRoad, GameEM.EM_RolePart.wheel.ToString());
        //載入時裝
        f_LoadAvater(iEyes, GameEM.EM_RolePart.eyes.ToString());
        f_LoadAvater(iFace, GameEM.EM_RolePart.face.ToString());
        f_LoadAvater(iHair, GameEM.EM_RolePart.hair.ToString());
        f_LoadAvater(iHairColor, GameEM.EM_RolePart.hair_color.ToString());
        f_LoadAvater(iTops, GameEM.EM_RolePart.jersey.ToString());
        f_LoadAvater(iBottoms, GameEM.EM_RolePart.shorts.ToString());
        f_LoadAvater(iHead, GameEM.EM_RolePart.head.ToString());
        f_LoadAvater(iShose, GameEM.EM_RolePart.shoes.ToString());
        f_LoadAvater(iSock, GameEM.EM_RolePart.socks.ToString());
        f_LoadAvater(iArm, GameEM.EM_RolePart.arm.ToString());
        f_LoadAvater(iLeg, GameEM.EM_RolePart.leg.ToString());
        f_LoadAvater(iFinger, GameEM.EM_RolePart.finger.ToString());
        f_LoadAvater(iGloves, GameEM.EM_RolePart.gloves.ToString());
        f_LoadAvater(iGlasses, GameEM.EM_RolePart.goggles.ToString());
        f_LoadAvater(iHelmet, GameEM.EM_RolePart.helmet.ToString());
        f_LoadAvater(iBeard, GameEM.EM_RolePart.beard.ToString());
        f_LoadAvater(iAccessories, GameEM.EM_RolePart.accessories.ToString());
        //載入膚色、寵物
        f_RoleSkin(iSkin);
        f_LoadAvater(iPet, GameEM.EM_RolePart.pet.ToString());
        //載入變形
        int iBodyHeight = tReadBuf.f_ReadInt();//身高
        int iWaist = tReadBuf.f_ReadInt();//腰圍
        int iChest = tReadBuf.f_ReadInt();//胸部
        int iHand = tReadBuf.f_ReadInt();//手臂
        int iLegWidth = tReadBuf.f_ReadInt();//腿
        int iTrapezius = tReadBuf.f_ReadInt();//斜方肌
        int iButtocks = tReadBuf.f_ReadInt();//臀部
        int iHeadNeck = tReadBuf.f_ReadInt();//頭頸
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.BodyHeight.ToString(), iBodyHeight);
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.Waist.ToString(), iWaist);
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.Chest.ToString(), iChest);
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.Hand.ToString(), iHand);
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.LegWidth.ToString(), iLegWidth);
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.Trapezius.ToString(), iTrapezius);
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.Buttocks.ToString(), iButtocks);
        _BoneDeformer.f_Deformer(GameEM.EM_Deformer.Head.ToString(), iHeadNeck);
    }

    /// <summary>加載時裝</summary>
    public void f_LoadAvatarBuf(byte[] aAvaterBuf)
    {//載入一般時裝
        ReadBuf tReadBuf = new ReadBuf();
        tReadBuf.f_Save(aAvaterBuf, aAvaterBuf.Length);
        //載入時裝
        int iEyes = tReadBuf.f_ReadInt();
        f_LoadAvater(iEyes, GameEM.EM_RolePart.eyes.ToString());
        int iFace = tReadBuf.f_ReadInt();
        f_LoadAvater(iFace, GameEM.EM_RolePart.face.ToString());
        int iHair = tReadBuf.f_ReadInt();
        f_LoadAvater(iHair, GameEM.EM_RolePart.hair.ToString());
        int iHairColor = tReadBuf.f_ReadInt();
        f_LoadAvater(iHairColor, GameEM.EM_RolePart.hair_color.ToString());
        int iTops = tReadBuf.f_ReadInt();
        f_LoadAvater(iTops, GameEM.EM_RolePart.jersey.ToString());
        int iBottoms = tReadBuf.f_ReadInt();
        f_LoadAvater(iBottoms, GameEM.EM_RolePart.shorts.ToString());
        int iHead = tReadBuf.f_ReadInt();
        f_LoadAvater(iHead, GameEM.EM_RolePart.head.ToString());
        int iShose = tReadBuf.f_ReadInt();
        f_LoadAvater(iShose, GameEM.EM_RolePart.shoes.ToString());
        int iSock = tReadBuf.f_ReadInt();
        f_LoadAvater(iSock, GameEM.EM_RolePart.socks.ToString());
        int iArm = tReadBuf.f_ReadInt();
        f_LoadAvater(iArm, GameEM.EM_RolePart.arm.ToString());
        int iLeg = tReadBuf.f_ReadInt();
        f_LoadAvater(iLeg, GameEM.EM_RolePart.leg.ToString());
        int iFinger = tReadBuf.f_ReadInt();
        f_LoadAvater(iFinger, GameEM.EM_RolePart.finger.ToString());
        int iGloves = tReadBuf.f_ReadInt();
        f_LoadAvater(iGloves, GameEM.EM_RolePart.gloves.ToString());
        int iGlasses = tReadBuf.f_ReadInt();
        f_LoadAvater(iGlasses, GameEM.EM_RolePart.goggles.ToString());
        int iHelmet = tReadBuf.f_ReadInt();
        f_LoadAvater(iHelmet, GameEM.EM_RolePart.helmet.ToString());
        int iBeard = tReadBuf.f_ReadInt();
        f_LoadAvater(iBeard, GameEM.EM_RolePart.beard.ToString());
        int iAccessories = tReadBuf.f_ReadInt();
        f_LoadAvater(iAccessories, GameEM.EM_RolePart.accessories.ToString());
        //載入膚色、寵物
        int iSkin = tReadBuf.f_ReadInt();
        f_RoleSkin(iSkin);
        int iPet = tReadBuf.f_ReadInt();
        f_LoadAvater(iPet, GameEM.EM_RolePart.pet.ToString());
        //載入自行車
        int iBicycle = tReadBuf.f_ReadInt();
        f_LoadAvater(iBicycle, GameEM.EM_RolePart.bike.ToString());
        int iFrontWheel = tReadBuf.f_ReadInt();
        f_LoadAvater(iFrontWheel, GameEM.EM_RolePart.wheel.ToString());
        int iFrontRoad = tReadBuf.f_ReadInt();
        f_LoadAvater(iFrontRoad, GameEM.EM_RolePart.frame.ToString());

        //載入變形
        int iBodyHeight = tReadBuf.f_ReadInt();//身高
        f_Deformer(GameEM.EM_Deformer.BodyHeight.ToString(), iBodyHeight);
        int iWaist = tReadBuf.f_ReadInt();//腰圍
        f_Deformer(GameEM.EM_Deformer.Waist.ToString(), iWaist);
        int iChest = tReadBuf.f_ReadInt();//胸部
        f_Deformer(GameEM.EM_Deformer.Chest.ToString(), iChest);
        int iHand = tReadBuf.f_ReadInt();//手臂
        f_Deformer(GameEM.EM_Deformer.Hand.ToString(), iHand);
        int iLegWidth = tReadBuf.f_ReadInt();//腿
        f_Deformer(GameEM.EM_Deformer.LegWidth.ToString(), iLegWidth);
        int iTrapezius = tReadBuf.f_ReadInt();//斜方肌
        f_Deformer(GameEM.EM_Deformer.Trapezius.ToString(), iTrapezius);
        int iButtocks = tReadBuf.f_ReadInt();//臀部
        f_Deformer(GameEM.EM_Deformer.Buttocks.ToString(), iButtocks);
        int iHeadNeck = tReadBuf.f_ReadInt();//頭頸
        f_Deformer(GameEM.EM_Deformer.Head.ToString(), iHeadNeck);
        int iSpine = tReadBuf.f_ReadInt();//身長
        f_Deformer(GameEM.EM_Deformer.Spine.ToString(), iSpine);
        int iLegHight = tReadBuf.f_ReadInt();//腿長
        f_Deformer(GameEM.EM_Deformer.LegHight.ToString(), iLegHight);
        int iShoulder = tReadBuf.f_ReadInt();//肩寬
        f_Deformer(GameEM.EM_Deformer.Shoulder.ToString(), iShoulder);
        int iHandHight = tReadBuf.f_ReadInt();//手臂長
        f_Deformer(GameEM.EM_Deformer.HandHight.ToString(), iHandHight);
        f_InitBikePos();
        f_InitPos(vOriginPos);
    }

    /// <summary>加載特殊造型</summary>
    public void f_LoadSpecialBuf(byte[] aAvaterBuf, byte[] aSpecialAvaterBuf)
    {//載入特殊造型
        ReadBuf tReadBuf = new ReadBuf();
        tReadBuf.f_Save(aAvaterBuf, aAvaterBuf.Length);
        int iEyes = tReadBuf.f_ReadInt();
        int iFace = tReadBuf.f_ReadInt();
        int iHair = tReadBuf.f_ReadInt();
        int iHairColor = tReadBuf.f_ReadInt();
        int iTops = tReadBuf.f_ReadInt();
        int iBottoms = tReadBuf.f_ReadInt();
        int iHead = tReadBuf.f_ReadInt();
        int iShose = tReadBuf.f_ReadInt();
        int iSock = tReadBuf.f_ReadInt();
        int iArm = tReadBuf.f_ReadInt();
        int iLeg = tReadBuf.f_ReadInt();
        int iFinger = tReadBuf.f_ReadInt();
        int iGloves = tReadBuf.f_ReadInt();
        int iGlasses = tReadBuf.f_ReadInt();
        int iHelmet = tReadBuf.f_ReadInt();
        int iBeard = tReadBuf.f_ReadInt();
        int iAccessories = tReadBuf.f_ReadInt();
        //載入膚色、寵物
        int iSkin = tReadBuf.f_ReadInt();
        f_RoleSkin(iSkin);
        int iPet = tReadBuf.f_ReadInt();
        f_LoadAvater(iPet, GameEM.EM_RolePart.pet.ToString());
        //載入自行車
        int iBicycle = tReadBuf.f_ReadInt();
        f_LoadAvater(iBicycle, GameEM.EM_RolePart.bike.ToString());
        int iFrontWheel = tReadBuf.f_ReadInt();
        f_LoadAvater(iFrontWheel, GameEM.EM_RolePart.wheel.ToString());
        int iFrontRoad = tReadBuf.f_ReadInt();
        f_LoadAvater(iFrontRoad, GameEM.EM_RolePart.frame.ToString());
        //載入特殊造型
        ReadBuf aReadBuf = new ReadBuf();
        aReadBuf.f_Save(aSpecialAvaterBuf, aSpecialAvaterBuf.Length);
        int iMode = aReadBuf.f_ReadInt();
        f_LoadAvater(iMode, GameEM.EM_RolePart.cosplay.ToString());

        f_InitBikePos();
        f_InitPos(vOriginPos);
    }

    /// <summary>載入時裝資料</summary>
    private void f_LoadAvater(int iId, string strEm)
    {
        if (iId > 0)
        {
            AvaterPartDT tData = (AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(iId);
            f_ChangeAvater(tData);
        }
        else if (iId <= 0)
        {
            if (strEm == GameEM.EM_RolePart.pet.ToString())
            {
                f_DelPet();
            }
            else if(strEm == GameEM.EM_RolePart.goggles.ToString() || strEm == GameEM.EM_RolePart.helmet.ToString() || strEm == GameEM.EM_RolePart.hair.ToString() || strEm == GameEM.EM_RolePart.accessories.ToString())
            {
                f_TakeOff(strEm);
            }
        }
    }

    #endregion

    #region 儲存與取得資料
    /// <summary>保存角色的时装信息</summary>
    public void f_SaveAvater()
    {
        byte[] aBuf = f_CreateBuf();
        //向伺服器呼叫儲存時裝的請求
        SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
        tSocketCallbackDT.m_ccCallbackSuc = CallBack_UpdateAvaterSuc;
        tSocketCallbackDT.m_ccCallbackFail = CallBack_UpdateAvaterFail;
        Data_Pool.GetInstance().m_PlayerData.f_UpdateAvaterBuf(aBuf, tSocketCallbackDT);
        Data_Pool.GetInstance().m_PlayerData.m_aAvaterBuf = aBuf;

        if (PlayerDataTools.f_CheckUseSpecial())
        {
            //特殊造型資料
            byte[] aSpecialBuf = _RoleAvater.f_GetSpecialAvaterBuf();

            //向伺服器呼叫儲存特殊造型的請求
            byte[] tBuf = new byte[128];
            Array.Copy(aSpecialBuf, tBuf, aSpecialBuf.Length);
            Data_Pool.GetInstance().m_PlayerData.f_UpdateSpecialAvaterBuf(tBuf, tSocketCallbackDT);
            Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf = tBuf;
        }
    }

    public void f_SaveAvater(SocketCallbackDT tSocketCallbackDT)
    {
        byte[] aBuf = f_CreateBuf();
        //向伺服器呼叫儲存時裝的請求
        Data_Pool.GetInstance().m_PlayerData.f_UpdateAvaterBuf(aBuf, tSocketCallbackDT);
        Data_Pool.GetInstance().m_PlayerData.m_aAvaterBuf = aBuf;

        if (PlayerDataTools.f_CheckUseSpecial())
        {
            //特殊造型資料
            byte[] aSpecialBuf = _RoleAvater.f_GetSpecialAvaterBuf();

            //向伺服器呼叫儲存特殊造型的請求
            byte[] tBuf = new byte[128];
            Array.Copy(aSpecialBuf, tBuf, aSpecialBuf.Length);
            Data_Pool.GetInstance().m_PlayerData.f_UpdateSpecialAvaterBuf(tBuf, tSocketCallbackDT);
            Data_Pool.GetInstance().m_PlayerData.m_aSpecialAvaterBuf = tBuf;
        }
    }

    public byte[] f_CreateBuf()
    {
        //角色時裝資料
        byte[] aAvaterBuf = _RoleAvater.f_GetAvaterBuf();
        //寵物資料
        byte[] aPetBuf = f_GetPetBuf();
        //腳踏車時裝資料
        byte[] aBikeAvaterBuf = _BicycleAvater.f_GetAvaterBuf();
        //變形資料
        byte[] aDeformerBuf = f_GetDeformerBuf();

        if ((aAvaterBuf.Length + aPetBuf.Length + aBikeAvaterBuf.Length + aDeformerBuf.Length) > 140)
        {
            MessageBox.ASSERT("时装信息超过预设长度140");
        }
        //18(角色) + 1(寵物) + 3(自行車) + 12(變形) = 34 Int
        byte[] aBuf = new byte[140];
        Array.Copy(aAvaterBuf, aBuf, aAvaterBuf.Length);
        Array.Copy(aPetBuf, 0, aBuf, 18 * 4, aPetBuf.Length);
        Array.Copy(aBikeAvaterBuf, 0, aBuf, 18 * 4 + 1 * 4, aBikeAvaterBuf.Length);
        Array.Copy(aDeformerBuf, 0, aBuf, 18 * 4 + 1 * 4 + 3 * 4, aDeformerBuf.Length);
        aBuf[139] = 27;
        return aBuf;
    }

    public byte[] f_CreateSpecialBuf()
    {
        return _RoleAvater.f_GetSpecialAvaterBuf();
    }

    /// <summary>
    /// 服务器返回成功，跳出成功提示
    /// </summary>
    /// <param name="data"></param>
    void CallBack_UpdateAvaterSuc(object data)
    {//儲存時裝成功
        MessageBox.DEBUG("UpdateAvaterSuc");
    }

    /// <summary>
    /// 服务器返回失败
    /// </summary>
    /// <param name="data"></param>
    void CallBack_UpdateAvaterFail(object data)
    {//儲存時裝失敗
        MessageBox.DEBUG("UpdateAvaterFail");
    }

    #endregion
    #endregion

    #region 換裝
    /// <summary>
    /// 换装
    /// </summary>
    /// <param name="tAvaterPartDT">時裝資料</param>
    public void f_ChangeAvater(AvaterPartDT tAvaterPartDT)
    {
        if (tAvaterPartDT == null)
        {
            MessageBox.ASSERT("換裝失敗，時裝資料為空");
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
            return;
        }
        if (tAvaterPartDT.iType == (int)GameEM.EM_DressType.Avatar || tAvaterPartDT.iType == (int)GameEM.EM_DressType.Pet)
        {//根據類別進行時裝或材質的更換
            if (tAvaterPartDT.iResType == 0)
            {
                GameObject Obj = glo_Main.GetInstance().m_ResourceManager.f_CreateResource(tAvaterPartDT.szResName);
                CallBack_SetSmrMesh(Obj, tAvaterPartDT, null);
            }
            else
            {
                glo_Main.GetInstance().m_ResourceManager.f_CreateForAB(tAvaterPartDT.szBundleName, tAvaterPartDT.szResName, CallBack_SetSmrMesh, tAvaterPartDT, null);
            }
        }
        else if(tAvaterPartDT.iType == (int)GameEM.EM_DressType.Color)
        {
            f_ChangeMaterial(tAvaterPartDT, ((GameEM.EM_RolePart)tAvaterPartDT.iRolePart).ToString());
        }
    }

    #region 卸下時裝
    /// <summary>卸裝</summary>
    public void f_TakeOff(string strEm)
    {
        _RoleAvater.f_TakeOff(strEm);
    }

    /// <summary>卸裝</summary>
    public void f_TakeOff(int iPart, string strData)
    {
        if (strData != null && strData != "")
        {
            string[] strArray = ccMath.f_String2ArrayString(strData, "-");
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] == "0")
                {
                    f_TakeOff(iPart);
                }
                else
                {
                    AvaterPartDT tAvaterPartDT = (AvaterPartDT)glo_Main.GetInstance().m_SC_Pool.m_AvaterPartSC.f_GetSC(ccMath.atoi(strArray[i]));
                    f_ChangeAvater(tAvaterPartDT);
                }
            }
        }
        else
        {
            f_TakeOff(iPart);
        }
    }

    private void f_TakeOff(int iPart)
    {
        if (iPart > (int)GameEM.EM_RolePart.RoleEnd)
        {
            _BicycleAvater.f_TakeOff(((GameEM.EM_RolePart)iPart).ToString());
        }
        else if (iPart == (int)GameEM.EM_RolePart.pet)
        {
            f_DelPet();
        }
        else if (iPart == (int)GameEM.EM_RolePart.cosplay)
        {
            _RoleAvater.f_DisPlayAvatar(true);
            _RoleAvater.f_TakeOff(((GameEM.EM_RolePart)iPart).ToString());
        }
        else
        {
            _RoleAvater.f_TakeOff(((GameEM.EM_RolePart)iPart).ToString());
        }
    }
    #endregion

    /// <summary>
    /// 換裝設定
    /// </summary>
    /// <param name="oModel">裝扮物</param>
    /// <param name="obj1">物件模板ID</param>
    public void CallBack_SetSmrMesh(GameObject oModel, object obj1, object obj2)
    {//有特殊換裝需求就從這裡追加if條件
        AvaterPartDT tAvaterPartDT = (AvaterPartDT)obj1;

        if (tAvaterPartDT.iRolePart >= (int)GameEM.EM_RolePart.RoleEnd)
        {//腳踏車換裝
            _BicycleAvater.f_Chanage(tAvaterPartDT, oModel);
        }
        else if (tAvaterPartDT.iRolePart == (int)GameEM.EM_RolePart.pet)
        {//寵物
            f_ChangePet(tAvaterPartDT.iId, oModel);
        }
        else if (tAvaterPartDT.iRolePart == (int)GameEM.EM_RolePart.cosplay)
        {//特殊造型換裝
            _RoleAvater.f_DisPlayAvatar(false);
            _RoleAvater.f_Chanage(tAvaterPartDT, oModel);
        }
        else
        {//角色換裝
            _RoleAvater.f_Chanage(tAvaterPartDT, oModel);
        }
    }
    #endregion

    #region 換色
    /// <summary>
    /// 更換時裝材質球
    /// </summary>
    /// <param name="tAvaterPartDT">材質球資料</param>
    /// <param name="strEm">部位名稱</param>
    public void f_ChangeMaterial(AvaterPartDT tAvaterPartDT, string strEm)
    {
        if (tAvaterPartDT.iRolePart >= (int)GameEM.EM_RolePart.RoleEnd)
        {
            //腳踏車換色
            if (!_BicycleAvater.f_GetInit())
            {
                _BicycleAvater.f_Init(this);
            }
            _BicycleAvater.f_ChangeMaterial(tAvaterPartDT, strEm);
        }
        else
        {//角色換色
            //若有特殊項目就在這增加if
            if (tAvaterPartDT.iRolePart == (int)GameEM.EM_RolePart.hair_color)
            {//髮色，同時更換頭髮和鬍子材質
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, GameEM.EM_RolePart.hair.ToString());
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, GameEM.EM_RolePart.beard.ToString());
                _RoleAvater.f_SetAvatar(tAvaterPartDT.iId, GameEM.EM_RolePart.hair_color.ToString());
                glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
            }
            else if(tAvaterPartDT.iRolePart == (int)GameEM.EM_RolePart.skin)
            {//膚色，同時更換手臂、腿、頭部、手指材質
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, GameEM.EM_RolePart.head.ToString());
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, GameEM.EM_RolePart.arm.ToString());
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, GameEM.EM_RolePart.leg.ToString());
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, GameEM.EM_RolePart.finger.ToString());
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, GameEM.EM_RolePart.face.ToString());
                _RoleAvater.f_SetAvatar(tAvaterPartDT.iId, GameEM.EM_RolePart.skin.ToString());
                glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
            }
            else
            {
                _RoleAvater.f_ChangeMaterial(tAvaterPartDT, strEm);
            }
        }
    }

    /// <summary>
    /// 更換時裝材質球
    /// </summary>
    /// <param name="material">材質球</param>
    /// /// <param name="texture">材質貼圖</param>
    /// <param name="strEm">部位名稱</param>
    public void f_ChangeMaterial(Material material, Texture texture, Color color, string strEm)
    {
        if(strEm.Equals(GameEM.EM_RolePart.hair_color.ToString()))
        {
            _RoleAvater.f_ChangeMaterial(GameEM.EM_RolePart.hair.ToString(), material, texture, color);
            _RoleAvater.f_ChangeMaterial(GameEM.EM_RolePart.beard.ToString(), material, texture, color);
        }
        else if (strEm.Equals(GameEM.EM_RolePart.skin.ToString()))
        {
            _RoleAvater.f_ChangeMaterial(GameEM.EM_RolePart.head.ToString(), material, texture, color);
            _RoleAvater.f_ChangeMaterial(GameEM.EM_RolePart.arm.ToString(), material, texture, color);
            _RoleAvater.f_ChangeMaterial(GameEM.EM_RolePart.leg.ToString(), material, texture, color);
            _RoleAvater.f_ChangeMaterial(GameEM.EM_RolePart.finger.ToString(), material, texture, color);
            _RoleAvater.f_ChangeMaterial(GameEM.EM_RolePart.face.ToString(), material, texture, color);
        }
        else
        {
            _RoleAvater.f_ChangeMaterial(strEm, material, texture, color);
        }
    }
    #endregion

    #region 模型相關
    public void f_RoleSkin(int iIndex)
    {//角色膚色
        RoleAvaterDT RoleData = GameMain.GetInstance().f_GetAvaterDT(_iRace, _iGender);
        if (RoleData != null)
        {
            f_ChangeMaterial(RoleData.f_GetSkinMaterial(iIndex), RoleData.f_GetSkinTexture(iIndex), RoleData.f_GetSkinColor(iIndex), GameEM.EM_RolePart.skin.ToString());
            _RoleAvater.f_ChangeSkin(iIndex);
        }
        else
        {
            MessageBox.ASSERT("未找到對應模型：" + iIndex);
        }
    }

    public Avatar f_RoleBody(int iIndex)
    {
        return f_RoleBody(iIndex, 1);
    }

    public Avatar f_RoleBody(int iIndex, int iMode)
    {//取得角色身高(Avatar)
        RoleAvaterDT RoleData = GameMain.GetInstance().f_GetAvaterDT(_iRace, _iGender);
        if (RoleData != null)
        {
            return RoleData.f_GetAvatar(iIndex, iMode);
        }
        else
        {
            MessageBox.ASSERT("未找到對應模型");
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
            return null;
        }
    }

    private Vector3 vOriginPos = Vector3.zero;
    /// <summary>
    /// 開關腳踏車
    /// </summary>
    /// <param name="bSet">是否顯示</param>
    public void f_SetDisplayBike(bool bSet)
    {
        vOriginPos = _BicycleAvater.f_GetCurBike().position;
        _BicycleAvater.f_DisPlayAvatar(bSet);
        f_InitPos(vOriginPos);
    }
    #endregion

    #region 自行車位置
    /// <summary>取得自行車底部</summary>
    public Transform f_GetBikePos()
    {
        return _BicycleAvater.f_GetBone(GameEM.EM_RolePart.bike.ToString());
    }

    /// <summary>
    /// 重設角色位置(自行車踏地)
    /// </summary>
    /// <param name="tTargetPos">目標位置</param>
    public virtual void f_InitPos(Vector3 tTargetPos)
    {
        if (f_GetBikePos() != null)
        {
            float fDistance = transform.position.y - f_GetBikePos().position.y;
            transform.position = tTargetPos + new Vector3(0, fDistance, 0);
        }
        else
        {
            transform.position = tTargetPos;
        }
        _vOriginRole = tTargetPos;
    }

    /// <summary>
    /// 重設角色位置(自行車踏地)
    /// </summary>
    /// <param name="tTargetPos">目標位置</param>
    /// <param name="tTargetRot">目標旋轉量</param>
    public void f_InitPos(Vector3 tTargetPos, Quaternion tTargetRot)
    {
        f_InitPos(tTargetPos);
        transform.rotation = tTargetRot;
    }

    public void f_InitPos()
    {
        f_InitPos(_vOriginRole);
    }

    public void f_InitBikePos()
    {
        _BicycleAvater.f_InitBikePos();
    }

    public Transform f_GetCurBike()
    {
        return _BicycleAvater.f_GetCurBike();
    }
    #endregion

    #region IK
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

    /// <summary>
    /// 設定IK
    /// </summary>
    /// <param name="bSet">是否啟用IK</param>
    /// <param name="iIK">1 腳  2 手</param>
    public virtual void f_SetIK(bool bSet, int iIK)
    {
        if (iIK == 1)
        {
            _bFootIK = bSet;
        }
        else if (iIK == 2)
        {
            _bHandIK = bSet;
        }
    }
    #endregion

    #region 變形
    /// <summary>
    /// 變形
    /// </summary>
    /// <param name="strEM">變形部位名稱</param>
    /// <param name="fValue">變形數值</param>
    public virtual void f_Deformer(string strEM, float fValue)
    {
        _BoneDeformer.f_Deformer(strEM, fValue);
    }

    /// <summary>
    /// 變形
    /// </summary>
    /// <param name="strPart">部位名稱</param>
    /// <param name="iIndex">Index</param>
    /// <param name="fValue">變形數值</param>
    public void f_BoneDeformer(string strPart, int iIndex, float fValue)
    {
        _RoleAvater.f_BoneDeformer(strPart, iIndex, fValue);
    }

    /// <summary>
    /// 更換Avatar
    /// </summary>
    /// <param name="tAvatar">更換物</param>
    public void f_BoneDeformer(Avatar tAvatar)
    {
        if (tAvatar == null)
        {
            MessageBox.ASSERT("目標Avatar為空");
            return;
        }
        if (_Animator != null)
        {
            _Animator.avatar = tAvatar;
        }
        else
        {
            MessageBox.ASSERT("角色組件上未找到Animator");
        }
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
    #endregion

    #region 動畫

    /// <summary>
    /// 切換角色指定動畫
    /// </summary>
    /// <param name="strAnim">動畫名</param>
    public virtual void f_SetPlayerAnimClip(string strAnim)
    {
        _DressRoleActionController.f_SetPlayerAnimClip(strAnim);
    }

    /// <summary>
    /// 切換腳踏車指定動畫
    /// </summary>
    /// <param name="strAnim">動畫名</param>
    public virtual void f_SetBikeAnimClip(string strAnim)
    {
        if (strAnim == "") { return; }
        //GameTools.f_PlayAnimator(_BicycleAvater.f_GetAnim(), strAnim);
        _DressRoleActionController.f_SetPlayerAnimClip(strAnim, 1);
    }

    /// <summary>
    /// 調節動畫速度
    /// </summary>
    /// <param name="fSpeed">速度</param>
    public virtual void f_SetBikeSpeed(float fSpeed)
    {//等確認傳輸單位後再做計算調整
        if (!_bInitOK) { return; }
        //Animator tAnimator = _BicycleAvater.f_GetAnim();
        //if (tAnimator != null)
        //{
        //    tAnimator.speed = fSpeed;
        //}
        //else
        //{
        //    MessageBox.ASSERT("未找到腳踏車的動畫機");
        //}
        _DressRoleActionController.f_SetPlayerAnimSpeed(fSpeed, "BikeSpeed");
    }

    //public Animator f_GetBikeAnimator()
    //{
    //    return _BicycleAvater.f_GetAnim();
    //}
    #endregion

    #region 寵物相關
    protected void f_ChangePet(int iId, GameObject oModel)
    {
        f_DelPet();
        _PetControl = (PetControl)RoleTools.f_CreateRoleControll(iId, oModel);
        _PetControl.f_SetTarget(f_GetPetTarget());
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
    }

    protected void f_DelPet()
    {
        if (_PetControl != null)
        {
            _PetControl.f_Destory();
            _PetControl = null;
        }
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(GameMessageDef.PlayerDressEnd);
    }

    protected Transform f_GetPetTarget()
    {
        GameObject tTarget = GameTools.f_GetGameObject(transform, GameEM.EM_RolePart.pet.ToString());
        if (tTarget != null) { return tTarget.transform; }
        return null;
    }

    protected byte[] f_GetPetBuf()
    {
        if (_PetControl != null)
        {
            return _PetControl.f_GetAvaterBuf();
        }
        return new byte[0];
    }

    public void f_SetPetAnimClip(string strAnim)
    {
        if (_PetControl == null)
        {
            MessageBox.DEBUG("角色未裝備寵物");
            return;
        }
        _PetControl.f_SetPetAnimClip(strAnim);
    }
    #endregion

    #region Editor
#if UNITY_EDITOR
    #region 加載(讀取編輯器數據)
    /// <summary>加載時裝</summary>
    public void f_LoadAvatarBuf_Editor(byte[] aAvaterBuf)
    {//載入一般時裝
        ReadBuf tReadBuf = new ReadBuf();
        tReadBuf.f_Save(aAvaterBuf, aAvaterBuf.Length);
        //載入時裝
        int iEyes = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iEyes, GameEM.EM_RolePart.eyes.ToString());
        int iFace = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iFace, GameEM.EM_RolePart.face.ToString());
        int iHair = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iHair, GameEM.EM_RolePart.hair.ToString());
        int iHairColor = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iHairColor, GameEM.EM_RolePart.hair_color.ToString());
        int iTops = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iTops, GameEM.EM_RolePart.jersey.ToString());
        int iBottoms = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iBottoms, GameEM.EM_RolePart.shorts.ToString());
        int iHead = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iHead, GameEM.EM_RolePart.head.ToString());
        int iShose = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iShose, GameEM.EM_RolePart.shoes.ToString());
        int iSock = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iSock, GameEM.EM_RolePart.socks.ToString());
        int iArm = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iArm, GameEM.EM_RolePart.arm.ToString());
        int iLeg = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iLeg, GameEM.EM_RolePart.leg.ToString());
        int iFinger = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iFinger, GameEM.EM_RolePart.finger.ToString());
        int iGloves = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iGloves, GameEM.EM_RolePart.gloves.ToString());
        int iGlasses = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iGlasses, GameEM.EM_RolePart.goggles.ToString());
        int iHelmet = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iHelmet, GameEM.EM_RolePart.helmet.ToString());
        int iBeard = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iBeard, GameEM.EM_RolePart.beard.ToString());
        int iAccessories = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iAccessories, GameEM.EM_RolePart.accessories.ToString());
        //載入膚色、寵物
        int iSkin = tReadBuf.f_ReadInt();
        f_RoleSkin(iSkin);
        int iPet = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iPet, GameEM.EM_RolePart.pet.ToString());
        //載入自行車
        int iBicycle = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iBicycle, GameEM.EM_RolePart.bike.ToString());
        int iFrontWheel = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iFrontWheel, GameEM.EM_RolePart.wheel.ToString());
        int iFrontRoad = tReadBuf.f_ReadInt();
        f_LoadAvater_Editor(iFrontRoad, GameEM.EM_RolePart.frame.ToString());
        //載入變形
        int iBodyHeight = tReadBuf.f_ReadInt();//身高
        f_Deformer(GameEM.EM_Deformer.BodyHeight.ToString(), iBodyHeight);
        int iWaist = tReadBuf.f_ReadInt();//腰圍
        f_Deformer(GameEM.EM_Deformer.Waist.ToString(), iWaist);
        int iChest = tReadBuf.f_ReadInt();//胸部
        f_Deformer(GameEM.EM_Deformer.Chest.ToString(), iChest);
        int iHand = tReadBuf.f_ReadInt();//手臂
        f_Deformer(GameEM.EM_Deformer.Hand.ToString(), iHand);
        int iLegWidth = tReadBuf.f_ReadInt();//腿
        f_Deformer(GameEM.EM_Deformer.LegWidth.ToString(), iLegWidth);
        int iTrapezius = tReadBuf.f_ReadInt();//斜方肌
        f_Deformer(GameEM.EM_Deformer.Trapezius.ToString(), iTrapezius);
        int iButtocks = tReadBuf.f_ReadInt();//臀部
        f_Deformer(GameEM.EM_Deformer.Buttocks.ToString(), iButtocks);
        int iHeadNeck = tReadBuf.f_ReadInt();//頭頸
        f_Deformer(GameEM.EM_Deformer.Head.ToString(), iHeadNeck);
        int iSpine = tReadBuf.f_ReadInt();//身長
        f_Deformer(GameEM.EM_Deformer.Spine.ToString(), iSpine);
        int iLegHight = tReadBuf.f_ReadInt();//腿長
        f_Deformer(GameEM.EM_Deformer.LegHight.ToString(), iLegHight);
        int iShoulder = tReadBuf.f_ReadInt();//肩寬
        f_Deformer(GameEM.EM_Deformer.Shoulder.ToString(), iShoulder);
        int iHandHight = tReadBuf.f_ReadInt();//手臂長
        f_Deformer(GameEM.EM_Deformer.HandHight.ToString(), iHandHight);
        f_InitBikePos();
    }

    private void f_LoadAvater_Editor(int iId, string strEm)
    {
        if (iId > 0)
        {
            DressEditorData tData = DressEditorManager.GetInstance()._AvatarData.Find(delegate (DressEditorData data)
              {
                  if (data._ID == iId) { return true; }
                  return false;
              });
            if (tData != null && tData._EditorAction.Equals(DressEditorData.EM_EditorAction.Avatar))
            {
                CallBack_SetSmrMesh(tData);
            }
        }
        else if (iId <= 0)
        {
            if (strEm == GameEM.EM_RolePart.pet.ToString())
            {
                f_DelPet();
            }
            else if (strEm == GameEM.EM_RolePart.goggles.ToString() || strEm == GameEM.EM_RolePart.helmet.ToString() || strEm == GameEM.EM_RolePart.hair.ToString() || strEm == GameEM.EM_RolePart.accessories.ToString() || strEm == GameEM.EM_RolePart.beard.ToString() || strEm == GameEM.EM_RolePart.cosplay.ToString())
            {
                f_TakeOff(strEm);
            }
        }
    }
    #endregion

    #region 換裝相關
    public void CallBack_SetSmrMesh(DressEditorData tDressEditorData)
    {//編輯器換裝功能
        if (tDressEditorData._AvatarData._iPart >= (int)GameEM.EM_RolePart.RoleEnd)
        {
            GameObject oObj = DressEditorPoolDT._iMesh.Equals(1) ? tDressEditorData._AvatarData._AvatarLowObj : tDressEditorData._AvatarData._AvatarHightObj;
            _BicycleAvater.f_Chanage(oObj, tDressEditorData);
        }
        else if(tDressEditorData._AvatarData._iPart == (int)GameEM.EM_RolePart.pet)
        {
            f_ChangePet(tDressEditorData);
        }
        else if (tDressEditorData._AvatarData._iPart == (int)GameEM.EM_RolePart.cosplay)
        {
            _RoleAvater.f_DisPlayAvatar(false);
            GameObject oObj = DressEditorPoolDT._iMesh.Equals(1) ? tDressEditorData._AvatarData._AvatarLowObj : tDressEditorData._AvatarData._AvatarHightObj;
            _RoleAvater.f_Chanage(oObj, tDressEditorData);
        }
        else if (tDressEditorData._AvatarData._iPart == (int)GameEM.EM_RolePart.hair_color)
        {
            f_ChangeMaterial(tDressEditorData._AvatarData._AvatarMaterial, tDressEditorData._AvatarData._AvatarTexture, tDressEditorData._AvatarData._Color, GameEM.EM_RolePart.hair_color.ToString());
            _RoleAvater.f_SetAvatar(tDressEditorData._ID, GameEM.EM_RolePart.hair_color.ToString());
        }
        else
        {
            GameObject oObj = DressEditorPoolDT._iMesh.Equals(1) ? tDressEditorData._AvatarData._AvatarLowObj : tDressEditorData._AvatarData._AvatarHightObj;
            _RoleAvater.f_Chanage(oObj, tDressEditorData);
        }
    }

    public void f_TakeOff(DressEditorData tDressEditorData)
    {//編輯器卸裝功能
        if (tDressEditorData._AvatarData._iPart > (int)GameEM.EM_RolePart.RoleEnd)
        {
            _BicycleAvater.f_TakeOff(((GameEM.EM_RolePart)tDressEditorData._AvatarData._iPart).ToString());
        }
        else if (tDressEditorData._AvatarData._iPart == (int)GameEM.EM_RolePart.pet)
        {
            f_DelPet();
        }
        else if (tDressEditorData._AvatarData._iPart == (int)GameEM.EM_RolePart.cosplay)
        {
            _RoleAvater.f_DisPlayAvatar(true);
            _RoleAvater.f_TakeOff(((GameEM.EM_RolePart)tDressEditorData._AvatarData._iPart).ToString());
        }
        else
        {
            _RoleAvater.f_TakeOff(((GameEM.EM_RolePart)tDressEditorData._AvatarData._iPart).ToString());
        }
    }

    public void CallBack_SetColor(DressEditorData tDressEditorData, int iIndex)
    {//編輯器換色功能
        try
        {
            f_ChangeMaterial(tDressEditorData._Race._RoleAvatar._SkinMaterial[iIndex], tDressEditorData._Race._RoleAvatar._Texture[iIndex], tDressEditorData._Race._RoleAvatar._Color[iIndex], GameEM.EM_RolePart.skin.ToString());
            _RoleAvater.f_ChangeSkin(iIndex);
        }
        catch
        {
            MessageBox.ASSERT("材質設定未完整：" + tDressEditorData._AvatarName);
        }
    }
    #endregion

    /// <summary>高低模切換</summary>
    public void f_ChangeMesh()
    {
        f_LoadAvatarBuf_Editor(f_CreateBuf());
        //載入特殊造型
        ReadBuf aReadBuf = new ReadBuf();
        byte[] aSpecialAvaterBuf = _RoleAvater.f_GetSpecialAvaterBuf();
        aReadBuf.f_Save(aSpecialAvaterBuf, aSpecialAvaterBuf.Length);
        int iMode = aReadBuf.f_ReadInt();
        if (iMode > 0)
        {
            f_LoadAvater_Editor(iMode, GameEM.EM_RolePart.cosplay.ToString());
        }
    }

    #region 寵物相關(編輯器)
    private void f_ChangePet(DressEditorData tDressEditorData)
    {
        f_DelPet();
        f_CreatePet(tDressEditorData);
    }

    private void f_CreatePet(DressEditorData tDressEditorData)
    {
        GameObject oModel = GameObject.Instantiate(DressEditorPoolDT._iMesh.Equals(1) ? tDressEditorData._AvatarData._AvatarLowObj : tDressEditorData._AvatarData._AvatarHightObj);
        _PetControl = (PetControl)RoleTools.f_CreateRoleControll_Editor(tDressEditorData._ID, (int)tDressEditorData._AvatarData._Part, oModel);
        _PetControl.f_SetTarget(f_GetPetTarget());
        oModel.name = tDressEditorData._AvatarName;
    }
    #endregion
#endif
    #endregion

}
