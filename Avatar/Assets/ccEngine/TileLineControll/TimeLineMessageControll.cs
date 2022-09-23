using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineMessageControll : MonoBehaviour
{

    private ccCallback _ccCallback = null;
    public void f_RegCompleteCallBack(ccCallback tccCallback)
    {
        _ccCallback = tccCallback;
    }

    public void OnTimeLineMessage(string strMessage)
    {
        if (_ccCallback != null)
        {
            _ccCallback(strMessage);
        }
        //Debug.Log("OnTimeLineMessage " + strMessage);
    }


}
