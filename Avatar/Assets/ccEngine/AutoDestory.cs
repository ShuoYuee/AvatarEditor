using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    public float m_fTime = 2;

    // Use this for initialization
    void Start() {
        StartCoroutine(DestoryObject());
    }


    IEnumerator DestoryObject() {
        yield return new WaitForSeconds(m_fTime);
        Destroy(this.gameObject);
    }

}
