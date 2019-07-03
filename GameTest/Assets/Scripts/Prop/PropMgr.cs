using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public enum PropOperator
    {
        PROP1 = KeyCode.Alpha1,
        PROP2 = KeyCode.Alpha2,
        PROP3 = KeyCode.Alpha3,
        PROP4 = KeyCode.Alpha4,
    }
    public class PropMgr : MonoBehaviour
    {

        public static PropMgr instance; //道具管理类的单例
        public Dictionary<int, PropBase> NormalProp;//所有常规道具，可以放到背包内的
        public Dictionary<int, PropBase> SpecialProp;//特殊道具

        private void Awake()
        {
            instance = this;
            NormalProp = new Dictionary<int, PropBase>();
            SpecialProp = new Dictionary<int, PropBase>();
            JnitProp();
        }

        public void JnitProp()
        {
            //初始化所有道具
            SpeedUpProp sp = new SpeedUpProp(1, (int)PROPGUID.SPEEDUP, "加速", "加速");
            NormalProp.Add(sp.EntityGUID, sp);
            ChangePlayerTypeProp cptp = new ChangePlayerTypeProp(1, (int)PROPGUID.CHANGEPLAYERTYPE, "转换角色身份", "转换角色身份");
            SpecialProp.Add(cptp.EntityGUID, cptp);
            RecoverProp rp = new RecoverProp(1, (int)PROPGUID.RECOVER, "回血", "回血");
            NormalProp.Add(rp.EntityGUID, rp);
        }
        
        //获得当前所有常规道具的数量
        public int GetAllNum() { return NormalProp.Count; }


        public int GetRandomNormalPropEntityID( int[] bagProp)
        {

            //获得背包中没有的道具
            Dictionary<int, PropBase>.KeyCollection kl = NormalProp.Keys;
            int[] keys = new int[kl.Count];
            kl.CopyTo(keys, 0);
            List<int> keysList = new List<int>(keys);

            foreach (int k in bagProp)
            {
                keysList.Remove(k);
            }

            //随机选择一个背包中没有的道具
            //Debug.Log(Random.Range(0, 1).ToString());
            Random.InitState((int)System.DateTime.Now.Ticks);
            if (keysList.Count > 0)
            {
                Debug.Log("剩余道具个数" + keysList.Count.ToString());
                return keysList[Random.Range(0, keysList.Count)];
            }
            else
                return 0;
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



