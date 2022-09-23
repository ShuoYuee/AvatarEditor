using System;
using UnityEngine;

/// <summary>
/// 編輯器資料
/// </summary>
[CreateAssetMenu]
public class DressEditorData : ScriptableObject
{
    [EnumAttirbute("資料類別")]
    public EM_EditorAction _EditorAction;

    [Rename("ID")]
    public int _ID;
    [Rename("名稱")]
    public string _AvatarName;

    public Editor_AvatarData _AvatarData = new Editor_AvatarData();
    public Editor_Race _Race = new Editor_Race();
    public Editor_Action _Action = new Editor_Action();
    public Editor_Scene _SceneData = new Editor_Scene();

    public enum EM_EditorAction
    {
        [EnumAttirbute("時裝")]
        Avatar,
        [EnumAttirbute("人種")]
        RoleRace,
        [EnumAttirbute("動作")]
        Action,
        [EnumAttirbute("鏡頭")]
        Lens,
    }
}

/// <summary>
/// 時裝
/// </summary>
[Serializable]
public class Editor_AvatarData
{
    #region 時裝資料
    [Rename("低模時裝物件")]
    public GameObject _AvatarLowObj;
    [Rename("高模時裝物件")]
    public GameObject _AvatarHightObj;
    [Rename("材質球")]
    public Material _AvatarMaterial;
    [Rename("材質貼圖")]
    public Texture _AvatarTexture;
    [Rename("色票")]
    public Color _Color = Color.white;

    [EnumAttirbute("時裝部位")]
    public RolePart _Part;
    [EnumAttirbute("性別")]
    public GameEM.EM_Gender _Gender;
    [EnumAttirbute("尺寸")]
    public GameEM.EM_BikeSize _Size;

    public int _iPart
    {
        get
        {
            return (int)_Part;
        }
    }

    public int _iGender
    {
        get
        {
            return (int)_Gender;
        }
    }

    [Rename("卸裝後的基礎時裝")]
    [Tooltip("若為空，則直接卸下該時裝部位")]
    public DressEditorData _TakeOffData = null;

    public enum RolePart
    {
        [EnumAttirbute("頭部")]
        head = 1,
        [EnumAttirbute("鬍子")]
        beard = 4,
        [EnumAttirbute("髮型")]
        hair = 5,
        [EnumAttirbute("髮色")]
        hair_color = 6,
        [EnumAttirbute("頭盔")]
        helmet = 7,
        [EnumAttirbute("風鏡")]
        goggles = 11,
        [EnumAttirbute("上半身服飾")]
        jersey = 12,
        [EnumAttirbute("下半身服飾")]
        pants = 13,
        [EnumAttirbute("手套")]
        gloves = 14,
        [EnumAttirbute("襪子")]
        socks = 15,
        [EnumAttirbute("鞋子")]
        shoes = 16,
        [EnumAttirbute("配件")]
        Accessories = 17,
        [EnumAttirbute("寵物")]
        pet = 18,
        [EnumAttirbute("特殊造型")]
        cosplay = 19,
        [EnumAttirbute("限量車款")]
        bike = 50,
        [EnumAttirbute("車架")]
        frame = 51,
        [EnumAttirbute("車輪")]
        wheel = 52,
    }
    #endregion

}

/// <summary>
/// 人種
/// </summary>
[Serializable]
public class Editor_Race
{
    public RoleAvaterDT _RoleAvatar;
}

/// <summary>
/// 動作
/// </summary>
[Serializable]
public class Editor_Action
{
    [Rename("角色動畫名稱")]
    [Tooltip("需先將該Clip放入角色Animator中才可生效")]
    public string _strAnimClip = "";

    [Rename("腳踏車動畫名稱")]
    [Tooltip("需先將該Clip放入腳踏車Animator中才可生效")]
    public string _strBikeAnimClip = "";

    [Rename("腳部IK")]
    [Tooltip("是否啟用IK")]
    public bool _bFootIK = true;

    [Rename("手部IK")]
    [Tooltip("是否啟用IK")]
    public bool _bHandIK = true;

    [Rename("自行車顯示")]
    [Tooltip("是否顯示自行車")]
    public bool _bBike = true;
}

/// <summary>
/// 場景
/// </summary>
[Serializable]
public class Editor_Scene
{
    [EnumAttirbute("加載類型")]
    public EM_EditorSecene _EditorSecene;
    [Rename("場景名稱")]
    public string _strScene = "";
    [Rename("場景物件")]
    public GameObject _oSceneModel;

    public enum EM_EditorSecene
    {
        [EnumAttirbute("須載入場景")]
        LoadScene = 1,
        [EnumAttirbute("不須載入場景")]
        LoadLight = 2,
    }
}

///// <summary>
///// 初始化資料
///// </summary>
//[Serializable]
//public class Editor_InitRole
//{
//    [Header("初始化資料")]
//    [EnumAttirbute("人種")]
//    public GameEM.EM_Race _RoleRace;
//    [EnumAttirbute("性別")]
//    public RoleGender _RoleGender;
//    [EnumAttirbute("身高")]
//    public RoleHight _RoleHight;

//    public enum RoleGender
//    {
//        [EnumAttirbute("男性")]
//        Male = 1,
//        [EnumAttirbute("女性")]
//        Female = 2,
//    }

//    public enum RoleHight
//    {
//        [EnumAttirbute("矮")]
//        Low = 0,
//        [EnumAttirbute("高")]
//        Hight = 1,
//    }
//}

[AttributeUsage(AttributeTargets.Field)]
public class EnumAttirbute : HeaderAttribute
{
    public string strName;
    public EnumAttirbute(string name) : base(name)
    {
        this.strName = name;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class RenameAttribute : PropertyAttribute
{
    public string strNmae = "";
    public RenameAttribute(string name)
    {
        strNmae = name;
    }
}