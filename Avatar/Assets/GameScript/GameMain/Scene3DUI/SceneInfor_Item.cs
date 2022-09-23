using System.Collections.Generic;
using UnityEngine;

public class SceneInfor_Item : MonoBehaviour
{
    public Transform _InitPos;
    public List<GameObject> _MapObjectList;

    public GameObject f_GetGameObject(string strName)
    {
        return GameTools.f_GetGameObject(_MapObjectList, strName);
    }
}
