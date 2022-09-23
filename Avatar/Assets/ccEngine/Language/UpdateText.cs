using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ccU3DEngine;


public class UpdateText : MonoBehaviour
{
    private Text _Text = null;
    private Image _TextImage = null;
    public string m_strLanuageKey = "";
    private LanguageDT _LanguageDT = null;


    void Start()
    {
        _Text = gameObject.GetComponent<Text>();
        _TextImage = transform.GetComponentInChildren<Image>();
        if (_Text == null)
        {           
            MessageBox.ASSERT("UpdateText设置的对象未找到Text组件." + gameObject.name);
        }
        else if (_TextImage == null)
        {
            MessageBox.ASSERT("UpdateText设置的对象未找到Image组件." + gameObject.name);
        }
        else
        {
            LanguageManager.GetInstance().f_RegUpdateText(this);
            f_Update();
        }
    }

    /// <summary>
    /// 更新Text文字显示
    /// </summary>
    public void f_Update()
    {
        if (_Text != null)
        {
            if (_LanguageDT == null)
            {
                _LanguageDT = glo_Main.GetInstance().m_SC_Pool.m_LanguageSC.f_GetLanguageDT(m_strLanuageKey);
                if (_LanguageDT == null)
                {
                    MessageBox.ASSERT(gameObject.name + ",UpdateText设置语言Key未找到." + m_strLanuageKey);
                }
            }

            switch (_LanguageDT.szUseImage)
            {
                case "true"://使用圖像字
                    _TextImage.sprite = glo_Main.GetInstance().m_ResourceManager.f_LoadResourceImage(_LanguageDT.f_GetLocaleText());
                    if (_TextImage.color.a <= 0) { _TextImage.color = Color.white; }
                    if (!_TextImage.preserveAspect) { _TextImage.preserveAspect = true; }
                    if (_Text.text != "") { _Text.text = ""; }
                    break;

                default://使用系統字
                    _Text.text = _LanguageDT.f_GetLocaleText();
                    if (_TextImage.color.a > 0) { _TextImage.color = Color.clear; }
                    break;
            }
        }
    }
    
    private void OnDestroy()
    {
        LanguageManager.GetInstance().f_UnRegUpdateText(this);
    }

}
