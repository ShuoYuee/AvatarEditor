using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    /// <summary>
    /// parameter configuration
    /// </summary>
    public class ParameterConfiguration : MonoBehaviour
    {


        public int lastTab=0;

        #region PCConfig
        [Header("Scaling of strength")]
        public float scrollWheelZoomingSensitivity;
        [Header("The rotation speed of mouse")]
        public float mouseRotationSpeed = 200f;
        #endregion

        #region MobileConfig
        [Header("Mobile Zoom of strength")]
        public  float MobileZoomingSensitivity=20f;
        [Header("Mobile Rotate of Speed")]
        public float MobileRotationSpeed = 5f;


        #endregion

        [Header("Maximum distance to target")]
        public float maxDistance = 100f;

        [Header("Minimum distance to target")]
        public float minDistance = 2f;

        [Header("Select the current distance of the object/set to 0 default middle position")] public float targetDistance = 0;

        [Header("Adjust the camera focus weight based on the target size")]
        public float targetWeight = 2;

     
        [Header("Set default Rotate")]
        [HideInInspector]
        public bool isSettingRotate = false;
        [Header("Rotate Value")][HideInInspector]
        public Vector3 currentRotate = Vector3.zero;



        // Start is called before the first frame update
        void Start()
        {
            if (scrollWheelZoomingSensitivity == 0)
            {
                scrollWheelZoomingSensitivity = (maxDistance - minDistance) / 2;
            }

            if (targetDistance == 0)
            {
                targetDistance = (maxDistance - minDistance) / 2;
            }

        }


        private void Record()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
