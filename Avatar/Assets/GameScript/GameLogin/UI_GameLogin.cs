
using UnityEngine;
using UnityEngine.SceneManagement;
using ccU3DEngine;
using UnityEngine.UI;
//using ccGameControllSDK;


namespace GameLogic
{
    public class UI_GameLogin : ccUILogicBase
    {

        protected override void On_Init()
        {
            f_RegClickEvent(f_GetObject("LoginBtn"), OnClick_LoginBtn);

            f_RegHoverEvent("LoginBtn", OnHover_LoginBtn);
            //ccTimeEvent.GetInstance().f_RegEvent(1, false, null, AutoLogin);            
        }

        protected override void On_Destory()
        {

        }

        protected override void On_UpdateGUI()
        {

        }

        protected override void On_Open(object e)
        {

        }

        protected override void On_Close()
        {

        }

        void AutoLogin(object Obj)
        {
            MessageBox.DEBUG("AutoLogin");
            SceneManager.LoadScene("GameMain");
        }

        void OnHover_LoginBtn(GameObject go, object obj1, object obj2)
        {
            bool bHover = (bool)obj1;
            if (bHover)
            {
                MessageBox.DEBUG("Hover_In");
            }
            else
            {
                MessageBox.DEBUG("Hover Leave");
            }
        }

        void OnClick_LoginBtn(GameObject go, object obj1, object obj2)
        {
            MessageBox.DEBUG("OnClick_LoginBtn");

            StaticValue.m_LoginName = f_GetObject("NameText").GetComponent<InputField>().text;
            StaticValue.m_LoginPwd = f_GetObject("PwdText").GetComponent<InputField>().text;
            GameSocket.GetInstance().f_Login(Callback_Login);

        }

        /// <summary>
        /// 登录游戏回调
        /// </summary>
        /// <param name="data">服务器返回的数据</param>
        private void Callback_Login(object Obj)
        {
            eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)Obj;
            switch (teMsgOperateResult)
            {
                case eMsgOperateResult.OR_Succeed:
                    MessageBox.DEBUG("登陆成功");
                    StaticValue.m_CurrentScene = "GameMain";
                    ccUIManage.GetInstance().f_SendMsgV3("ui_gameinit.bundle", "UI_GameInit", BaseUIMessageDef.UI_OPEN);
                    break;
                case eMsgOperateResult.OR_Error_NoAccount:
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "没有此账号！");
                    break;
                case eMsgOperateResult.OR_Error_Password:
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "密码错误！");
                    break;
                //case eMsgOperateResult.eOR_CreateAndLogin://登录并创角//打开创角界面已经写在Socket_SelCharacter里面
                //    LocalDataManager.f_SetLocalData<string>(LocalDataType.String_UserName, strName);
                //    LocalDataManager.f_SetLocalData<string>(LocalDataType.String_Password, strPwd);
                //    mIsEnter = true;
                //    break;
                case eMsgOperateResult.OR_Error_SeverMaintain:
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "服务器维护！");
                    break;
                case eMsgOperateResult.OR_Error_VersionNotMatch:
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "版本不匹配,请重新下载！");
                    break;
                case eMsgOperateResult.eOR_IP_Forbidden:
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Ip封禁！");
                    break;
                case eMsgOperateResult.eOR_Account_Forbidden:
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "账号封禁！");
                    break;
                default://其他原因
                    //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "登录失败：" + teMsgOperateResult.ToString());
                    break;
            }
           
        }
    }
}