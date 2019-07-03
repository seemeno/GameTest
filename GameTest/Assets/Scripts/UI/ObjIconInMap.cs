using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    public class ObjIconInMap : MonoBehaviourPunCallbacks
    {
        //wanghao
        public GameObject ObjIcon; //绑定对象
        RectTransform IconRT;
        RectTransform MiniMapRT;
        public static float XRatio;//X轴比例
        public static float YRatio;//Z轴比例
        float timeCount = 0;
        // 使用进行初始化
        void Start()
        {
            IconRT = GetComponent<RectTransform>();
            MiniMapRT = transform.parent.GetComponent<RectTransform>();
            //playerIcon = GameObject.Find("Player").transform;
        }

        //每一帧都会调用该函数
        void Update()
        {
            //timeCount += Time.deltaTime;
            //if (timeCount > 0.5f)
            //{
            //    timeCount = 0;
            
            if (ObjIcon.transform.localScale.x != 0)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 1;
                gameObject.GetComponent<CanvasGroup>().interactable = true;
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                UpdatePos();
            }
            else
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 0;
                gameObject.GetComponent<CanvasGroup>().interactable = false;
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            //}
            //float y = playerIcon.eulerAngles.y;
            //Debug.LogError("y" + y);
            //transform.eulerAngles = new Vector3(0, 0, -y);
            //transform.Translate(new Vector3(playerIcon.position.x, playerIcon.position.y, 0));
        }


        void UpdatePos()
        {
            float widthRate = ObjIcon.transform.position.x / Scene.Instance.MapWidth;
            float heightRate = ObjIcon.transform.position.z / Scene.Instance.MapLength;

            Vector2 tmpPos = Vector2.zero;
            tmpPos.x = MiniMapRT.sizeDelta.x * widthRate;
            tmpPos.y = MiniMapRT.sizeDelta.y * heightRate;
            IconRT.localPosition = tmpPos;
            //Vector3 tmpAngle = ObjIcon.localEulerAngles;
            //tmpAngle.z = 90 - ObjIcon.localEulerAngles.y;
            //IconRT.localEulerAngles = tmpAngle;
        }
    }
}

