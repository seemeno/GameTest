using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class RandomPosProp : PropBase
    {
        //该方法还没写，预计是随机传送道具，和transferDoor重复
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
            if (entity == null) return;
            
            Vector3 pos = new Vector3(Random.Range(0, Scene.Instance.MapWidth), 0, Random.Range(0, Scene.Instance.MapLength));
            entity.MoveDirect(pos);
        }


        public RandomPosProp(int type, int GUID, string name, string Desc) : base(type,  GUID, name, Desc)
        {
        }
    }
}

