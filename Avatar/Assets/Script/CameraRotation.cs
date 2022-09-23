using UnityEngine;

/// <summary>
/// 攝影機功能
/// </summary>
public class CameraRotation : MonoBehaviour
{
    #region 旋轉攝影機數值
    /// <summary>攝影機旋轉中心</summary>
    private Vector3 _vRotationPoint;
    /// <summary>攝影機座標</summary>
    private Transform _RotationCamera;
    /// <summary>初始旋轉值</summary>
    private Quaternion _InitRotation;
    /// <summary>旋轉速度</summary>
    public float RotateSpeed = 5f;
    /// <summary>攝影機旋轉值</summary>
    private float targetX = 0, targetY = 0;
    /// <summary>目前旋轉值</summary>
    private float curX = 0, curY = 0;
    /// <summary>啟用物件旋轉</summary>
    private bool RotateObjBool = false;
    #endregion

    #region 縮放攝影機數值
    /// <summary>攝影機面向中心</summary>
    private Vector3 _vScalePoint;
    /// <summary>初始位置</summary>
    private Vector3 _InitScale;
    /// <summary>視野縮放速度</summary>
    public float _fScaleSpeed = 10f;
    /// <summary>使用攝影機縮放(移動)功能</summary>
    public bool _UseCameraScale = false;
    #endregion

    /// <summary>重置旋轉速度</summary>
    public float Re_RotateSpeed = 5f;
    /// <summary>是否重置旋轉值</summary>
    private bool ReOriginRot = false;
    /// <summary>重置視野縮放速度</summary>
    public float Re_fScaleSpeed = 10f;
    /// <summary>是否重置縮放值</summary>
    private bool ReOriginScale = false;

    [Space(5)]
    /// <summary>是否開啟平滑功能</summary>
    public bool _LerpChange = false;

    // Start is called before the first frame update
    void Start()
    {
        _RotationCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //重置初始值
        if (ReOriginRot || ReOriginScale)
        {
            f_ChangeRotationPoint();
            f_ChangeScalePoint();
            return;
        }

        //旋轉場景攝影機
        if (Input.GetMouseButtonUp(0)) { RotateObjBool = false; }
        if (RotateObjBool)
        {
            f_RotationCamera();
        }

        //縮放場景視野
        if (_UseCameraScale && !RotateObjBool)
        {
            f_CameraScale(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    #region 設定初始化
    /// <summary>設定旋轉中心</summary>
    public void f_InitRotation(Vector3 Point)
    {
        _vRotationPoint = Point;
    }

    /// <summary>設定旋轉和縮放中心</summary>
    public void f_Init(Vector3 Point, Vector3 PointV2)
    {
        _vRotationPoint = Point;
        _vScalePoint = PointV2;
    }

    /// <summary>設定縮放中心</summary>
    public void f_InitScale(Vector3 Point)
    {
        _vScalePoint = Point;
    }

    /// <summary>重置位置與旋轉</summary>
    public void f_InitPos(Vector3 vNewPos, Quaternion vNewRot)
    {
        if (_LerpChange)//若開啟平滑則進行平滑運動
        {
            f_ReRotationLerp(vNewPos, vNewRot);
            ReOriginScale = _UseCameraScale;
        }
        else//直接移位
        {
            f_ReRotationNotLerp(vNewPos, vNewRot);
            f_InitScale(vNewPos);
        }
    }
    #endregion

    #region 攝影機旋轉功能
    /// <summary>旋轉場景攝影機</summary>
    private void f_RotationCamera()
    {
        targetX = Input.GetAxis("Mouse X");
        targetY = Input.GetAxis("Mouse Y");
        curX = targetX * RotateSpeed;
        curY = targetY * RotateSpeed;
        f_Rotation();
    }

    /// <summary>旋轉</summary>
    private void f_Rotation()
    {
        //依中心進行旋轉
        if (_vRotationPoint != null)
        {
            _RotationCamera.RotateAround(_vRotationPoint, Vector3.up, curX);
            _RotationCamera.RotateAround(_vRotationPoint, _RotationCamera.right, curY);
        }
        else
        {
            _RotationCamera.RotateAround(_RotationCamera.position, Vector3.up, curX);
            _RotationCamera.RotateAround(_RotationCamera.position, _RotationCamera.right, curY);
        }
    }

    /// <summary>
    /// 設置旋轉狀態
    /// </summary>
    /// <param name="bSet">true 啟動,false 關閉</param>
    public void f_SetRotateState(bool bSet)
    {
        RotateObjBool = bSet;
    }

    /// <summary>重置旋轉值</summary>
    private void f_ChangeRotationPoint()
    {
        float _Distance = Vector3.Distance(_InitRotation.eulerAngles, _RotationCamera.eulerAngles);
        if (_Distance < 0.2f)
        {
            ReOriginRot = false;
            return;
        }

        _RotationCamera.rotation = Quaternion.Lerp(_RotationCamera.rotation, _InitRotation, Re_RotateSpeed * Time.deltaTime);
    }

    public void f_ReRotationNotLerp(Vector3 vNewPos, Quaternion vNewRot)
    {
        _RotationCamera.position = vNewPos;
        _RotationCamera.rotation = vNewRot;
    }

    public void f_ReRotationLerp(Vector3 vNewPos, Quaternion vNewRot)
    {
        _InitRotation = vNewRot;
        _InitScale = vNewPos;
        ReOriginRot = true;
    }
    #endregion

    #region 縮放視野
    /// <summary>
    /// 攝影機縮放
    /// </summary>
    /// <param name="fScroll">縮放數值</param>
    private void f_CameraScale(float fScroll)
    {
        if (_vScalePoint != null)//以某一座標為中心點做縮放
        {
            _RotationCamera.Translate(fScroll * _vScalePoint.x, fScroll * _vScalePoint.y, fScroll * _fScaleSpeed);
        }
        else//以視野中心做縮放
        {
            _RotationCamera.Translate(0, 0, fScroll * _fScaleSpeed);
        }
    }

    /// <summary>重置位置</summary>
    private void f_ChangeScalePoint()
    {
        if (!_UseCameraScale) { return; }
        float _Distance = Vector3.Distance(_InitScale, _RotationCamera.position);
        if (_Distance < 0.2f)
        {
            ReOriginScale = false;
            f_InitScale(_InitScale);
            return;
        }

        _RotationCamera.position = Vector3.Lerp(_RotationCamera.position, _InitScale, Re_fScaleSpeed * Time.deltaTime);
    }
    #endregion
}
