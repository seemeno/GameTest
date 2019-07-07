using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
namespace Com.MyCompany.MyGame
{
    public class Bag : MonoBehaviourPun
    {
        private Dictionary<int, int> BagContent;//<GUID,COUNT>，保存道具的唯一标识和数量
        public int MaxOwnPropCount { get; set; } = 5; //背包最大容量
        Player player;
                                                      // Start is called before the first frame update
        void Start()
        {
            BagContent = new Dictionary<int, int>();
            player = transform.GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {  //使用道具
                if (Input.GetKeyUp(KeyCode.J))
                {
                    UsePropInBag();
                }

            }
              
        }
        
        //wanghao
        public void AddProp(int EntityGUID, MessageUI message, int count = 1)
        {
            Debug.Log("背包里有" + GetPropNumInBag() + "个道具");
            if (IsGhost())
            {
                message.AddMessage("鬼不能加道具");
            }

            //将EntityGUID道具添加到背包中
            else if (!PropMgr.instance.NormalProp.ContainsKey(EntityGUID))
            {
                Debug.Log("error!!there is no prop whose entityID is " + EntityGUID.ToString());
                //return false;
            }
            else if (!BagContent.ContainsKey(EntityGUID))
            {
                if (BagContent.Count == MaxOwnPropCount)
                {
                    message.AddMessage("error!! 背包容量已经达到最大值");
                    //return false;
                }
                else
                {
                    BagContent.Add(EntityGUID, count);
                    message.AddMessage("获得" + PropMgr.instance.NormalProp[EntityGUID].EntityName + "道具");
                    //return true;
                }

            }
            else if (PropMgr.instance.NormalProp[EntityGUID].OwnMaxCountLimit <= count + BagContent[EntityGUID])
            {
                message.AddMessage("error!!当前道具已达到拥有的最大数量");
                //return false;
            }
            else
            {
                Debug.Log(PropMgr.instance.NormalProp[EntityGUID].EntityName + "道具数量增加");
                BagContent[EntityGUID] += count;
                //return true;
            }  
        }


        //获得背包中道具数量
        public int GetPropNumInBag() { return BagContent.Count; }


        //获得编号EntityGUID的道具数量
        public int GetOwnNum(int EntityGUID) { return BagContent[EntityGUID]; }

        public int[] GetBagPropGUID()
        {
            Dictionary<int, int>.KeyCollection kl = BagContent.Keys;
            int[] bk = new int[kl.Count];
            kl.CopyTo(bk, 0);
            return bk;
        }

        public bool UsePropInBag(int bagSite = 1)
        {
            if (IsGhost())
            {
                MessageUI.instance.AddMessage("鬼不能使用道具");
                return false;
            }
            //使用背包中的物体，根据在背包中的位置bagSite来判断
            //bagSite从1开始
            int[] keys = GetBagPropGUID();
            if (bagSite < 1 || bagSite > keys.Length)
            {
                MessageUI.instance.AddMessage("不存在该道具");
                return false;
            }
            else
            {
                int entityGUID = keys[bagSite - 1];
                PropMgr.instance.NormalProp[entityGUID].Use(transform);
                MessageUI.instance.AddMessage("使用"+ PropMgr.instance.NormalProp[entityGUID].EntityName + "道具");
                BagContent[entityGUID]--;
                if (BagContent[entityGUID] <= 0)
                {
                    BagContent.Remove(entityGUID);
                }
                return true ;
            }
        }


        public bool IsGhost()
        {
            return player.iCharcaterCount == (int)Charactors_type.Ghost;
        }
    }
}

