using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoleData", menuName = "RoleData/DataList", order = 1)]
public class RoleAvaterList : ScriptableObject
{
    [Tooltip("儲存指定性別及人種的相關設定")]
    public List<RoleAvaterDT> _RoleAvaterList = new List<RoleAvaterDT>();

    public RoleAvaterDT f_GetRoleDT(int iRace, int iGender)
    {
        if (_RoleAvaterList.Count == 0) { return null; }
        for(int i = 0; i < _RoleAvaterList.Count; i++)
        {
            if(_RoleAvaterList[i].f_CheckRole(iRace, iGender))
            {
                return _RoleAvaterList[i];
            }
        }
        return null;
    }
}
