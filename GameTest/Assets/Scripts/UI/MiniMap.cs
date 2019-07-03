using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
namespace Com.MyCompany.MyGame
{
    public class MiniMap : MonoBehaviourPunCallbacks
    {
        public static MiniMap Instance;
        public List<ObjIconInMap> ObjIconList;
        public GameObject objIconPrefab;
        public GameObject myIconPrefab;
        //wanghao
        bool isAdd = false;
        // Start is called before the first frame update
        private void Awake()
        {
            Instance = this;
            if (Instance == null)
                Debug.Log("no Instance");

            ObjIconList = new List<ObjIconInMap>();

        }
        void Start()
        {
            //wanghao:为门添加小地图图标

            Debug.Log("发现人》》》》》》》》》》》》》》" + GameObject.FindGameObjectsWithTag("Player").Length.ToString());
            Debug.Log("发现门》》》》》》》》》》》》》》" + GameObject.FindGameObjectsWithTag("Door").Length.ToString());

            foreach (var d in GameObject.FindGameObjectsWithTag("Door"))
            {
                AddObjIcon(objIconPrefab, d);
                isAdd = true;
            }
            AddObjIcon(myIconPrefab, transform.parent.parent.gameObject);
        }

        //wanghao
        public Transform AddObjIcon(GameObject prefab, GameObject obj)
        {
            if (obj == null)
            {
                Debug.Log("没有添加对象");
                return null;
            }
            
            //GameObject tmp = PhotonNetwork.Instantiate(prefab.name, new Vector3(), Quaternion.identity);
            GameObject tmp = Instantiate(prefab);
            tmp.AddComponent<ObjIconInMap>();
            tmp.transform.parent = transform;

            ObjIconInMap script = tmp.GetComponent<ObjIconInMap>();
            script.ObjIcon = obj;

            ObjIconList.Add(script);
            return tmp.transform;
        }



        // Update is called once per frame
        void Update()
        {
            //wanghao
            if (!isAdd)
            {
                foreach (var d in GameObject.FindGameObjectsWithTag("Door"))
                {
                    AddObjIcon(objIconPrefab, d);
                    isAdd = true;
                }
            }

        }
    }

}
