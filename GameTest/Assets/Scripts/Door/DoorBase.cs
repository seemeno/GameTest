using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
namespace Com.MyCompany.MyGame
{
    public class DoorBase : MonoBehaviourPunCallbacks, IPunObservable
    {
        public string DoorName;//门的名称
        public int DoorGUID { get; set; }//门的唯一标识
        public int DoorType;//门的种类
        public bool IsUsed = false; //门是否被使用
        public string DoorDesc;//门的描述

        private void Awake()
        {
        }
        // Start is called before the first frame update
        void Start()
        {

            //GameObject prefab = (GameObject)Resources.Load("prefabs/ObjectIcon");
            //MiniMap.Instance.InitOneObjectIcon(prefab, transform);

        }
        #region PUN Callbacks
        //wanghao:数据信息同步
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(DoorName);
                stream.SendNext(DoorGUID);
                stream.SendNext(DoorType);
                stream.SendNext(IsUsed);
                stream.SendNext(DoorDesc);
            }
            else
            {
                // Network player, receive data
                this.DoorName = (string)stream.ReceiveNext();
                this.DoorGUID = (int)stream.ReceiveNext();
                this.DoorType = (int)stream.ReceiveNext();
                this.IsUsed = (bool)stream.ReceiveNext();
                this.DoorDesc = (string)stream.ReceiveNext();
            }
        }
        #endregion

        public virtual void OnTriggerEnterUse(Collider player) { }

        // Update is called once per frame
        void Update()
        {

        }
        public Vector3 GetForwardPos()
        {
            //获得门前方的位置，可能存在问题
            Debug.Log("doorPosition" + transform.position.ToString());
            return transform.TransformPoint(new Vector3(15, 0, 0));
            //return GetComponent<Transform>().position;
        }


    }
}

