using ccU3DEngine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ccU3DEngine
{
    /// <summary>
    /// 语言管理器
    /// </summary>
    public class LanguageManager
    {
        Dictionary<UpdateText, UpdateText> _dirUpdateText = new Dictionary<UpdateText, UpdateText>();

        private Locale _Locale = Locale.None;
        private static LanguageManager _Instance = null;
        public static LanguageManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new LanguageManager();
            }
            return _Instance;
        }

        #region 内部方法

        void SetLanguage(Locale tLocale)
        {
            if (_Locale == tLocale)
            {
                return;
            }
            _Locale = tLocale;
        }
               
        internal LanguageDT f_GetLanguageDT(string strLanuageKey)
        {
            return glo_Main.GetInstance().m_SC_Pool.m_LanguageSC.f_GetLanguageDT(strLanuageKey);
        }

        internal void f_RegUpdateText(UpdateText tUpdateText)
        {
            if (_dirUpdateText.ContainsKey(tUpdateText))
            {
                return;
            }
            _dirUpdateText.Add(tUpdateText, tUpdateText);
        }

        internal void f_UnRegUpdateText(UpdateText tUpdateText)
        {
            if (!_dirUpdateText.ContainsKey(tUpdateText))
            {
                return;
            }
            _dirUpdateText.Remove(tUpdateText);
        }

        private void UpdateAllText()
        {
            UpdateText tCurItem = null;
            try
            {
                foreach (KeyValuePair<UpdateText, UpdateText> tItem in _dirUpdateText)
                {
                    tCurItem = tItem.Value;
                    if (tItem.Value != null)
                    {
                        tItem.Value.f_Update();
                    }
                }
            }
            catch
            {
                f_UnRegUpdateText(tCurItem);
            }
        }

        #endregion


        #region 外部接口

        public Locale f_GetCurLanguage()
        {
            return _Locale;
        }

        /// <summary>
        /// 切换语言，切换语言时会同时刷新相应的显示UI的文字信息
        /// </summary>
        /// <param name="tLocale"></param>
        public void f_ChangeLanguage(Locale tLocale)
        {
            if (tLocale == Locale.None)
            {
                MessageBox.ASSERT("未设置合法的语言类型. " + _Locale.ToString());
            }
            if (tLocale == f_GetCurLanguage())
            {
                return;
            }
            SetLanguage(tLocale);
            UpdateAllText();
        }


        /// <summary>
        /// 获取语言KEY对应的文字信息
        /// </summary>
        /// <param name="strLanuageKey">语言KEY</param>
        /// <param name="values">参数列表</param>
        /// <returns></returns>
        public string f_GetText(string strLanuageKey, params object[] values)
        {
            LanguageDT _LanguageDT = glo_Main.GetInstance().m_SC_Pool.m_LanguageSC.f_GetLanguageDT(strLanuageKey);
            return _LanguageDT.f_GetLocaleText(values);
        }

        /// <summary>
        /// 获取语言KEY对应的文字信息
        /// </summary>
        /// <param name="strLanuageKey">语言KEY</param>
        /// <returns></returns>
        public string f_GetText(string strLanuageKey)
        {
            LanguageDT _LanguageDT = glo_Main.GetInstance().m_SC_Pool.m_LanguageSC.f_GetLanguageDT(strLanuageKey);
            return _LanguageDT.f_GetLocaleText();
        }

        #endregion

    }
}