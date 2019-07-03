using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    public class DoorTrigger : MonoBehaviourPun
    {
        //门的触发器
        private float CD = 20;//门冷却时间，该方法还没有加
        private bool IsActive; //当前门是否可用
        private DoorBase CurDoor; //当前绑定门脚本
        void Awake()
        {
        }

        void Start()
        {

            IsActive = true;
            switch (gameObject.GetComponent<DoorBase>().DoorType)
            {
                case (int)DOORTYPE.NORMALDOOR:
                    CurDoor = gameObject.GetComponent<NormalDoor>();
                    break;
                case (int)DOORTYPE.TRANSFERDOOR:
                    CurDoor = gameObject.GetComponent<TransferDoor>();
                    break;
                case (int)DOORTYPE.FINALDOOR:
                    CurDoor = gameObject.GetComponent<FinalDoor>();
                    break;
                default:
                    CurDoor = gameObject.GetComponent<DoorBase>();
                    break;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider coll)
        {
            //进入门时触发
            Debug.Log("OnTriggerEnter");
            if (IsActive && coll.tag == "Player")
            {
                Debug.Log("Door" + CurDoor.DoorGUID.ToString() + "可用");
                TimeMgr.instance.AddTimer("Door" + CurDoor.DoorGUID.ToString(), new TimeCount(CD, null, () =>
                {
                    //计时器终止时调用
                    IsActive = true;
                    TimeMgr.instance.RemoveTimer("Door" + CurDoor.DoorGUID.ToString());
                }, () =>
                {
                    //计时器开始前调用
                    //调用doorbase中的触发函数
                    CurDoor.OnTriggerEnterUse(coll);
                    IsActive = false;
                }));
            }
            else
            {

                Debug.Log("Door" + CurDoor.DoorGUID.ToString() + "不可用");
            }

            
        }
    }
}

