using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class FinalDoor : DoorBase
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnTriggerEnterUse(Collider player)
        {
            //先获得道具，再转换角色，只有当前角色为人能够获得对应道具
            //大鬼门触发事件
            Debug.Log("FinalDoor : OnTriggerEnterUse");
            //获得随机道具
            Player p = player.gameObject.GetComponent<Player>();
            if (p.iCharcaterCount == (int)Charactors_type.Person && photonView.IsMine)
            {
                object[] content = new object[] { player.gameObject.GetComponent<PhotonView>().ViewID }; // Array contains the target position and the IDs of the selected units
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent((byte)Event_Code.GameEnd, content, raiseEventOptions, sendOptions);
            }

        }
    }
}