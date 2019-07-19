using System.Collections;

using System.Collections.Generic;

using UnityEngine;
namespace Com.MyCompany.MyGame
{
    public class KnapsackManager : MonoBehaviour
    {

        public GridPanel gridPanel;
        
        public Bag bag;

        //private static KnapsackManager instance;

        //public static KnapsackManager Instance
        //{

        //    get
        //    {

        //        return instance;

        //    }

        //}

        void Awake()
        {

            //instance = this;

            //this.LoadData();

        }

        public void StoreItem(int GUID, int COUNT)
        {

            //PropMgr.instance.NormalProp[GUID];
            Debug.Log("添加道具栏");
            GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/ItemImage");
            Transform emptyGrid = gridPanel.GetEmptyGrid();
            itemPrefab.GetComponent<ItemImage>().UpdateItem(PropMgr.instance.NormalProp[GUID].EntityName,COUNT);

            GameObject itemGo = GameObject.Instantiate(itemPrefab);

            itemGo.transform.SetParent(emptyGrid);

            itemGo.transform.localPosition = Vector3.zero;

            itemGo.transform.localScale = Vector3.one;

        }

    }
}