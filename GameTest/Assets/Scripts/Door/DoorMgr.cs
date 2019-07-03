using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class DoorMgr : MonoBehaviour
    {
        public static DoorMgr Instance; //门管理的单例
        public Dictionary<int, DoorPool> AllDoorPool;//所有门<doortype, 门池>

        private void Awake()
        {
            Instance = this;
            AllDoorPool = new Dictionary<int, DoorPool>();
        }

        public void Add(DoorBase tmp)
        {
            //添加一个门的gameobject到门管理器中
            if (AllDoorPool.ContainsKey(tmp.DoorType) == false)
            {
                AllDoorPool.Add(tmp.DoorType, new DoorPool());
            }
            AllDoorPool[tmp.DoorType].Add(tmp);
        }
        public void Dele(DoorBase tmp)
        {
            //从门管理器中删除一个门
            if (AllDoorPool.ContainsKey(tmp.DoorType))
            {
                AllDoorPool[tmp.DoorType].Dele(tmp.DoorGUID);
            }
        }

        public int GetDoorNum(int DoorType)
        {
            //获得当前门种类（DoorType）的门数量
            if (AllDoorPool.ContainsKey(DoorType))
            {
                return AllDoorPool[DoorType].Alldoor.Count;
            }
            else
            {
                return 0;
            }
        }

        public int GetNormalDoorNum()
        {
            //获得小鬼门的数量
            return GetDoorNum((int)DOORTYPE.NORMALDOOR) +
                GetDoorNum((int)DOORTYPE.TRANSFERDOOR);
        }

        public bool IfDoorShow(int doortype)
        {
            //根据门种类判断门是否为active状态，即是否显现
            if (AllDoorPool.ContainsKey(doortype))
            {
                foreach (var d in AllDoorPool[doortype].Alldoor)
                {
                    if (!d.Value.gameObject.activeInHierarchy)
                        return false;
                }
            }
            return true;
        }

        public DoorBase GetDoorBase(int doorGUID)
        {
            //根据门的唯一标识找到对应对象
            foreach (var d in AllDoorPool)
            {
                if (d.Value.Alldoor.ContainsKey(doorGUID))
                {
                    return d.Value.Alldoor[doorGUID];
                }
            }
            return null;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}


