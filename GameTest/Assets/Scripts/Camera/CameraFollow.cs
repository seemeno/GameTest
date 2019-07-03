using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wanghao
namespace Com.MyCompany.MyGame
{
    public class CameraFollow : MonoBehaviour
    {

        //距离
        public float distance = 30f;
        //横向角度
        public float rot = -190;
        //纵向角度
        public float roll = 30f * Mathf.PI * 2 / 360;
        //目标物体
        private GameObject target;
        //旋转速度
        public float rotSpeed = 0.2f;
        //纵向旋转临界
        private float maxRoll = 50f * Mathf.PI * 2 / 360;
        private float minRoll = 0;
        //滚轮距离临界
        private float maxDistance = 40f;
        private float minDistance = 20f;
        private float zoomSpeed = 1f;

        [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
        [SerializeField]
        private bool followOnStart = false;


        // cached transform of the target
        Transform cameraTransform;


        // maintain a flag internally to reconnect if target is lost or camera is switched
        bool isFollowing;

        void Start()
        {
            // Start following the target if wanted.
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //判断
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }
            // only follow is explicitly declared
            if (isFollowing)
            {
                //摄像头横向旋转
                Rotate();
                //摄像头纵向旋转
                //Roll();
                //滑轮调节距离
                Zoom();
                //摄像头跟随人物移动
                Camera_Move();
            }
        }

        #region Public Methods
        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            Camera_Move();
        }


        #endregion

        public void Camera_Move()
        {
            //目标的坐标
            Vector3 targetPos = transform.position;
            //三角函数计算相机位置
            Vector3 cameraPos;
            float d = distance * Mathf.Cos(roll);
            float height = distance * Mathf.Sin(roll);
            cameraPos.x = targetPos.x + d * Mathf.Cos(rot);
            cameraPos.z = targetPos.z + d * Mathf.Sin(rot);
            cameraPos.y = height;

            cameraTransform.position = cameraPos;

            //对准目标
            cameraTransform.LookAt(transform);
        }

        public void Rotate()
        {
            float w = Input.GetAxis("Mouse X") * rotSpeed;
            rot -= w;
        }

        public void Roll()
        {
            float w = Input.GetAxis("Mouse Y") * rotSpeed;
            roll -= w;
            if (roll > maxRoll)
                roll = maxRoll;
            if (roll < minRoll)
                roll = minRoll;
        }

        public void Zoom()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (distance > minDistance)
                    distance -= zoomSpeed;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (distance < maxDistance)
                    distance += zoomSpeed;
            }
        }
    }
}


