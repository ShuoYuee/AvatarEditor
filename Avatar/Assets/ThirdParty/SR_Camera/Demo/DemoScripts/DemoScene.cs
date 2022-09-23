using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace SR
{
    public class DemoScene : MonoBehaviour
    {
        public Button btn45;
        public Button btn90;
        public Toggle toggle;
        public Button button;

        public Button gudingButton;
        public Button guiweiButton;

        private SR_Camera sr_camera;

        public Text ShowText;
        private float showTime = 3f;
        private float timer = 0;

        private Vector3 originPos;
        private Vector3 originRotate;

        private void Start()
        {
            sr_camera = GameObject.FindObjectOfType<SR_Camera>();
            toggle.onValueChanged.AddListener(isOn =>
            {
                sr_camera.IsFixedXAngle = isOn;
                if (isOn)
                {

                    btn45.gameObject.SetActive(true);
                    btn90.gameObject.SetActive(true);
                }
                else
                {
                    btn45.gameObject.SetActive(false);
                    btn90.gameObject.SetActive(false);
                }
            });
            toggle.isOn = sr_camera.IsFixedXAngle;
            button.onClick.AddListener(() =>
            {
                Vector3 rotate = new Vector3(Random.Range(sr_camera.clampMinXAngle, sr_camera.clampMaxXAngle),
                    Random.Range(0, 360), 0);
                if (sr_camera.IsFixedXAngle)
                {
                    rotate = new Vector3(sr_camera.xAngle,
                        Random.Range(0, 360), 0);
                }
                //随机生成一个位置和旋转
                sr_camera.SetCameraPostionRotation(new Vector3(Random.Range(sr_camera.center.x - sr_camera.limitX, sr_camera.center.x + sr_camera.limitX), Random.Range(sr_camera.minHeight, sr_camera.maxHeight), Random.Range(sr_camera.center.z - sr_camera.limitZ, sr_camera.center.z + sr_camera.limitZ)), rotate);

            });
            btn45.onClick.AddListener(() => SetXRotation(45f));
            btn90.onClick.AddListener(() => SetXRotation(90f));
            sr_camera.SelectTargetAction += go =>
            {
                ShowText.gameObject.SetActive(true);
                ShowText.text = "SelectTarget:" + go.name;
            };
            sr_camera.ResetTargetAction += () =>
            {
                ShowText.gameObject.SetActive(true);
                ShowText.text = "ResetTarget";
            };

            sr_camera.FocusStartAction += go =>
            {
                ShowText.gameObject.SetActive(true);
                ShowText.text = "StartFocus:" + go.name;
            };
            sr_camera.FocusEndAction += go =>
            {
                ShowText.gameObject.SetActive(true);
                ShowText.text = "EndForcus:" + go.name;
            };

            gudingButton.onClick.AddListener(() =>
            {
                originPos = sr_camera.transform.position;
                originRotate = sr_camera.transform.eulerAngles;
                Vector3 pos = new Vector3(-1.145074f, 4.976437f, 30.48049f);
                Vector3 rotate = Vector3.zero;

                sr_camera.SetCameraPostionRotation(pos, rotate);
            });

            guiweiButton.onClick.AddListener(() =>
            {
                sr_camera.SetCameraPostionRotation(originPos, originRotate);
            });
        }

        private void SetXRotation(float angle)
        {
            sr_camera.xAngle = angle;
        }

        void Update()
        {
            if (ShowText.gameObject.activeSelf)
            {
                timer += Time.deltaTime;
                if (timer >= showTime)
                {
                    timer = 0;
                    ShowText.gameObject.SetActive(false);
                }
            }
        }
    }
}

