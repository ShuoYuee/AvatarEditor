using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SR;
using ccU3DEngine;

namespace GameLogic
{
    public class UI_Editor : ccUILogicBase
    {
        #region 菜單資訊
        /// <summary>二層菜單</summary>
        public ScrollDlg_Multilayer _ScrollDlg;
        /// <summary>變形菜單</summary>
        public GridPageView _DeformerPage;
        /// <summary>性別菜單</summary>
        public GridPageView _GenderPage;
        /// <summary>人種菜單</summary>
        public GridPageView _RacePage;
        /// <summary>模型菜單</summary>
        public GridPageView _MeshPage;
        /// <summary>膚色菜單</summary>
        public GridPageView _SkinPage;
        /// <summary>時裝資訊</summary>
        private DressEditorData _SelEditorDT;
        #endregion

        #region UI物件
        /// <summary>菜單物件</summary>
        private GameObject _oMenu;
        /// <summary>離開按鈕</summary>
        private GameObject _oExitBtn;
        /// <summary>離開鏡頭預覽按鈕</summary>
        private GameObject _oLenExitBtn;
        /// <summary>控制菜單顯示按鈕</summary>
        private ccButton _MenuCtrlBtn;
        /// <summary>速度調節</summary>
        private InputField _SpeedInput;
        /// <summary>背景圖</summary>
        private Image _BG;
        #endregion

        private RawImage _PlayerRaw;
        private DressRoleControlV3 _RoleControl;
        private SceneInfor_3D _BGSceneInfor;
        private GameObject _CameraPos;
        private SR_Camera _SRCamera;

        protected override void On_Init()
        {
            MessageBox.DEBUG("啟用遊戲包中的UI_Editor腳本");
#if UNITY_EDITOR
            _oExitBtn = f_GetObject("BtnExit");
            _oLenExitBtn = f_GetObject("BtnLenExit");
            _oMenu = f_GetObject("Menu");
            _oLenExitBtn.SetActive(false);
            _MenuCtrlBtn = f_GetObject("BtnChangeUI").GetComponentInChildren<ccButton>();
            _SpeedInput = f_GetObject("SpeedInput").GetComponentInChildren<InputField>();
            _BGSceneInfor = f_GetObject("BGScene").GetComponent<SceneInfor_3D>();
            _PlayerRaw = f_GetObject("PlayerRaw").GetComponent<RawImage>();
            _BG = f_GetObject("BG").GetComponent<Image>();

            _GenderPage = f_GetObject("GenderPage").GetComponentInChildren<GridPageView>();
            _RacePage = f_GetObject("RacePage").GetComponentInChildren<GridPageView>();
            _MeshPage = f_GetObject("MeshPage").GetComponentInChildren<GridPageView>();
            _SkinPage = f_GetObject("SkinPage").GetComponentInChildren<GridPageView>();

            f_RegClickEvent(_oLenExitBtn, CallBack_OnClickLenExitBtn);
            f_RegClickEvent(_oExitBtn, OnClick_BtnExit);
            f_RegClickEvent(_MenuCtrlBtn.gameObject, CallBack_MenuCtrlBtn);
            f_RegClickEvent(f_GetObject("BtnSave"), CallBack_BtnSave);

            _ScrollDlg = GameUITools.f_CreateScrollDlgMultilayer(f_GetObject("ScrollMenu"));
            _ScrollDlg.f_AddPagesItem(1, f_GetObject("scroll_2").GetComponent<GridPageView>());
            _DeformerPage = f_GetObject("DeformerMenu").GetComponentInChildren<GridPageView>();
            _ScrollDlg.f_AddPagesItem(2, _DeformerPage);
#endif
        }

        private void f_InitMessage()
        {
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_MenuBtn, CallBack_OnClickMenuBtn);
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DeformerBtn, CallBack_BodyHightBtn);
            _SpeedInput.onValueChanged.AddListener(f_SetBikeSpeed);
        }

        protected override void On_Open(object e)
        {
#if UNITY_EDITOR
            f_InitRoleControl();
            f_InitPageView();
            f_InitScrollDlg();
            f_OpenBGCamera();
            f_InitMessage();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMainUI, false);
#endif
        }

        protected override void On_Close()
        {
            f_UnMessage();
            f_CloseBGCamera();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMainUI, true);
        }

        private void f_UnMessage()
        {
            glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_MenuBtn, CallBack_OnClickMenuBtn);
            glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_DeformerBtn, CallBack_BodyHightBtn);
            _SpeedInput.onValueChanged.RemoveListener(f_SetBikeSpeed);
        }

        protected override void On_Destory()
        {
        }

        protected override void On_UpdateGUI()
        {

        }

        private void f_InitScrollDlg()
        {//處理ScrollDlg
#if UNITY_EDITOR
            _ScrollDlg.f_Reset();

            //設定分層
            _ScrollDlg.f_AddScrollItem(RoleTools.f_CreateKeyId(), "髮型", "1", GameEM.EM_RolePart.hair.ToString(), (int)GameEM.EM_RolePart.HairMenu, (int)GameEM.EM_RolePart.hair, true);
            _ScrollDlg.f_AddScrollItem(RoleTools.f_CreateKeyId(), "鬍子", "1", GameEM.EM_RolePart.beard.ToString(), (int)GameEM.EM_RolePart.HairMenu, (int)GameEM.EM_RolePart.beard, true);
            _ScrollDlg.f_AddScrollItem(RoleTools.f_CreateKeyId(), "髮色", "1", GameEM.EM_RolePart.hair_color.ToString(), (int)GameEM.EM_RolePart.HairMenu, (int)GameEM.EM_RolePart.hair_color, true);
            _ScrollDlg.f_AddScrollItem(RoleTools.f_CreateKeyId(), "限量款車種", "1", GameEM.EM_RolePart.bike.ToString(), (int)GameEM.EM_RolePart.BikeMenu, (int)GameEM.EM_RolePart.bike, true);
            _ScrollDlg.f_AddScrollItem(RoleTools.f_CreateKeyId(), "車輪", "1", GameEM.EM_RolePart.wheel.ToString(), (int)GameEM.EM_RolePart.BikeMenu, (int)GameEM.EM_RolePart.wheel, true);
            _ScrollDlg.f_AddScrollItem(RoleTools.f_CreateKeyId(), "車架", "1", GameEM.EM_RolePart.frame.ToString(), (int)GameEM.EM_RolePart.BikeMenu, (int)GameEM.EM_RolePart.frame, true);

            //設定菜單
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.head, "頭型");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.HairMenu, "毛髮");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.jersey, "衣服");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.shorts, "褲子");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.shoes, "鞋子");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.socks, "襪子");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.gloves, "手套");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.helmet, "頭盔");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.goggles, "風鏡");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.BikeMenu, "車款");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.accessories, "配件");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.cosplay, "特殊造型");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.pet, "寵物");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.Deformer, "變形");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.animation, "動作");
            _ScrollDlg.f_AddTitle((int)GameEM.EM_RolePart.Len, "鏡頭");

            f_AddData(DressEditorManager.GetInstance()._AvatarData);

            _ScrollDlg.f_Create(1, CallBack_OnClick, 168, 25, 8, 1, 32, 32, false, false);
            _ScrollDlg.f_CreatePage(2, 1, null, 370, 35, 8, 1, 32, 32, false, false);//重設變形菜單的Item長寬
            _DeformerPage.gameObject.SetActive(false);//關閉變形菜單
            _ScrollDlg.f_SetScroll(false);
            _ScrollDlg.f_SetPage(1, false);
            _MenuCtrlBtn.f_UnSelect();
#endif
        }

        #region 資料整理
        /// <summary>將資料加入對應清單中</summary>
        private void f_AddData(List<DressEditorData> aData)
        {//增加時裝項目
#if UNITY_EDITOR
            for (int i = 0; i < aData.Count; i++)
            {
                if (!aData[i]._EditorAction.Equals(DressEditorData.EM_EditorAction.Avatar))
                {
                    GameEM.EM_RolePart tEM = GameEM.EM_RolePart.Default;
                    if (aData[i]._EditorAction.Equals(DressEditorData.EM_EditorAction.Action))
                    {
                        tEM = GameEM.EM_RolePart.animation;
                    }
                    else if (aData[i]._EditorAction.Equals(DressEditorData.EM_EditorAction.Lens))
                    {
                        tEM = GameEM.EM_RolePart.Len;
                    }
                    _ScrollDlg.f_AddScrollItem(aData[i]._ID, aData[i]._AvatarName, "", "", (int)tEM, aData[i], false);
                    continue;
                }
                else if (aData[i]._AvatarData._iPart < (int)GameEM.EM_RolePart.RoleEnd)
                {//時裝性別確認
                    if (!aData[i]._AvatarData._iGender.Equals(DressEditorPoolDT._iGender) && !aData[i]._AvatarData._iGender.Equals((int)GameEM.EM_Gender.Common))
                    {
                        continue;
                    }
                }
                else if (aData[i]._AvatarData._iPart > (int)GameEM.EM_RolePart.RoleEnd)
                {//時裝尺寸確認
                    if (!PlayerDataTools.f_CheckBodyHight((int)aData[i]._AvatarData._Size, _RoleControl))
                    {
                        continue;
                    }
                }
                if ((aData[i]._AvatarData._AvatarLowObj == null || aData[i]._AvatarData._AvatarHightObj == null) && aData[i]._AvatarData._iPart != (int)GameEM.EM_RolePart.hair_color)
                {
                    MessageBox.ASSERT("模型資料為空 : " + aData[i]._ID);
                    continue;
                }

                switch (aData[i]._AvatarData._iPart)
                {
                    case (int)GameEM.EM_RolePart.hair:
                        _ScrollDlg.f_AddPagesScrollItem(1, aData[i]._ID, aData[i]._AvatarName, "", "", aData[i]._AvatarData._iPart, aData[i]);
                        break;

                    case (int)GameEM.EM_RolePart.beard:
                        _ScrollDlg.f_AddPagesScrollItem(1, aData[i]._ID, aData[i]._AvatarName, "", "", aData[i]._AvatarData._iPart, aData[i]);
                        break;

                    case (int)GameEM.EM_RolePart.hair_color:
                        _ScrollDlg.f_AddPagesScrollItem(1, aData[i]._ID, aData[i]._AvatarName, "", "", aData[i]._AvatarData._iPart, aData[i]);
                        break;

                    case (int)GameEM.EM_RolePart.bike:
                        _ScrollDlg.f_AddPagesScrollItem(1, aData[i]._ID, aData[i]._AvatarName, "", "", aData[i]._AvatarData._iPart, aData[i]);
                        break;

                    case (int)GameEM.EM_RolePart.wheel:
                        _ScrollDlg.f_AddPagesScrollItem(1, aData[i]._ID, aData[i]._AvatarName, "", "", aData[i]._AvatarData._iPart, aData[i]);
                        break;

                    case (int)GameEM.EM_RolePart.frame:
                        _ScrollDlg.f_AddPagesScrollItem(1, aData[i]._ID, aData[i]._AvatarName, "", "", aData[i]._AvatarData._iPart, aData[i]);
                        break;

                    default:
                        _ScrollDlg.f_AddScrollItem(aData[i]._ID, aData[i]._AvatarName, "", "", aData[i]._AvatarData._iPart, aData[i], false);
                        break;
                }
            }

            //設定變形器項目
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "身高", "", "", (int)GameEM.EM_Deformer.BodyHeight, null);
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "腰圍", "", "", (int)GameEM.EM_Deformer.Waist, null);
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "胸圍", "", "", (int)GameEM.EM_Deformer.Chest, null);
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "手臂", "", "", (int)GameEM.EM_Deformer.Hand, null);
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "腿", "", "", (int)GameEM.EM_Deformer.LegWidth, null);
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "斜方肌", "", "", (int)GameEM.EM_Deformer.Trapezius, null);
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "臀部", "", "", (int)GameEM.EM_Deformer.Buttocks, null);
            _ScrollDlg.f_AddPagesScrollItem(2, RoleTools.f_CreateKeyId(), "頭頸", "", "", (int)GameEM.EM_Deformer.Head, null);
#endif
        }

        private void f_InitPageView()
        {
#if UNITY_EDITOR
            _RacePage.f_Reset();
            _MeshPage.f_Reset();
            _GenderPage.f_Reset();
            //--------------------------------------------------------------------------------------------------------
            DressEditorData aData = DressEditorManager.GetInstance()._RoleData.Find(delegate (DressEditorData data)
            {
                if ((int)data._Race._RoleAvatar._RoleRace == DressEditorPoolDT._iRace && (int)data._Race._RoleAvatar._RoleGender == DressEditorPoolDT._iGender) { return true; }
                return false;
            });

            //設定性別項目
            List<GridItemDT> tGenderList = new List<GridItemDT>();
            int iInitGender = 0;
            for (int i = 0; i < 2; i++)
            {
                tGenderList.Add(new GridItemDT());
                tGenderList[i].f_AddScrollItem(i, ((GameEM.EM_Gender)i + 1).ToString(), "1", "", i + 1, null);
                if (i + 1 == (int)aData._Race._RoleAvatar._RoleGender)
                {
                    iInitGender = i;
                }
            }
            _GenderPage.f_Init(1, -99, tGenderList, 160, 55, 1, 2, CallBack_OnRaceBtn);
            _GenderPage.f_GetPageItem(0)[iInitGender].f_Select();
            _GenderPage.f_GetCurSelItem().Add(_GenderPage.f_GetPageItem(0)[iInitGender]);
            //設定模型項目
            List<GridItemDT> tMeshList = new List<GridItemDT>();
            tMeshList.Add(new GridItemDT());
            tMeshList[0].f_AddScrollItem(1, "低模", "2", "", 0, null);
            tMeshList.Add(new GridItemDT());
            tMeshList[1].f_AddScrollItem(2, "高模", "2", "", 1, null);
            _MeshPage.f_Init(1, -99, tMeshList, 160, 55, 1, 2, CallBack_OnRaceBtn);
            _MeshPage.f_GetPageItem(0)[0].f_Select();
            _MeshPage.f_GetCurSelItem().Add(_MeshPage.f_GetPageItem(0)[0]);
            //設定人種項目
            List<GridItemDT> tRaceList = new List<GridItemDT>();
            int iInitRace = 0;
            for (int i = 0; i < 3; i++)
            {
                tRaceList.Add(new GridItemDT());
                tRaceList[i].f_AddScrollItem(i, ((GameEM.EM_Race)i).ToString(), "3", "", i, null);
                if (i == (int)aData._Race._RoleAvatar._RoleRace)
                {
                    iInitRace = i;
                }
            }
            _RacePage.f_Init(1, -99, tRaceList, 160, 55, 1, 3, CallBack_OnRaceBtn);
            _RacePage.f_GetPageItem(0)[iInitRace].f_Select();
            _RacePage.f_GetCurSelItem().Add(_RacePage.f_GetPageItem(0)[iInitRace]);
            f_InitSkinPage();
#endif
        }

        private void f_InitSkinPage()
        {
            _SkinPage.f_Reset();
            DressEditorData aData = DressEditorManager.GetInstance()._RoleData.Find(delegate (DressEditorData data)
            {
                if ((int)data._Race._RoleAvatar._RoleRace == DressEditorPoolDT._iRace && (int)data._Race._RoleAvatar._RoleGender == DressEditorPoolDT._iGender) { return true; }
                return false;
            });

            //設定膚色項目
            List<GridItemDT> tSkinList = new List<GridItemDT>();
            int iInitSkin = 0;
            for (int i = 0; i < aData._Race._RoleAvatar._SkinMaterial.Count; i++)
            {
                tSkinList.Add(new GridItemDT());
                tSkinList[i].f_AddScrollItem(i, aData._Race._RoleAvatar._ColorName[i], "4", "", i, aData);
                if ((int)aData._Race._RoleAvatar._RoleRace == i)
                {
                    iInitSkin = i;
                }
            }
            _SkinPage.f_Init(1, -99, tSkinList, 160, 55, 1, 3, CallBack_OnRaceBtn);
            List<GridItem> aGridList = _SkinPage.f_GetPageItem(0);
            aGridList[0].f_Select();
            _SkinPage.f_GetCurSelItem().Add(aGridList[0]);
            CallBack_OnRaceBtn(aGridList[0]);
        }
        #endregion

        #region 按鈕回調
        /// <summary>點擊按鈕後回調</summary>
        private void CallBack_OnClick(object Obj)
        {
#if UNITY_EDITOR
            EditorGridItem tGridItem = (EditorGridItem)Obj;
            MessageBox.DEBUG("Click " + tGridItem.f_GetGridItemDT().f_GetName());
            if (tGridItem.f_GetGridItemDT().f_GetMultilayer())//開啟子分頁
            {
                _ScrollDlg.f_SortPage(ccMath.atoi(tGridItem.f_GetGridItemDT().f_GetBundle()), (int)tGridItem.f_GetGridItemDT().f_GetObj());
            }
            else//點選項目進行對應行為
            {
                _SelEditorDT = (DressEditorData)tGridItem.f_GetGridItemDT().f_GetObj();

                if (_SelEditorDT._EditorAction == DressEditorData.EM_EditorAction.Avatar)
                {//預覽換裝
                    if (tGridItem._CurSelect)
                    {//卸下時裝
                        DressEditorManager.GetInstance().f_TakeOff(_SelEditorDT, _RoleControl);
                        if (_SelEditorDT._AvatarData._iPart == (int)GameEM.EM_RolePart.bike)
                        {
                            Vector3 vPos = _BGSceneInfor._MapObject.GetComponent<SceneInfor_Item>()._InitPos.position;
                            _RoleControl.transform.position = vPos;
                        }
                        _SelEditorDT = null;
                        _ScrollDlg.f_ClearCurSelItem();
                        tGridItem._CurSelect = false;
                    }
                    else
                    {//裝備時裝
                        DressEditorManager.GetInstance().f_ChangeAvatar(_SelEditorDT, _RoleControl);
                        tGridItem._CurSelect = true;
                        if (_SelEditorDT._AvatarData._iPart < (int)GameEM.EM_RolePart.BikeEnd && _SelEditorDT._AvatarData._iPart > (int)GameEM.EM_RolePart.RoleEnd)
                        {
                            f_InitPos();
                        }
                    }
                }
                else if (_SelEditorDT._EditorAction == DressEditorData.EM_EditorAction.Action)
                {//播放指定動作(動畫)
                    GameMain.GetInstance().f_SetAnimClip(_RoleControl, _SelEditorDT._Action._strAnimClip, _SelEditorDT._Action._strBikeAnimClip, _SelEditorDT._Action._bFootIK, _SelEditorDT._Action._bHandIK, _SelEditorDT._Action._bBike);
                }
                else if (_SelEditorDT._EditorAction == DressEditorData.EM_EditorAction.Lens)
                {//載入場景
                    DressEditorManager.GetInstance().f_ChangeSceneLight(_SelEditorDT);
                    f_GameLenContrl(false);
                    GameTools.f_SetGameObject(_oMap, false);
                    GameUITools.f_SetImage(_BG, false);
                    if (_PlayerRaw != null)
                    {
                        _PlayerRaw.gameObject.SetActive(false);
                        _PlayerRaw.enabled = false;
                    }
                }
            }
#endif
        }

        /// <summary>點擊菜單按鈕後回調 </summary>
        public void CallBack_OnClickMenuBtn(object Obj)
        {
            if (!f_IsOpen()) { return; }
#if UNITY_EDITOR
            //設定攝影機鏡頭位置(依點選的時裝菜單去抓對應位置)
            int iEM = (int)Obj;
            string strEM = ((GameEM.EM_RolePart)iEM).ToString();
            MessageBox.DEBUG("SRCamera：" + strEM);
            Transform UpdateCameraPos = _CameraPos.transform.Find(strEM);
            GameUITools.f_SetSRCamera(_SRCamera, UpdateCameraPos);
#endif
        }

        /// <summary>離開換衣間介面</summary>
        void OnClick_BtnExit(GameObject go, object obj1, object obj2)
        {
            f_Close();
        }

        void CallBack_BtnSave(GameObject go, object obj1, object obj2)
        {
            MessageBox.DEBUG("CallBack_BtnSave");
            //1.弹出UI让用户输入Avater数据的名称，长度限制16个汉字
            SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
            tSocketCallbackDT.m_ccCallbackSuc = CallBackSuc_Save;
            tSocketCallbackDT.m_ccCallbackFail = CallBackFail_Save;
            GameMain.GetInstance().f_SetGameUI("UI_EditorInputDlg", BaseUIMessageDef.UI_OPEN, tSocketCallbackDT);
        }

        /// <summary>點擊離開預覽場景按鈕 </summary>
        private void CallBack_OnClickLenExitBtn(GameObject go, object obj1, object obj2)
        {
#if UNITY_EDITOR
            DressEditorManager.GetInstance().f_CallBack_Load();
            f_GameLenContrl(true);
            GameTools.f_SetGameObject(_oMap, true);
            GameUITools.f_SetImage(_BG, true);
            if (_PlayerRaw != null)
            {
                _PlayerRaw.gameObject.SetActive(true);
                _PlayerRaw.enabled = true;
            }
#endif
        }

        /// <summary>點擊人種相關按鈕 </summary>
        private void CallBack_OnRaceBtn(object Obj)
        {
#if UNITY_EDITOR
            GridItem tGridItem = (GridItem)Obj;
            int iIndex = ccMath.atoi(tGridItem.f_GetGridItemDT().f_GetBundle());
            if (iIndex == 1)
            {//性別
                if (DressEditorPoolDT._iGender != tGridItem.f_GetGridItemDT().f_GetClass())
                {//重置時裝
                    DressEditorPoolDT._iGender = tGridItem.f_GetGridItemDT().f_GetClass();
                    _RoleControl.f_InitUserData(DressEditorPoolDT._iGender);
                    _RoleControl.f_LoadAvatarBuf_Editor(GameMain.GetInstance().f_DefualAvatarBuf());
                    CallBack_OnRaceBtn(_SkinPage.f_GetCurSelItem()[0]);
                }
                f_InitScrollDlg();
            }
            else if (iIndex == 2)
            {//模型
                DressEditorPoolDT._iMesh = tGridItem.f_GetGridItemDT().f_GetClass();
                _RoleControl.f_ChangeMesh();
            }
            else if (iIndex == 3)
            {//人種
                DressEditorPoolDT._iRace = tGridItem.f_GetGridItemDT().f_GetClass();
                _RoleControl.f_InitUserDataRace(DressEditorPoolDT._iRace);
                f_InitSkinPage();
            }
            else if (iIndex == 4)
            {//膚色
                _SelEditorDT = (DressEditorData)tGridItem.f_GetGridItemDT().f_GetObj();
                _RoleControl.CallBack_SetColor(_SelEditorDT, tGridItem.f_GetGridItemDT().f_GetClass());
            }
#endif
        }

        /// <summary>點擊菜單顯示按鈕</summary>
        private void CallBack_MenuCtrlBtn(GameObject go, object obj1, object obj2)
        {
            if (_oMenu.activeSelf)
            {
                f_GameObjContrl(false);
                _MenuCtrlBtn.f_Select();
            }
            else
            {
                f_GameObjContrl(true);
                _MenuCtrlBtn.f_UnSelect();
            }
        }

        private float _fBodyHight = 0;
        private void CallBack_BodyHightBtn(object Obj)
        {//更改身高後，重整列表
            float fValue = _RoleControl.f_GetDeformerValue(GameEM.EM_Deformer.BodyHeight.ToString());
            if (fValue != _fBodyHight)
            {
                _fBodyHight = fValue;
                f_InitScrollDlg();
            }
        }
        #endregion

        #region UI顯示
        /// <summary>顯示按鈕UI</summary>
        private void f_GameLenContrl(bool bSet)
        {
            _oLenExitBtn.SetActive(!bSet);
            _oExitBtn.SetActive(bSet);
        }

        /// <summary>顯示UI物件</summary>
        private void f_GameObjContrl(bool bSet)
        {
            _oMenu.SetActive(bSet);
        }
        #endregion

        #region 攝影機相關
        private GameObject _oMap = null;
        private void f_OpenBGCamera()
        {//3D背景攝影機_開啟
            _BGSceneInfor.f_OpenMap();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMap, false);
            _BGSceneInfor.f_OpenMainObj(_RoleControl.gameObject, true);
            _CameraPos = _BGSceneInfor.f_GetGameObj("CameraPos");
            _SRCamera = _BGSceneInfor.f_GetGameObj("SRCamera").GetComponent<SR_Camera>();
            _oMap = _BGSceneInfor.f_GetGameObj("Map");

            if (_PlayerRaw != null)
            {
                _PlayerRaw.gameObject.SetActive(true);
                _PlayerRaw.enabled = true;
                _PlayerRaw.texture = _SRCamera.GetComponent<Camera>().targetTexture;
            }
            ccTimeEvent.GetInstance().f_RegEvent(0.2f, false, null, CallBack_OnClickReturnMenuBtn);
        }

        /// <summary>點擊返回按鈕後回調 </summary>
        public void CallBack_OnClickReturnMenuBtn(object Obj)
        {
            //重置攝影機
            Transform UpdateCameraPos = _CameraPos.transform.Find(GameEM.EM_RolePart.Default.ToString());
            GameUITools.f_SetSRCamera(_SRCamera, UpdateCameraPos);
        }

        private void f_CloseBGCamera()
        {//3D背景攝影機_關閉
            if (_PlayerRaw != null)
            {
                _PlayerRaw.texture = null;
                _PlayerRaw.enabled = false;
            }
            f_CloseRoleControl();
            _BGSceneInfor.f_CloseMap();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMap, true);
        }
        #endregion

        #region 角色相關
        private DressRoleControlV3 oOriginRole = null;
        private int iTime = 0;
        private void f_InitRoleControl()
        {//初始化模型
            oOriginRole = GameMain.GetInstance().m_DressRoleControl;
            _RoleControl = GameObject.Instantiate(oOriginRole.gameObject).GetComponent<DressRoleControlV3>();
            _RoleControl.f_Init(1, new DressRoleActionController(_RoleControl), null);
            _RoleControl.f_InitUserData(DressEditorPoolDT._iGender, DressEditorPoolDT._iRace);
            _RoleControl.name = "EditorModel";
            GameMain.GetInstance().m_DressRoleControl = _RoleControl;
            iTime = ccTimeEvent.GetInstance().f_RegEvent(0.5f, false, null, f_InitRoleControl);
        }

        private void f_InitRoleControl(object obj)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(iTime);
            if (GameMain.GetInstance().m_UseEditor)
            {
                _RoleControl.f_LoadAvatarBuf_Editor(GameMain.GetInstance().f_CreateBuf());
            }
            else
            {
                _RoleControl.f_LoadAvatarBuf(GameMain.GetInstance().f_CreateBuf());
            }
            _fBodyHight = 0;
            _RoleControl.f_Deformer(GameEM.EM_Deformer.BodyHeight.ToString(), _fBodyHight);

            f_InitPos();
        }

        private void f_CloseRoleControl()
        {//關閉模型
            GameMain.GetInstance().m_DressRoleControl = oOriginRole;
            glo_Main.GetInstance().f_DoDestroyObj(_RoleControl.gameObject);
            oOriginRole = null;
        }
        #endregion

        /// <summary>重置角色位置</summary>
        private void f_InitPos()
        {
            Vector3 vPos = _BGSceneInfor._MapObject.GetComponent<SceneInfor_Item>()._InitPos.position;
            _RoleControl.f_InitPos(vPos);
        }

        /// <summary>
        /// 調節腳踏車動畫速度
        /// </summary>
        /// <param name="strValue">數值</param>
        private void f_SetBikeSpeed(string strValue)
        {
            GameMain.GetInstance().f_SetBikeSpeed(_RoleControl, ccMath.atof(strValue));
        }

        private void CallBackSuc_Save(object obj)
        {//儲存資料成功
         //2.向服务器后台提交编辑好的Buf数据
            string strName = (string)obj;
            byte[] aBuf = _RoleControl.f_CreateBuf();
            CreateSocketBuf tCreateSocket = new CreateSocketBuf();
            tCreateSocket.f_Add(DressEditorPoolDT._iGender);
            tCreateSocket.f_Add(DressEditorPoolDT._iRace);
            byte[] tBuf = tCreateSocket.f_GetBuf();
            byte[] NewBuf = new byte[138];
            System.Array.Copy(tBuf, NewBuf, tBuf.Length);
            System.Array.Copy(aBuf, 0, NewBuf, tBuf.Length, aBuf.Length);

            //SaveAvater2PHP.f_Save(strName, aBuf);
            SaveAvater2PHP.f_Save(strName, NewBuf);
            MessageBox.DEBUG("儲存成功：" + strName);
        }

        private void CallBackFail_Save(object obj)
        {//儲存資料失敗
            MessageBox.ASSERT("儲存失敗");
        }
    }
}