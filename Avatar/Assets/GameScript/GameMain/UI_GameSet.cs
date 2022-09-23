using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace GameLogic
{
    public class UI_GameSet : ccUILogicBase
    {
        private Slider MusicSlider, SoundSlider;

        //PowerIndicator _PowerIndicator;
        protected override void On_Init()
        {
            MessageBox.DEBUG("启用游戏包中的UI_GameSet脚本");

            f_RegClickEvent(f_GetObject("BtnExit"), OnClick_BtnExit);
            f_RegClickEvent(f_GetObject("BtnZnTW"), OnClick_BtnZnTW);
            f_RegClickEvent(f_GetObject("BtnZnCN"), OnClick_BtnZnCN);
            f_RegClickEvent(f_GetObject("BtnEnUS"), OnClick_BtnEnUS);

            MusicSlider = f_GetObject("MusicSlider").GetComponent<Slider>();
            SoundSlider = f_GetObject("SoundSlider").GetComponent<Slider>();

            //同步UI介面的音效和音量的數值
            MusicSlider.value = StaticValue.m_fBgmVolume;
            SoundSlider.value = StaticValue.m_fSoundVolume;
        }
        
        protected override void On_Open(object e)
        {
           
        }

      
        protected override void On_Close()
        {
           
        }

        protected override void On_Update()
        {
            base.On_Update();

            //調整音量
            if (StaticValue.m_fBgmVolume != MusicSlider.value)
            {
                StaticValue.m_fBgmVolume = MusicSlider.value;
                glo_Main.GetInstance().m_AudioManager.f_SetBGMVolume(MusicSlider.value);
            }
            //調整音效
            if (StaticValue.m_fSoundVolume != SoundSlider.value)
            {
                StaticValue.m_fSoundVolume = SoundSlider.value;
            }

        }

        protected override void On_UpdateGUI()
        {

        }


        void OnClick_BtnExit(GameObject go, object obj1, object obj2)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
            f_Close();           
        }

        //繁體中文語言按鈕
        void OnClick_BtnZnTW(GameObject znTW, object obj1, object obj2)
        {
            Locale locale = Locale.zhTW;
            LanguageManager.GetInstance().f_ChangeLanguage(locale);
        }

        //簡體中文語言按鈕
        void OnClick_BtnZnCN(GameObject znCN, object obj1, object obj2)
        {
            Locale locale = Locale.zhCN;
            LanguageManager.GetInstance().f_ChangeLanguage(locale);
        }

        //英文語言按鈕
        void OnClick_BtnEnUS(GameObject enUS, object obj1, object obj2)
        {
            Locale locale = Locale.enUS;
            LanguageManager.GetInstance().f_ChangeLanguage(locale);
        }

        private void DoExit()
        {
           
        }

        protected override void On_Destory()
        {

        }



    }
}