using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;
using UnityEngine.UI;

public class ListPositionCtrlTools
{

    #region ListItem
    /// <summary>
    /// 創建圖示
    /// </summary>
    /// <param name="tListPositionCtrl">圖示控制器</param>
    /// <param name="aData">圖示資料庫</param>
    public static void f_Create(ListPositionCtrl tListPositionCtrl, List<NBaseSCDT> aData)
    {
        f_ClearItem(tListPositionCtrl);
        List<ListItem> aList = new List<ListItem>();
        ccMathEx.f_CreateChild(tListPositionCtrl.gameObject, aData.Count);

        for (int i = 0; i < aData.Count; i++)
        {
            Sprite tSprite = null;
            string strGirdNum = "";

            ListItem tListItem = f_AddItem(tListPositionCtrl, i, aData[i], i.ToString(), tSprite, strGirdNum);
            aList.Add(tListItem);
        }

        tListPositionCtrl.listBoxes = aList.ToArray();
    }

    /// <summary>
    /// 增加圖示資料
    /// </summary>
    /// <param name="tListPositionCtrl">圖示控制器</param>
    /// <param name="iIndex">圖示Index</param>
    /// <param name="tData">圖示資料</param>
    /// <param name="strName">圖示名稱</param>
    /// <param name="tSprite">圖片</param>
    /// <param name="strNum"></param>
    /// <returns></returns>
    static ListItem f_AddItem(ListPositionCtrl tListPositionCtrl, int iIndex, NBaseSCDT tData, string strName, Sprite tSprite, string strNum)
    {
        GameObject Obj = tListPositionCtrl.transform.GetChild(iIndex).gameObject;
        ListItem tListItem = Obj.GetComponent<ListItem>();
        if (tListItem == null)
        {
            tListItem = Obj.AddComponent<ListItem>();
        }
        Obj.name = tData.iId + "-" + strName;

        return tListItem;
    }

    public static void f_ClearItem(ListPositionCtrl tListPositionCtrl)
    {
        tListPositionCtrl.listBoxes = new ListBoxBase[0];
        ccMathEx.f_CreateChild(tListPositionCtrl.gameObject, 1);
        List<ListItem> tListItem = new List<ListItem>();
        tListItem.Add(tListPositionCtrl.transform.GetChild(0).GetComponent<ListItem>());
        tListPositionCtrl.listBoxes = tListItem.ToArray();
    }
    #endregion

}