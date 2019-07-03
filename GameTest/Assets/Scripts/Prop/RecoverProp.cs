using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class RecoverProp : PropBase
    {
        private float IncHP = 20;
        //回血道具
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
            Debug.Log("使用回血道具，需要调用player中的方法");

            var entity = tmp.gameObject.GetComponent<Player>();
            var finalHP = IncHP + entity.curr_Health_Point;
            if (entity.Initial_HP <= finalHP)
                finalHP = entity.Initial_HP;
            entity.curr_Health_Point = finalHP;
        }
        public RecoverProp(int type, int GUID, string name, string Desc) : base(type, GUID, name, Desc)
        {
        }

    }
}


