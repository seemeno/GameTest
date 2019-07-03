using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Com.MyCompany.MyGame
{
    public class DoorPool
    {
        //门资源池
        public Dictionary<int, DoorBase> Alldoor { get; set; }//<entityID,class>

        public DoorPool()
        {
            Alldoor = new Dictionary<int, DoorBase>();
        }

        public void Add(DoorBase tmp)
        {
            //添加一个门到门资源池中
            if (!Alldoor.ContainsKey(tmp.DoorGUID))
            {
                Alldoor.Add(tmp.DoorGUID, tmp);
            }
        }

        public void Dele(int DoorGUID)
        {
            //从资源池中删除一个门
            var tmp = Alldoor[DoorGUID];
            if (tmp != null)
            {
                Alldoor.Remove(DoorGUID);
            }
        }
    }
}

