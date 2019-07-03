using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class ChangePlayerTypeProp : PropBase
    {
        //角色转换道具
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override void Use(Transform tmp)
        {
            
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent((byte)Event_Code.ChangeCharactor, null, raiseEventOptions, sendOptions);

            //Debug.Log("角色转换身份，需要调用相应的API");
            ////获得所有的人物对象
            //GameObject [] obs = GameObject.FindGameObjectsWithTag("Player");
            //Debug.Log("player个数："+obs.Length.ToString());
            //foreach(var tmpOB in obs)
            //{
            //    Player tmpP = tmpOB.GetComponent<Player>();
            //    Debug.Log("目前角色：" + tmpP.iCharcaterCount);
            //    tmpP.iCharcaterCount = 1 - tmpP.iCharcaterCount;
            //    tmpP.setCharactor(tmpP.iCharcaterCount);
            //    //Debug.Log("最终身份：" + tmpP.iCharcaterCount.ToString());
            //}
            ////tmp.GetComponent<Player1>.setCharactor(Charactors_type.Ghost);
        }
        public ChangePlayerTypeProp(int type, int GUID, string name, string Desc) : base(type, GUID, name, Desc)
        {
        }
    }
}

