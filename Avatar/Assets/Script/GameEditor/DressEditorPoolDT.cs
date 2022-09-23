using UnityEngine;

/// <summary>
/// 儲存編輯器資料
/// </summary>
public class DressEditorPoolDT
{
    public static int _iGender = (int)GameEM.EM_Gender.Female;
    public static int _iRace = (int)GameEM.EM_Race.Asian;
    public static int _iMesh = 1;

    public static void f_SetRoleData(int iGender, int iRace, int iMesh)
    {
        _iGender = iGender;
        _iRace = iRace;
        _iMesh = iMesh;
    }
}
