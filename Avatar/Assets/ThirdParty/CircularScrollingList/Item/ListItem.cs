using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : ListBoxBase
{
    /// <summary>說明文字套件</summary>
    public Text m_Text;
    /// <summary>物件圖片套件</summary>
    public Image m_Image;
    public Image m_Highlight;
    public Text m_Num;

    /// <summary>更新顯示資訊</summary>
    protected override void UpdateDisplayContent(object content)
    {
        //var team = (TeamData)content;
        
        // image.sprite = level.girl.sprite;
        //m_Text.text = m_strName;
        //m_Image.sprite = m_Logo;
        //m_Num.text = m_strNum;
    }
}