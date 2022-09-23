using UnityEngine;

public class SceneInfor_3D : MonoBehaviour
{
    public string _MapName = "GameMap";
    public GameObject _MapObject;
    public Vector3 _InitPos = Vector3.zero;
    public Vector3 _InitRot = Vector3.zero;
    public Vector3 _InitScale = Vector3.zero;

    [Space(10)]
    public bool _UseInitPos = false;
    public bool _UseInitRot = false;
    public bool _UseInitScale = false;

    private GameObject _oCurMap = null;
    private GameObject _oCurMapObj = null;
    private SceneInfor_Item _oCurItem = null;

    public void f_OpenMap()
    {
        if (_oCurMap != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMap);
        }
        _oCurMap = Instantiate(_MapObject);
        _oCurMap.name = _MapName;
        if (_InitPos != null)
        {
            if (_UseInitPos)
            {
                _oCurMap.transform.position = _InitPos;
            }
            if (_UseInitRot)
            {
                _oCurMap.transform.rotation = Quaternion.Euler(_InitRot);
            }
            if (_UseInitScale)
            {
                _oCurMap.transform.localScale = _InitScale;
            }
        }

        SceneInfor_Item Item = _oCurMap.GetComponentInChildren<SceneInfor_Item>();
        if (Item != null)
        {
            _oCurItem = Item;
        }
    }

    public void f_CloseMap()
    {
        if (_oCurMap != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMap);
        }
        if (_oCurItem != null)
        {
            _oCurItem = null;
        }
    }

    public void f_OpenMainObj(GameObject oObj, bool PosZero = false)
    {
        if (_oCurMapObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMapObj);
        }
        _oCurMapObj = oObj;
        if (_oCurItem != null)
        {
            _oCurMapObj.transform.SetParent(_oCurItem._InitPos);
        }
        else if (_oCurMap != null)
        {
            _oCurMapObj.transform.SetParent(_oCurMap.transform);
        }
        if (PosZero)
        {
            _oCurMapObj.transform.localPosition = Vector3.zero;
            _oCurMapObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _oCurMapObj.transform.localScale = Vector3.one;
        }
        //TTT();
    }

    /// <summary>
    /// 简单效能测试代码
    /// </summary>
    private void TTT()
    {
        _oCurMapObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //0.1481
        float fX = 0;
        float fY = 0;
        int iIndex = 0;
        for (int i = 0; i < 21; i++)
        {
            GameObject tObj = GameObject.Instantiate(_oCurMapObj);
            _oCurMapObj.transform.localPosition = new Vector3(fX, fY, 0);

            fX += 0.2f;
            iIndex++;
            if (iIndex == 5)
            {
                fY = fY + 0.2f;
                fX = 0;
                iIndex = 0;
            }
        }

    }

    public void f_OpenMainObj(GameObject oObj, Vector3 vPos)
    {
        if (_oCurMapObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMapObj);
        }
        _oCurMapObj = oObj;
        if (_oCurMap != null)
        {
            _oCurMapObj.transform.SetParent(_oCurMap.transform);
        }
        _oCurMapObj.transform.position = vPos;
    }

    public void f_OpenMainObj(GameObject oObj, Vector3 vPos, Quaternion vRot)
    {
        if (_oCurMapObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMapObj);
        }
        _oCurMapObj = oObj;
        if (_oCurMap != null)
        {
            _oCurMapObj.transform.SetParent(_oCurMap.transform);
        }
        _oCurMapObj.transform.SetPositionAndRotation(vPos, vRot);
    }

    public void f_OpenMainObj(GameObject oObj, Vector3 vPos, Quaternion vRot, Vector3 vScale)
    {
        if (_oCurMapObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMapObj);
        }
        _oCurMapObj = oObj;
        if (_oCurMap != null)
        {
            _oCurMapObj.transform.SetParent(_oCurMap.transform);
        }
        _oCurMapObj.transform.SetPositionAndRotation(vPos, vRot);
        _oCurMapObj.transform.localScale = vScale;
    }

    public void f_OpenMainObj(GameObject oObj, Vector3 vPos, Quaternion vRot, Vector3 vScale, Transform tParent)
    {
        if (_oCurMapObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMapObj);
        }
        _oCurMapObj = oObj;
        _oCurMapObj.transform.SetParent(tParent);
        _oCurMapObj.transform.SetPositionAndRotation(vPos, vRot);
        _oCurMapObj.transform.localScale = vScale;
    }

    public void f_OpenMainObj(GameObject oObj, Transform tParent, bool PosZero = false)
    {
        if (_oCurMapObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMapObj);
        }
        _oCurMapObj = oObj;
        _oCurMapObj.transform.SetParent(tParent);
        if (PosZero)
        {
            _oCurMapObj.transform.localPosition = Vector3.zero;
            _oCurMapObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _oCurMapObj.transform.localScale = Vector3.one;
        }
    }

    public void f_CloseMainObj()
    {
        if (_oCurMapObj != null)
        {
            glo_Main.GetInstance().f_DoDestroyObj(_oCurMapObj);
        }
    }

    public GameObject f_GetGameObj(string strName)
    {
        if (_oCurItem != null)
        {
            return _oCurItem.f_GetGameObject(strName);
        }
        return null;
    }
}
