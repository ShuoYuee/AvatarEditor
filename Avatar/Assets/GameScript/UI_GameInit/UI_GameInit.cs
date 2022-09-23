using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace GameLogic
{
    public class UI_GameInit : ccUILogicBase
    {
        public Slider m_Progress;

        private AsyncOperation _async;
        private float targetValue;


        protected override void On_Init()
        {
            m_Progress = f_GetObject("Progress").GetComponent<Slider>();

            MessageBox.DEBUG("启用游戏包中的UI_GameInit脚本");

            m_Progress.value = 0;
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UpdateInitProgress, On_UI_UpdateInitProgress);
            CanageScene();//加載場景
        }

        protected override void On_Destory()
        {
            glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UpdateInitProgress, On_UI_UpdateInitProgress);
        }

        protected override void On_UpdateGUI()
        {
            
        }

        protected override void On_Update()
        {
            //場景加載進度條
            if (m_Progress != null)
            {
                targetValue = ccSceneMgr.GetInstance().m_fSceneLoadProgressValue;
                if (targetValue >= 0.9f)
                {
                    m_Progress.value = 1f;
                }

                if (m_Progress.value != targetValue)
                {
                    m_Progress.value = Mathf.Lerp(m_Progress.value, targetValue, Time.deltaTime);

                    if (Mathf.Abs(m_Progress.value - targetValue) < 0.01f)
                    {
                        m_Progress.value = targetValue;
                    }
                }
                if ((int)(m_Progress.value * 100) >= 99)
                {
                    ccSceneMgr.GetInstance().f_SceneLoadComplete();
                }
            }
        }

        protected override void On_Open(object e)
        {

        }

        protected override void On_Close()
        {

        }


        private void On_UI_UpdateInitProgress(object Obj)
        {
            m_Progress.value = (float)Obj;
        }

        //轉變場景
        public void CanageScene()
        {
            //ccSceneMgr.GetInstance().f_ChangeScene(Enum.GetName(typeof(EM_GameScene), NextSceneID), true);
            ccSceneMgr.GetInstance().f_ChangeScene(StaticValue.m_CurrentScene, true);
        }
    }

}