using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NBaseSC
{
    Dictionary<int, NBaseSCDT> _aData = new Dictionary<int, NBaseSCDT>();
    List<NBaseSCDT> _aList = null;

    public string m_strRegDTName;
    private bool _bUserList = false;

    protected void Create(string strRegDTName, bool bUseList = false)
    {
        m_strRegDTName = strRegDTName;
        _bUserList = bUseList;
        if (_bUserList)
        {
            _aList = new List<NBaseSCDT>();
        }
    }

    protected void SaveItem(NBaseSCDT DataDT)
    {
        _aData.Add(DataDT.iId, DataDT);
        if (_bUserList)
        {
            _aList.Add(DataDT);
        }
    }


    public NBaseSCDT f_GetSC(int iId)
    {
        NBaseSCDT tNBaseSCDT = null;
        if (_aData.TryGetValue(iId, out tNBaseSCDT))
        {
            return tNBaseSCDT;
        }
        MessageBox.ASSERT(m_strRegDTName + " 请求的SC脚本未找到 " + iId);
        return null;
    }


    public List<NBaseSCDT> f_GetAll()
    {
        return _aList;
    }

    public abstract void f_LoadSCForData(string strData);
}