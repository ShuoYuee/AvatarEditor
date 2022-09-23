using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 編輯資料管理器
/// </summary>
public class DressEditorManager : MonoBehaviour
{
    private static DressEditorManager _Instance = null;
    public static DressEditorManager GetInstance()
    {
        return _Instance;
    }

    private void Start()
    {
        _Instance = this;

#if UNITY_EDITOR
        if (_MainCamera == null)
        {
            _MainCamera = Camera.main;
        }
        List<int> _CheckInt = new List<int>();
        for (int i = 0; i < _AvatarData.Count; i++)
        {
            if (_AvatarData[i]._EditorAction == DressEditorData.EM_EditorAction.Avatar && _CheckInt.Contains(_AvatarData[i]._ID))
            {
                MessageBox.ASSERT("時裝資料ID有重複：" + _AvatarData[i]._ID + "_" + _AvatarData[i]._AvatarName + "，這將會影響儲存！");
                break;
            }
            _CheckInt.Add(_AvatarData[i]._ID);
        }
        DressEditorPoolDT.f_SetRoleData((int)GameEM.EM_Gender.Female, (int)GameEM.EM_Race.Asian, 1);
#endif
    }

#if UNITY_EDITOR
    [Tooltip("將時裝、動作、鏡頭等Data皆置入")]
    public List<DressEditorData> _AvatarData;

    [Tooltip("將人種Data置入")]
    public List<DressEditorData> _RoleData;

    //[Space(10)]
    //[Rename("初始化資料")]
    //[Tooltip("作為初始化的角色資料")]
    //public DressEditorData _InitRoleData;

    public int _AvatarCount { get { return _AvatarData.Count; } }
    public int _RoleCount { get { return _RoleData.Count; } }

    #region 時裝相關
    /// <summary>換裝(編輯器)</summary>
    public void f_ChangeAvatar(DressEditorData data, DressRoleControl _RoleControl)
    {
        _RoleControl.CallBack_SetSmrMesh(data);
    }

    /// <summary>卸下時裝(編輯器)</summary>
    /// <param name="iId">時裝ID</param>
    public void f_TakeOff(DressEditorData data, DressRoleControl _RoleControl)
    {
        if (data._AvatarData._TakeOffData != null)
        {
            f_ChangeAvatar(data._AvatarData._TakeOffData, _RoleControl);
        }
        else
        {
            _RoleControl.f_TakeOff(data);
        }
    }

    /// <summary>取得當前Avatar</summary>
    public Avatar f_GetCurAvatar(int iIndex)
    {//根據角色性別和人種、取得對應Index的Avatar(換身高用)
        DressEditorData tData = _RoleData.Find(delegate (DressEditorData p)
        {
            if ((int)p._Race._RoleAvatar._RoleGender == DressEditorPoolDT._iGender && (int)p._Race._RoleAvatar._RoleRace == DressEditorPoolDT._iRace) { return true; }
            return false;
        });

        if (DressEditorPoolDT._iMesh == 0)
        {
            return tData._Race._RoleAvatar._LowModel[iIndex];
        }
        else
        {
            return tData._Race._RoleAvatar._HightModel[iIndex];
        }
    }
    #endregion

    #region 場景預覽
    [Space(10)]
    [Rename("主場景物件(預覽場景時將關掉該物件)")]
    public GameObject _oLightObj = null;
    [Rename("主場景攝影機")]
    public Camera _MainCamera = null;

    /// <summary>當前預覽物件</summary>
    private GameObject oLightScene = null;
    /// <summary>當前預覽場景</summary>
    private string strLightScene = "";
    /// <summary>開啟預覽</summary>
    public void f_ChangeSceneLight(DressEditorData tData)
    {
        GameTools.f_SetGameObject(_MainCamera.gameObject, false);
        GameTools.f_SetGameObject(_oLightObj, false);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SceneView, false);

        if (_oLightObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(oLightScene);
            oLightScene = null;
        }
        if (strLightScene != "")
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(strLightScene);
            strLightScene = "";
        }
        if (tData._SceneData._EditorSecene == Editor_Scene.EM_EditorSecene.LoadLight)//生成預覽物件
        {
            oLightScene = Instantiate(tData._SceneData._oSceneModel, GameMain.GetInstance().gameObject.transform);
        }
        else//載入預覽場景
        {
            strLightScene = tData._SceneData._strScene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(tData._SceneData._strScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    /// <summary>關閉預覽</summary>
    public void f_CallBack_Load()
    {
        if (oLightScene)//銷毀預覽物件
        {
            glo_Main.GetInstance().f_DoDestroyObj(oLightScene);
            oLightScene = null;
        }
        else//關閉預覽場景
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(strLightScene);
            strLightScene = "";
        }
        GameTools.f_SetGameObject(_MainCamera.gameObject, true);
        GameTools.f_SetGameObject(_oLightObj, true);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SceneView, true);
    }
    #endregion

    #region 應用功能
    [Header("應用功能")]
    [Space(10)]

    [Rename("刪除\"時裝資料\"指定資料")]
    [Tooltip("輸入資料名字後，點擊\"刪除指定名稱的資料\"進行刪除")]
    public string _DelAvatarData = "";

    [Rename("刪除\"人種資料\"指定資料")]
    [Tooltip("輸入資料名字後，點擊\"刪除指定名稱的資料\"進行刪除")]
    public string _DelRaceData = "";

    [ContextMenu("重新整理時裝清單")]
    public void f_ResetList()
    {
        for (int i = 0; i < _AvatarData.Count; i++)
        {
            if (_AvatarData[i] == null)
            {
                _AvatarData.Remove(_AvatarData[i]);
                i -= 1;
            }
        }
        for (int i = 0; i < _RoleData.Count; i++)
        {
            if (_RoleData[i] == null)
            {
                _RoleData.Remove(_RoleData[i]);
                i -= 1;
            }
        }
    }

    [ContextMenu("刪除\"時裝資料\"指定名稱的資料")]
    public void f_DelAvatarData()
    {
        if (_DelAvatarData != "")
        {
            for (int i = 0; i < _AvatarData.Count; i++)
            {
                if (_AvatarData[i].name == _DelAvatarData)
                {
                    _AvatarData.Remove(_AvatarData[i]);
                    break;
                }
            }
        }
    }

    [ContextMenu("刪除\"人種資料\"指定名稱的資料")]
    public void f_DelRaceData()
    {
        if (_DelRaceData != "")
        {
            for (int i = 0; i < _AvatarData.Count; i++)
            {
                if (_RoleData[i].name == _DelRaceData)
                {
                    _RoleData.Remove(_RoleData[i]);
                    break;
                }
            }
        }
    }
    #endregion
#endif
}