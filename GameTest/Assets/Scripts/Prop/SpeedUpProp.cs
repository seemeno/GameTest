using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class SpeedUpProp : PropBase
    {
        private float Duration = 20;//持续时间
        private float IncSpeed = 0.5f;//在原始速度上倍数
                                    // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void UseMethod(Transform tmp)
        {
            var entity = tmp.gameObject.GetComponent<Player>();
            //if (entity == null) return;
            TimeMgr.instance.AddTimer("Speed", new TimeCount(Duration, null, () =>
            {
                DecreaseSpeed(entity);
                TimeMgr.instance.RemoveTimer("Speed");
            }, () =>
            {
                IncreaseSpeed(entity);
            }

            ));
        }

        //减速 
        public void DecreaseSpeed(Player entity)
        {
            if (entity != null)
            {
                entity.Change_Speed(-entity.Initial_Person_Speed * IncSpeed);
                Debug.Log(entity.curr_Speed);
            }
        }

        //恢复速度
        public void RecoverSpeed(Player entity)
        {
            if (entity != null)
            {
                entity.curr_Speed = entity.Initial_Person_Speed;
            }
        }

        //加速
        public void IncreaseSpeed(Player entity)
        {
            if (entity != null)
            {
                entity.Change_Speed(entity.Initial_Person_Speed * IncSpeed);
                Debug.Log(entity.curr_Speed);
            }
        }
        public SpeedUpProp(int type, int GUID, string name, string Desc, string path) : base(type, GUID, name, Desc,path)
        {
        }
    }
}

