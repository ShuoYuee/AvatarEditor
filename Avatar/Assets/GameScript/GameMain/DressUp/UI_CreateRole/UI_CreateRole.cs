using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using System;
using SR;

namespace GameLogic
{
    public class UI_CreateRole : ccUILogicBase
    {
        private GridPageView _ScrollDlg;
        private List<BaseAvaterDT> _AvatarList = new List<BaseAvaterDT>();
        private DressRoleControlV3 _RoleControl;
        private SceneInfor_3D _BGSceneInfor;
        private SR_Camera _SRCamera;

        protected override void On_Init()
        {
            MessageBox.DEBUG("啟用遊戲包中的UI_CreateRole腳本");
            _ScrollDlg = GameUITools.f_CreateGridPage(f_GetObject("ScrollMenu"));
            _BGSceneInfor = f_GetObject("BGScene").GetComponent<SceneInfor_3D>();

            f_RegClickEvent(f_GetObject("BtnExit"), CallBackBtn_Exit);
            f_RegClickEvent(f_GetObject("BtnSave"), CallBackBtn_Save);
        }

        protected override void On_Open(object e)
        {
            f_LoadWWW();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMainUI, false);
        }

        private void f_InitMessage()
        {

        }

        private void f_InitScrollDlg()
        {
            MessageBox.DEBUG("初始化Avatar清單");
            _ScrollDlg.f_Reset();

            List<GridItemDT> tData = new List<GridItemDT>();
            for(int i = 0; i < _AvatarList.Count; i++)
            {//生成Avatar選項
                GridItemDT tGridItemDT = new GridItemDT();
                tGridItemDT.f_AddScrollItem(_AvatarList[i].m_iId, _AvatarList[i].m_strName, "", _AvatarList[i].m_strLogo, 0, _AvatarList[i]);
                tData.Add(tGridItemDT);
            }
            _ScrollDlg.f_Init(1, -99, tData, 168, 55, 9, 1, CallBackBtn_Avatar, false);
        }

        private void f_LoadWWW()
        {
            SaveAvater2PHP.f_LoadWWW(CallBack_LoadResult);
        }

        private void CallBack_LoadResult(object Obj)
        {
            string ppSQL = (string)Obj;

            _AvatarList.Clear();
            string[] aData = ccMath.f_String2ArrayString(ppSQL, ";");
            for (int i = 0; i < aData.Length; i++)
            {
                string[] aItem = ccMath.f_String2ArrayString(aData[i], ":");
                if (aItem.Length == 3)
                {
                    BaseAvaterDT tBaseAvaterDT = new BaseAvaterDT();
                    tBaseAvaterDT.m_iId = ccMath.atoi(aItem[0]);
                    tBaseAvaterDT.m_strName = aItem[1];

                    tBaseAvaterDT.m_aBuf = Convert.FromBase64String(aItem[2]);
                    _AvatarList.Add(tBaseAvaterDT);
                }
            }
            MessageBox.DEBUG("加載基礎模型資料成功");

            f_InitScrollDlg();
            f_OpenBGCamera();
        }

        protected override void On_Close()
        {
            MessageBox.DEBUG("關閉遊戲包中的UI_CreateRole腳本");
            f_CloseBGCamera();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMainUI, true);
        }

        private void f_UnMessage()
        {

        }

        protected override void On_Destory()
        {

        }

        protected override void On_UpdateGUI()
        {

        }

        protected override void On_Update()
        {
            base.On_Update();
            On_UpdateGUI();
        }

        private void CallBackBtn_Exit(GameObject go, object obj1, object obj2)
        {
            f_Close();
        }

        private void CallBackBtn_Avatar(object obj)
        {//點擊後進行加載
            GridItem tGridItem = (GridItem)obj;
            BaseAvaterDT tBaseAvaterDT = (BaseAvaterDT)tGridItem.f_GetGridItemDT().f_GetObj();
            byte[] NewBuf_User = new byte[8];
            System.Array.Copy(tBaseAvaterDT.m_aBuf, NewBuf_User, 2 * 4);
            //取得設定好的屬性(性別、人種)------------>可從此處做屬性設定
            ReadBuf tReadBuf = new ReadBuf();
            tReadBuf.f_Save(NewBuf_User, NewBuf_User.Length);
            int iGender = tReadBuf.f_ReadInt();
            int iRace = tReadBuf.f_ReadInt();
            _RoleControl.f_InitUserData(iGender, iRace);

#if UNITY_EDITOR
            //同步設定編輯器數據
            DressEditorPoolDT._iGender = iGender;
            DressEditorPoolDT._iRace = iRace;
#endif

            //取得設定好的時裝資料------------>由於是用於創角，不包含特殊造型
            byte[] NewBuf_Avatar = new byte[140];
            System.Array.Copy(tBaseAvaterDT.m_aBuf, 2 * 4, NewBuf_Avatar, 0, tBaseAvaterDT.m_aBuf.Length - 2 * 4);
            if (GameMain.GetInstance().m_UseEditor)
            {//加載的數據將對應DressEditData的ID
#if UNITY_EDITOR
                _RoleControl.f_LoadAvatarBuf_Editor(NewBuf_Avatar);
#endif
            }
            else
            {//加載的數據將對應AvaterPart.xlsx的ID
                _RoleControl.f_LoadAvatarBuf(NewBuf_Avatar);
            }
        }

        private void CallBackBtn_Save(GameObject go, object obj1, object obj2)
        {//時裝儲存(保存至內存)
            SocketCallbackDT tSocketCallback = new SocketCallbackDT();
            tSocketCallback.m_ccCallbackSuc = CallBackSuc_Save;//儲存成功後回調
            tSocketCallback.m_ccCallbackFail = CallBackFail_Save;//儲存失敗後回調
            _RoleControl.f_SaveAvater(tSocketCallback);//儲存
        }

        private void CallBackSuc_Save(object obj)
        {//儲存資料成功
            MessageBox.DEBUG("儲存成功");
        }

        private void CallBackFail_Save(object obj)
        {//儲存資料失敗
            MessageBox.ASSERT("儲存失敗");
        }

        #region 角色相關
        private DressRoleControlV3 oOriginRole = null;
        private void f_InitRoleControl()
        {//初始化模型
            oOriginRole = GameMain.GetInstance().m_DressRoleControl;
            _RoleControl = GameObject.Instantiate(oOriginRole.gameObject).GetComponent<DressRoleControlV3>();
            _RoleControl.f_Init(1, new DressRoleActionController(_RoleControl), null);
            _RoleControl.f_InitUserData(DressEditorPoolDT._iGender, DressEditorPoolDT._iRace);
            _RoleControl.name = "EditorModel";
            GameMain.GetInstance().m_DressRoleControl = _RoleControl;
            if (GameMain.GetInstance().m_UseEditor)
            {
#if UNITY_EDITOR
                _RoleControl.f_LoadAvatarBuf_Editor(GameMain.GetInstance().f_CreateBuf());
#endif
            }
            else
            {
                _RoleControl.f_LoadAvatarBuf(GameMain.GetInstance().f_CreateBuf());
            }
            f_InitPos();
        }

        /// <summary>重置角色位置</summary>
        private void f_InitPos()
        {
            Vector3 vPos = _BGSceneInfor._MapObject.GetComponent<SceneInfor_Item>()._InitPos.position;
            _RoleControl.f_InitPos(vPos);
        }

        private void f_CloseRoleControl()
        {//關閉模型
            GameMain.GetInstance().m_DressRoleControl = oOriginRole;
            glo_Main.GetInstance().f_DoDestroyObj(_RoleControl.gameObject);
            oOriginRole = null;
        }
        #endregion

        #region 攝影機相關
        private void f_OpenBGCamera()
        {//3D背景攝影機_開啟
            _BGSceneInfor.f_OpenMap();
            f_InitRoleControl();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMap, false);
            _BGSceneInfor.f_OpenMainObj(_RoleControl.gameObject, true);
            _SRCamera = _BGSceneInfor.f_GetGameObj("SRCamera").GetComponent<SR_Camera>();
            ccTimeEvent.GetInstance().f_RegEvent(0.2f, false, null, CallBack_SR);
        }

        public void CallBack_SR(object Obj)
        {
            GameUITools.f_SetSRCamera(_SRCamera, _RoleControl.transform);
        }

        private void f_CloseBGCamera()
        {//3D背景攝影機_關閉
            f_CloseRoleControl();
            _BGSceneInfor.f_CloseMap();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_GameMap, true);
        }
        #endregion
    }
}
