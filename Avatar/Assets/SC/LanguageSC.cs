using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageDT
{
    public string szId;
    public string szUseImage;
    public string szLanuageKey;
    public string[] aText;


    public string f_GetLocaleText()
    {
        if (LanguageManager.GetInstance().f_GetCurLanguage() == Locale.zhTW)
        {
            return aText[3];
        }
        else if (LanguageManager.GetInstance().f_GetCurLanguage() == Locale.zhCN)
        {
            return aText[4];
        }
        else if (LanguageManager.GetInstance().f_GetCurLanguage() == Locale.enUS)
        {
            return aText[5];
        }
        else if (LanguageManager.GetInstance().f_GetCurLanguage() == Locale.koKR)
        {
            return aText[6];
        }
        else if (LanguageManager.GetInstance().f_GetCurLanguage() == Locale.jaJP)
        {
            return aText[7];
        }
        else if (LanguageManager.GetInstance().f_GetCurLanguage() == Locale.ruRU)
        {
            return aText[8];
        }
        MessageBox.ASSERT("未支持的文本。" + szLanuageKey);
        return "";
    }

    public string f_GetLocaleText(params object[] values)
    {
        string localeText = f_GetLocaleText();
        return string.Format(localeText, values);
    }
}

public class LanguageSC : NBaseSC
{
   

    string[] _aTitle = null;
    private Locale _Locale = Locale.None;
    Dictionary<string, LanguageDT> _dirData = new Dictionary<string, LanguageDT>();


    public LanguageSC()
    {
        Create("LanguageSC");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }
       
    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LanguageDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        { 
            try
            {
                if (tFoddScData[i] == "")
                {
                    MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                
                DataDT = new LanguageDT();
                DataDT.szId = tData[0];
                DataDT.szLanuageKey = tData[1];
                DataDT.szUseImage = tData[2];
                DataDT.aText = tData;
                SaveItemData(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
                continue;
            }
        }
    }

    private void SaveItemData(LanguageDT tLanguageDT)
    {
        LanguageDT tNBaseSCDT = f_GetLanguageDT(tLanguageDT.szLanuageKey);
        if (tNBaseSCDT == null)
        {
            _dirData.Add(tLanguageDT.szLanuageKey, tLanguageDT);
        }
        else
        {
            MessageBox.ASSERT(tLanguageDT.szLanuageKey + "语言关健已存在");
        }
    }

    public LanguageDT f_GetLanguageDT(string szLanuageKey)
    {
        LanguageDT tNBaseSCDT = null;
        if (_dirData.TryGetValue(szLanuageKey, out tNBaseSCDT))
        {
            return tNBaseSCDT;
        }
        return null;
    }


}
