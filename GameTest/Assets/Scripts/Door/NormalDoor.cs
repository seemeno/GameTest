using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    public class NormalDoor : DoorBase
    {
        // Start is called before the first frame update
        // public int DoorType = (int)DOORTYPE.NORMALDOOR;
        
        void Start()
        {
            //wanghao
            //GameObject prefab = (GameObject)Resources.Load("Prefabs/ObjectIcon");
            //MiniMap.Instance.AddObjIcon(prefab, gameObject.GetComponent<Transform>());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnTriggerEnterUse(Collider player)
        {
            //先获得道具，再转换角色，只有当前角色为人能够获得对应道具
            //常规门触发事件
            Debug.Log("NormalDoor : OnTriggerEnterUse, Player ViewId: " + player.gameObject.GetComponent<PhotonView>().ViewID.ToString());
            //获得随机道具
            Bag bag = player.gameObject.GetComponent<Bag>();
            int entityType = PropMgr.instance.GetRandomNormalPropEntityID(bag.GetBagPropGUID());
            //wanghao
            bag.AddProp(entityType, player.gameObject.GetComponent<Player>().message,PropMgr.instance.NormalProp[entityType].Count);
            //使用角色变换道具
            if (photonView.IsMine)
                PropMgr.instance.SpecialProp[(int)PROPGUID.CHANGEPLAYERTYPE].Use(player.transform);
		}
    }
}

