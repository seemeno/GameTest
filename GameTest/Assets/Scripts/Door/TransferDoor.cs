using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class TransferDoor : DoorBase
    {
        // Start is called before the first frame update
        void Start()
        {
            //wanghao
            //GameObject prefab = (GameObject)Resources.Load("Prefabs/ObjectIcon");
            //MiniMap.Instance.AddObjIcon(prefab, gameObject.GetComponent<Transform>());

        }

        public override void OnTriggerEnterUse(Collider player)
        {
            Debug.Log("RenYiDoor:OnTriggerEnterUse");

            if (DoorMgr.Instance.AllDoorPool.ContainsKey(DoorType) == true)
            {
                if (DoorMgr.Instance.AllDoorPool[DoorType].Alldoor.ContainsKey(DoorGUID))
                {

                    //获得所有同类型的门的GUID（除当前门）
                    DoorPool curPool = DoorMgr.Instance.AllDoorPool[DoorType];
                    int numDoor = curPool.Alldoor.Count;

                    int[] idSet = new int[numDoor - 1];

                    int idx = 0;
                    foreach (int key in curPool.Alldoor.Keys)
                    {
                        if (key != DoorGUID)
                        {

                            idSet[idx] = key;
                            idx++;
                        }
                    }

                    //从中随机选一个门
                    int targetNum = Random.Range(1, idSet.Length);
                    DoorBase targetDoor = curPool.Alldoor[idSet[targetNum]];

                    //获取目标门的前方位置
                    Vector3 targetPos = targetDoor.GetForwardPos();

                    //将玩家直接传送
                    player.gameObject.GetComponent<Player>().MoveDirect(targetPos);


                    //Debug.Log("targetNum" + targetNum.ToString());
                    //Debug.Log("idSet.Length" + idSet.Length.ToString());
                    //Debug.Log("targetNum" + targetNum.ToString());
                    //Debug.Log("targetPos" + targetPos.ToString());

                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

