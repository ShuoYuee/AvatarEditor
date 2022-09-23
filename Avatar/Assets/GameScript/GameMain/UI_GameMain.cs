using ccU3DEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class UI_GameMain : ccUILogicBase
    {
        protected override void On_Init()
        {
            MessageBox.DEBUG("启用游戏包中的UI_GameMain脚本");
            Canvas tCanvas = f_GetCanvas();
            InitMessage();
        }

        private void InitMessage()
        {
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_GameMainUI, f_GameMainUIActive);//開關主UI
        }

        protected override void On_Open(object e)
        {
            GameUIOpen();
            if (!GameDataLoad.f_LoadGameStep())
            {
                f_GetObject("BtnContinue").GetComponent<Button>().interactable = false;
            }
        }

      
        protected override void On_Close()
        {
           
        }

        protected override void On_Update()
        {
            base.On_Update();
        }

        protected override void On_UpdateGUI()
        {

        }

        private void DoExit()
        {
           
        }

        protected override void On_Destory()
        {

        }

        float GetPower()
        {


            return 0;
        }

        #region 游戏UI管理

        private void f_GameMainUIActive(object e)
        {
            f_GetObject("Panel").SetActive((bool)e);
        }

        private void GameUIClose()
        {

        }

        private void GameUIOpen()
        {

        }
        #endregion
    }
}