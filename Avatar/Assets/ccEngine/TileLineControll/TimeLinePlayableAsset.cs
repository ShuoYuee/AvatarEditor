using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

[System.Serializable]
public class TimeLinePlayableAsset : PlayableAsset
{
    public string m_strMessage;

    private GameObject _ParentGo;
    private TimeLinePlayableBehaviour _TimeLinePlayableBehaviour;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        _ParentGo = go;

        var tPlayable = ScriptPlayable<TimeLinePlayableBehaviour>.Create(graph);
        _TimeLinePlayableBehaviour = tPlayable.GetBehaviour();
        f_RegCompleteCallBack();
        return tPlayable;        // Playable.Create(graph);
    }
    
    public void f_RegCompleteCallBack()
    {
        if (_TimeLinePlayableBehaviour != null)
        {
            _TimeLinePlayableBehaviour.f_RegCompleteCallBack(OnCompleteCalllback);
        }
    }

    private void OnCompleteCalllback(object Obj)
    {
        _ParentGo.SendMessage("OnTimeLineMessage", m_strMessage);
    }


}
