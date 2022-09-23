using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ccU3DEngine;

public class ScrollLampText : MonoBehaviour
{
    #region enum
    public EM_ScrollState _ScrollState = EM_ScrollState.ScrollContect;
    public enum EM_ScrollState
    {
        InforRemove = 1,
        ScrollContect = 2,
    }

    public EM_LampDistance _LampDistance = EM_LampDistance.TextLenght;
    public enum EM_LampDistance
    {
        Value = 1,
        TextLenght = 2,
    }

    public EM_Direction _Direction = EM_Direction.Left_to_Right;
    public enum EM_Direction
    {
        Left_to_Right = 1,
        Right_to_Left = 2,
        Top_to_Bottom = 3,
        Bottom_to_Top = 4,
    }
    #endregion

    /// <summary>跑馬燈Text</summary>
    public Text _PmdText;
    private bool _bInit = false;
    private Vector2 _vOriginPivot = Vector2.zero;
    private Vector2 _vOriginAnchorMax = Vector2.zero, _vOriginAnchorMin = Vector2.zero;

    private void Start()
    {
        f_Init();
    }

    public void f_Init()
    {
        if (_PmdText == null) { _PmdText = gameObject.GetComponentInChildren<Text>(); }
        if (_PmdText == null) { return; }
        _strInfor = _PmdText.text;
        _iOriginLoop = _iLoop;
        _vOriginPivot = _PmdText.rectTransform.pivot;
        _vOriginAnchorMax = _PmdText.rectTransform.anchorMax;
        _vOriginAnchorMin = _PmdText.rectTransform.anchorMin;

        ccUIEventListener.Get(_PmdText.gameObject).f_RegHover(OnHoverBtn_Text, null);
        ccUIEventListener.Get(_PmdText.gameObject).f_RegClick(OnClickBtn_Text, null, null, "");
        f_InitComponent();
        _bIsEnd = false;
        _bInit = true;
    }


    private void OnClickBtn_Text(GameObject go, object obj1, object obj2)
    {
        f_Revert();
        f_InitComponent();
    }

    private void OnHoverBtn_Text(GameObject go, object obj1, object obj2)
    {
        bool bHover = (bool)obj1;
        if (!_bInit)
        {
            return;
        }
        if (bHover)
        {
            if (_bIsEnd) { return; }
            f_StartText();
        }
        else
        {
            f_Revert();
            f_InitComponent();
        }
    }

    public void f_InitComponent()
    {
        _iLoop = _iOriginLoop;
        if (_Direction == EM_Direction.Left_to_Right || _Direction == EM_Direction.Right_to_Left)
        {
            if(_LampDistance == EM_LampDistance.Value && f_CheckTextInfor())
            {
                _PmdText.text = _strInfor;
                if (_Direction == EM_Direction.Left_to_Right)
                {
                    _PmdText.rectTransform.pivot = new Vector2(0, _vOriginPivot.y);
                    _PmdText.rectTransform.anchorMax = new Vector2(0, _vOriginAnchorMax.y);
                    _PmdText.rectTransform.anchorMin = new Vector2(0, _vOriginAnchorMin.y);
                }
                else if (_Direction == EM_Direction.Right_to_Left)
                {
                    _PmdText.rectTransform.pivot = new Vector2(1, _vOriginPivot.y);
                    _PmdText.rectTransform.anchorMax = new Vector2(1, _vOriginAnchorMax.y);
                    _PmdText.rectTransform.anchorMin = new Vector2(1, _vOriginAnchorMin.y);
                }
            }
            else if (_LampDistance == EM_LampDistance.TextLenght && f_CheckTextScroll())
            {
                _fOriginPos = _PmdText.rectTransform.anchoredPosition.x;
                if (_Direction == EM_Direction.Left_to_Right)
                {
                    _PmdText.rectTransform.pivot = new Vector2(0, _vOriginPivot.y);
                    _PmdText.rectTransform.anchorMax = new Vector2(0, _vOriginAnchorMax.y);
                    _PmdText.rectTransform.anchorMin = new Vector2(0, _vOriginAnchorMin.y);
                }
                else if (_Direction == EM_Direction.Right_to_Left)
                {
                    _PmdText.rectTransform.pivot = new Vector2(1, _vOriginPivot.y);
                    _PmdText.rectTransform.anchorMax = new Vector2(1, _vOriginAnchorMax.y);
                    _PmdText.rectTransform.anchorMin = new Vector2(1, _vOriginAnchorMin.y);
                }
            }
            else
            {
                _PmdText.rectTransform.pivot = _vOriginPivot;
                _PmdText.rectTransform.anchorMax = _vOriginAnchorMax;
                _PmdText.rectTransform.anchorMin = _vOriginAnchorMin;
            }
        }
        else if (_Direction == EM_Direction.Top_to_Bottom || _Direction == EM_Direction.Bottom_to_Top)
        {
            if (_LampDistance == EM_LampDistance.Value && f_CheckTextInfor())
            {
                _PmdText.text = _strInfor;
                if (_Direction == EM_Direction.Top_to_Bottom)
                {
                    _PmdText.rectTransform.pivot = new Vector2(_vOriginPivot.x, 1);
                    _PmdText.rectTransform.anchorMax = new Vector2(_vOriginAnchorMax.x, 1);
                    _PmdText.rectTransform.anchorMin = new Vector2(_vOriginAnchorMin.x, 1);
                }
                else if (_Direction == EM_Direction.Bottom_to_Top)
                {
                    _PmdText.rectTransform.pivot = new Vector2(_vOriginPivot.x, 0);
                    _PmdText.rectTransform.anchorMax = new Vector2(_vOriginAnchorMax.x, 0);
                    _PmdText.rectTransform.anchorMin = new Vector2(_vOriginAnchorMin.x, 0);
                }
            }
            else if (_LampDistance == EM_LampDistance.TextLenght && f_CheckTextScroll())
            {
                _fOriginPos = _PmdText.rectTransform.anchoredPosition.y;
                if (_Direction == EM_Direction.Top_to_Bottom)
                {
                    _PmdText.rectTransform.pivot = new Vector2(_vOriginPivot.x, 1);
                    _PmdText.rectTransform.anchorMax = new Vector2(_vOriginAnchorMax.x, 1);
                    _PmdText.rectTransform.anchorMin = new Vector2(_vOriginAnchorMin.x, 1);
                }
                else if (_Direction == EM_Direction.Bottom_to_Top)
                {
                    _PmdText.rectTransform.pivot = new Vector2(_vOriginPivot.x, 0);
                    _PmdText.rectTransform.anchorMax = new Vector2(_vOriginAnchorMax.x, 0);
                    _PmdText.rectTransform.anchorMin = new Vector2(_vOriginAnchorMin.x, 0);
                }
            }
            else
            {
                _PmdText.rectTransform.pivot = _vOriginPivot;
                _PmdText.rectTransform.anchorMax = _vOriginAnchorMax;
                _PmdText.rectTransform.anchorMin = _vOriginAnchorMin;
            }
        }
        _PmdText.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void f_StartText()
    {
        if (_PmdText == null) { return; }
        if (_bIsEnd) { return; }
        if (_ScrollState == EM_ScrollState.InforRemove)
        {
            if (f_CheckTextInfor())
            {
                _bIsEnd = true;
                _iTimeEvent = ccTimeEvent.GetInstance().f_RegEvent(_fSpeed, true, null, f_ScrollText);
            }
        }
        else if (_ScrollState == EM_ScrollState.ScrollContect)
        {
            if (f_CheckTextScroll())
            {
                _MsgQueue = new Queue<string>();
                f_AddMessage();
            }
        }
    }

    public void f_Revert()
    {
        f_Stop();
        _bIsEnd = false;
    }

    public void f_Stop()
    {
        _iSubText = 0;
        _MsgQueue = new Queue<string>();
        if (_iTimeEvent != -99)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(_iTimeEvent);
        }
        StopCoroutine(f_ScrollingH());
        StopCoroutine(f_ScrollingV());
        _PmdText.rectTransform.DOKill();
    }

    #region 以去除字元的方式遊覽
    [Space(10)]
    /// <summary>跑馬燈時長</summary>
    public float _fSpeedTime = 1;
    /// <summary>是否依照跑馬燈時長</summary>
    public bool _UseSpeedTime = false;

    private int _iSubText = 0;
    /// <summary>判斷當前跑馬燈是否完成</summary>
    private bool _bIsEnd = false;
    /// <summary>跑馬燈速度</summary>
    private float _fSpeed = 0.05f;
    /// <summary>時間事件ID</summary>
    private int _iTimeEvent = -99;
    /// <summary>原始內容</summary>
    private string _strInfor;
    /// <summary>欲移動的文字數量</summary>

    private void f_ScrollText(object obj)
    {
        if (_iSubText > 0)
        {
            if (_Direction == EM_Direction.Left_to_Right)
            {
                _PmdText.text = _PmdText.text.Remove(0, 1);
            }
            else if (_Direction == EM_Direction.Right_to_Left)
            {
                _PmdText.text = _PmdText.text.Remove(_PmdText.text.Length - 1, 1);
            }
            else if (_Direction == EM_Direction.Top_to_Bottom)
            {
                string[] strInfor = ccMath.f_String2ArrayString(_PmdText.text, "\n");
                _PmdText.text = _PmdText.text.Replace(strInfor[0], "");
            }
            else if (_Direction == EM_Direction.Bottom_to_Top)
            {
                string[] strInfor = ccMath.f_String2ArrayString(_PmdText.text, "\n");
                _PmdText.text = _PmdText.text.Replace(strInfor[_PmdText.text.Length - 1], "");
            }
            _iSubText--;
        }
        else
        {
            f_Stop();
        }
    }

    public bool f_CheckTextInfor()
    {
        TextGenerator tGenerator = new TextGenerator();
        RectTransform tRect = _PmdText.rectTransform;
        TextGenerationSettings tSettings = _PmdText.GetGenerationSettings(tRect.rect.size);
        tGenerator.Populate(_PmdText.text, tSettings);
        if (_Direction == EM_Direction.Left_to_Right || _Direction == EM_Direction.Right_to_Left)
        {
            if (_PmdText.text.Length > tGenerator.characterCountVisible)
            {
                //string strInfor = m_Text.text.Substring(0, tGenerator.characterCountVisible - 3);
                //strInfor += "......";
                //m_Text.text = strInfor;
                _strInfor = _PmdText.text;
                _iSubText = _PmdText.text.Length - tGenerator.characterCountVisible;
                if (_UseSpeedTime)
                {
                    _fSpeed = _fSpeedTime / _iSubText;
                }
                else
                {
                    _fSpeed = 0.05f;
                }
                return true;
            }
            else
            {
                _iSubText = 0;
                return false;
            }
        }
        else if (_Direction == EM_Direction.Top_to_Bottom || _Direction == EM_Direction.Bottom_to_Top)
        {
            int iRow = ccMath.f_String2ArrayString(_PmdText.text, "\n").Length;
            if (iRow > tGenerator.lineCount)
            {
                _strInfor = _PmdText.text;
                _iSubText = iRow - tGenerator.lineCount;
                if (_UseSpeedTime)
                {
                    _fSpeed = _fSpeedTime / _iSubText;
                }
                else
                {
                    _fSpeed = 0.05f;
                }
                return true;
            }
            else
            {
                _iSubText = 0;
                return false;
            }
        }
        return false;
    }

    public bool f_CheckTextScroll()
    {
        if (_Direction == EM_Direction.Left_to_Right || _Direction == EM_Direction.Right_to_Left)
        {
            if (_PmdText.rectTransform.sizeDelta.x > gameObject.GetComponent<RectTransform>().sizeDelta.x)
            {
                return true;
            }
        }
        else if (_Direction == EM_Direction.Top_to_Bottom || _Direction == EM_Direction.Bottom_to_Top)
        {
            if (_PmdText.rectTransform.sizeDelta.y > gameObject.GetComponent<RectTransform>().sizeDelta.y)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region 跑馬燈
    [Space(10)]
    public float _fDuration = 10f;
    public float _fScrollSpeed = 200f;
    public float _fLampDis = -20;
    public int _iLoop = 1;

    private Queue<string> _MsgQueue;
    private float _fOriginPos = 0;
    private int _iOriginLoop = 1;

    /// <summary>
    /// 添加跑馬燈信息.
    /// </summary>
    /// <param name="msg"></param>
    public void f_AddMessage()
    {
        if (!_PmdText.gameObject.activeSelf)
        {
            _PmdText.gameObject.SetActive(true);
            _MsgQueue = new Queue<string>();
        }
        _MsgQueue.Enqueue(_PmdText.text);
        if (_bIsEnd) return;
        if (_Direction == EM_Direction.Left_to_Right || _Direction == EM_Direction.Right_to_Left)
        {
            StartCoroutine(f_ScrollingH());
        }
        else if (_Direction == EM_Direction.Top_to_Bottom || _Direction == EM_Direction.Bottom_to_Top)
        {
            StartCoroutine(f_ScrollingV());
        }
    }

    public IEnumerator f_ScrollingH()
    {
        float fBeginPos = _PmdText.rectTransform.anchoredPosition.x;
        float fNewPos = f_GetScrollDistance();
        while (_MsgQueue.Count > 0)
        {
            string strMsg = _MsgQueue.Dequeue();
            _PmdText.text = strMsg;
            float fTxtWidth = _PmdText.preferredWidth;//文本自身的長度.
            Vector3 pos = _PmdText.rectTransform.localPosition;
            float fDistance = fBeginPos - fNewPos + fTxtWidth;
            _fDuration = fDistance / _fScrollSpeed;
            _bIsEnd = true;
            while (_iLoop-- > 0)
            {
                if (!_bIsEnd || !_bInit)
                {
                    _iLoop = 0;
                    yield break;
                }
                _PmdText.rectTransform.localPosition = new Vector3(fBeginPos, pos.y, pos.z);
                if (_Direction == EM_Direction.Left_to_Right)
                {
                    _PmdText.rectTransform.DOLocalMoveX(-fDistance, _fDuration).SetEase(Ease.Linear);
                }
                else if (_Direction == EM_Direction.Right_to_Left)
                {
                    _PmdText.rectTransform.DOLocalMoveX(fDistance, _fDuration).SetEase(Ease.Linear);
                }
                yield return new WaitForSeconds(_fDuration);
            }
            yield return null;
        }
        _bIsEnd = false;
        yield break;
    }

    public IEnumerator f_ScrollingV()
    {
        float fBeginPos = _PmdText.rectTransform.anchoredPosition.y;
        float fNewPos = f_GetScrollDistance();
        while (_MsgQueue.Count > 0)
        {
            string strMsg = _MsgQueue.Dequeue();
            _PmdText.text = strMsg;
            float fTxtHeight = _PmdText.preferredHeight;//文本自身的長度.
            Vector3 pos = _PmdText.rectTransform.localPosition;
            float fDistance = fBeginPos - fNewPos + fTxtHeight;
            _fDuration = fDistance / _fScrollSpeed;
            _bIsEnd = true;
            while (_iLoop-- > 0)
            {
                _PmdText.rectTransform.localPosition = new Vector3(pos.x, fBeginPos, pos.z);
                if (_Direction == EM_Direction.Top_to_Bottom)
                {
                    _PmdText.rectTransform.DOLocalMoveY(-fDistance, _fDuration).SetEase(Ease.Linear);
                }
                else if (_Direction == EM_Direction.Bottom_to_Top)
                {
                    _PmdText.rectTransform.DOLocalMoveY(fDistance, _fDuration).SetEase(Ease.Linear);
                }
                yield return new WaitForSeconds(_fDuration);
            }
            yield return null;
        }
        _bIsEnd = false;
        yield break;
    }

    private float f_GetScrollDistance()
    {
        if (_LampDistance == EM_LampDistance.Value)
        {
            return _fLampDis;
        }
        else if (_LampDistance == EM_LampDistance.TextLenght)
        {
            TextGenerator tGenerator = _PmdText.cachedTextGeneratorForLayout;
            TextGenerationSettings tSetting = _PmdText.GetGenerationSettings(Vector2.zero);
            float fWidth = 0;
            if (_Direction == EM_Direction.Left_to_Right)
            {
                fWidth = tGenerator.GetPreferredWidth(_PmdText.text, tSetting) / _PmdText.pixelsPerUnit;
                return fWidth = fWidth + _fLampDis - (_PmdText.rectTransform.sizeDelta.x / 2);
            }
            else if (_Direction == EM_Direction.Right_to_Left)
            {
                fWidth = tGenerator.GetPreferredWidth(_PmdText.text, tSetting) / _PmdText.pixelsPerUnit;
                return fWidth = fWidth + _fLampDis - (_PmdText.rectTransform.sizeDelta.x / 2);
            }
            else if (_Direction == EM_Direction.Top_to_Bottom)
            {
                fWidth = tGenerator.GetPreferredHeight(_PmdText.text, tSetting) / _PmdText.pixelsPerUnit;
                return fWidth = fWidth + _fLampDis + (_PmdText.rectTransform.sizeDelta.y / 2);
            }
            else if (_Direction == EM_Direction.Bottom_to_Top)
            {
                fWidth = tGenerator.GetPreferredHeight(_PmdText.text, tSetting) / _PmdText.pixelsPerUnit;
                return fWidth = fWidth + _fLampDis + (_PmdText.rectTransform.sizeDelta.y / 2);
            }
            else
            {
                fWidth = tGenerator.GetPreferredWidth(_PmdText.text, tSetting) / _PmdText.pixelsPerUnit;
                return fWidth = fWidth + _fLampDis - (_PmdText.rectTransform.sizeDelta.x / 2);
            }
        }
        return _fLampDis;
    }
    #endregion
}
