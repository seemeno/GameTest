using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class PropBase
    {
        public int EntityType { get; }//道具种类
        public int EntityGUID { get; }//道具唯一ID
        public string EntityName { get; }//道具名称
        public string EntityDesc { get; }//道具描述
        public int OwnMaxCountLimit { get; } = 1;//拥有的道具数量最大限制

        public virtual void Use(Transform tmp)
        {
                Debug.Log("使用道具" + EntityName);
                UseMethod(tmp);
        }

        public virtual void UseMethod(Transform tmp) { }

        public PropBase(int type, int GUID, string name, string Desc = "", int limit = 1)
        {
            this.EntityType = type;
            this.EntityGUID = GUID;
            this.EntityName = name;
            this.EntityDesc = Desc;
            this.OwnMaxCountLimit = limit;
        }
    }
}


