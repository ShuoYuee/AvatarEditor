using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoleData", menuName = "RoleData/DataSetting", order = 0)]
public class RoleAvaterDT : ScriptableObject
{//模型Avatar資料(包含指定性別及人種的模型膚色、Avatar設定)
    //人種
    public GameEM.EM_Race _RoleRace;
    //性別
    public RoleGender _RoleGender;

    [Space(10)]
    //膚色材質球
    [Tooltip("按照材質順序排列")]
    public List<Material> _SkinMaterial;
    //膚色材質貼圖
    [Tooltip("按照材質順序排列")]
    public List<Texture> _Texture;
    //膚色色票
    [Tooltip("按照材質順序排列")]
    public List<Color> _Color;
    //膚色名稱
    [Tooltip("按照材質順序排列")]
    public List<string> _ColorName;


    [Space(10)]
    //低模模型
    [Tooltip("按照身高順序，由矮至高")]
    public List<Avatar> _LowModel;
    //高模模型
    [Tooltip("按照身高順序，由矮至高")]
    public List<Avatar> _HightModel;

    public enum RoleGender
    {
        //男性
        Male = 1,
        //女性
        Female = 2,
    }

    public bool f_CheckRole(int iRace, int iGender)
    {
        return iRace == (int)_RoleRace && iGender == (int)_RoleGender;
    }

    public Material f_GetSkinMaterial(int iIndex)
    {
        if (iIndex >= _SkinMaterial.Count) { return null; }
        return _SkinMaterial[iIndex];
    }

    public Texture f_GetSkinTexture(int iIndex)
    {
        if (iIndex >= _Texture.Count) { return null; }
        return _Texture[iIndex];
    }

    public Color f_GetSkinColor(int iIndex)
    {
        if (iIndex >= _Color.Count) { return Color.white; }
        return _Color[iIndex];
    }

    public string f_GetColorName(int iIndex)
    {
        if (iIndex >= _Color.Count) { return ""; }
        return _ColorName[iIndex];
    }

    public Avatar f_GetAvatar(int iIndex, int iMode)
    {
        if (iMode == 0)
        {
            if (iIndex >= _LowModel.Count) { return null; }
            return _LowModel[iIndex];
        }
        else if (iMode == 1)
        {
            if (iIndex >= _HightModel.Count) { return null; }
            return _HightModel[iIndex];
        }
        return null;
    }
}
