
//============================================
//
//    Character来自Character.xlsx文件自动生成脚本
//    2021/9/28 17:54:31
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CharacterDT : NBaseSCDT
{

    /// <summary>
    /// 角色名
    /// </summary>
    public string szName;
    /// <summary>
    /// 角色类型
    /// </summary>
    public int iType;
    /// <summary>
    /// 資源名
    /// </summary>
    public string szResName;
    /// <summary>
    /// 资源类型0為Resource,1為AB
    /// </summary>
    public string szResType;
    /// <summary>
    /// 人物說明(中文)
    /// </summary>
    public string strReadme;
    /// <summary>
    /// 跑步速度
    /// </summary>
    public float fRunSpeed;
    /// <summary>
    /// 身体高
    /// </summary>
    public float fHeight;
    /// <summary>
    /// 身体大小
    /// </summary>
    public float fBodySize;
    /// <summary>
    /// 是否可以被侦测0可以被侦测1隐身不能被侦测
    /// </summary>
    public int iNoFind;
    /// <summary>
    /// AI列表
    /// </summary>
    public string szAI;
    /// <summary>
    /// 按鈕資源名
    /// </summary>
    public string szResBtn;
}
