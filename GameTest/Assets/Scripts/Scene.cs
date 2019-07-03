using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;


namespace Com.MyCompany.MyGame
{
    public class Scene : MonoBehaviourPunCallbacks, IPunObservable
    {
        //单例模式
        public static Scene Instance;//实例
        private float minDoorInverval = 4;
        private float BoundDistance = 2;

        public float MapWidth { get; } = 100; //地图宽度
        public float MapLength { get; } = 110;//地图长度

        Block[] normalBlocks;//生成小鬼门的区域块，每个区域块随机取位置生成小鬼门
        Block[] finalBlocks;//生成大鬼门的区域块，在所有区域块中随机选择一个
        public int[] NormalDoorOrder;//小鬼门出现顺序，与normalBlocks下标对应
        int CurDoorOrder;//当前出现的小鬼门序号，与NormalDoorOrder下标对应
        public int[] NormalDoorGUID;//小鬼门的唯一编号

        //wanghao
        Vector3 doorSize;

        #region Public Fields

        //[SerializeField]
        [Tooltip("The prefab to use for representing the player")]
        public GameObject doorPrefab;
        public GameObject finaldoorPrefab;
        #endregion

        #region PUN Callbacks
        //数据信息同步
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //if (stream.IsWriting)
            //{
            //    // We own this player: send the others our data
            //    stream.SendNext(DoorMgr.Instance);

            //}
            //else
            //{
            //    // Network player, receive data
                
            //    DoorMgr.Instance = (DoorMgr)stream.ReceiveNext();
            //}
        }
        #endregion
        void Awake()
        {
            //createDoorSet();
            //createWall(new Vector3(0.5f,5,10), new Vector3(0,0,0) , new Vector3(1,10,15));
            //createWall(new Vector3(10, 5, 3f), new Vector3(0, -90, 0), new Vector3(1, 10, 20));
            //createWall(new Vector3(10, 5, 17f), new Vector3(0, 90, 0), new Vector3(1, 10, 20));
            Instance = this;



        }

        // Start is called before the first frame update
        void Start()
        {
            //随机种子
            Random.InitState((int)System.DateTime.Now.Ticks);

            //生成地图四周空气墙
            CreateTerrian();

            //生成地图区域块
            InitMapMgr();

            //wanghao:如果是主客户端，初始化门
            if (PhotonNetwork.IsMasterClient || PhotonNetwork.IsConnected == false)
            {
                //生成各种门的gameobject
                Debug.Log("生成门");
                InitDoor();
            }

            //生成小鬼门的出现顺序
            GeretateNormalDoorShowOrder();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void CreateTerrian()
        {

            //TerrainData td = new TerrainData();
            //td.heightmapResolution = 513;
            //td.baseMapResolution = 513;
            //td.size = new Vector3(mapWidth, 50, mapLength);
            //td.alphamapResolution = 512;
            //td.SetDetailResolution(32, 8);
            //GameObject mainTerrian = Terrain.CreateTerrainGameObject(td);

            //生成地图四周的空气墙
            CreateWall2(new Vector3(0, 5, MapLength / 2), new Vector3(0, 0, 0), new Vector3(1, 10, MapLength));
            CreateWall2(new Vector3(MapWidth, 5, MapLength / 2), new Vector3(0, 0, 0), new Vector3(1, 10, MapLength));
            CreateWall2(new Vector3(MapWidth / 2, 5, 0), new Vector3(0, 0, 0), new Vector3(MapWidth, 10, 1));
            CreateWall2(new Vector3(MapWidth / 2, 5, MapLength), new Vector3(0, 0, 0), new Vector3(MapWidth, 10, 1));
        }

        void InitMapMgr()
        {

            //生成地图区域块，每块中最多出现一个鬼门
            //根据块的左上和右下点左边生成
            Vector2[] normalBlockPos = new Vector2[10];
            normalBlockPos[0] = new Vector2(0.0f, 0.0f);
            normalBlockPos[1] = new Vector2(1.0f / 2.0f, 3.0f / 8.0f);

            normalBlockPos[2] = new Vector2(1.0f / 2.0f, 0.0f);
            normalBlockPos[3] = new Vector2(1.0f, 3 / 8.0f);

            normalBlockPos[4] = new Vector2(1.0f / 3.0f, 3.0f / 8.0f);
            normalBlockPos[5] = new Vector2(2.0f / 3.0f, 5.0f / 8.0f);

            normalBlockPos[6] = new Vector2(0, 5.0f / 8.0f);
            normalBlockPos[7] = new Vector2(1.0f / 2.0f, 1.0f);

            normalBlockPos[8] = new Vector2(1.0f / 2.0f, 5.0f / 8.0f);
            normalBlockPos[9] = new Vector2(1.0f, 1.0f);

            Vector2[] finalBlockPos = new Vector2[4];

            finalBlockPos[0] = new Vector2(0, 3.0f / 8.0f);
            finalBlockPos[1] = new Vector2(1.0f / 3.0f, 5.0f / 8.0f);


            finalBlockPos[2] = new Vector2(2.0f / 3.0f, 3.0f / 8.0f);
            finalBlockPos[3] = new Vector2(1, 5.0f / 8.0f);

            //根据块的左上和右下点左边生成Block对象
            normalBlocks = InitBlock(normalBlockPos);
            finalBlocks = InitBlock(finalBlockPos);

        }

        Block[] InitBlock(Vector2[] blockPos)
        {
            //Debug.Log(blockPos.ToString());
            Block[] blocks = new Block[blockPos.Length / 2];
            for (int n = 0; n < blockPos.Length; n += 2)
            {
                //Debug.Log(n / 2);
                //Debug.Log("tl" + blockPos[n].ToString());
                //Debug.Log("br" + blockPos[n + 1].ToString());
                Vector2 tlPos = new Vector2(blockPos[n].x * MapWidth + BoundDistance, blockPos[n].y * MapLength + BoundDistance);
                Vector2 brPos = new Vector2(blockPos[n + 1].x * MapWidth - BoundDistance, blockPos[n + 1].y * MapLength - BoundDistance);

                //Debug.Log("tlPos" + tlPos.ToString());
                //Debug.Log("brPos" + brPos.ToString());

                blocks[n / 2] = new Block(tlPos, brPos);
            }
            return blocks;
        }
        void InitDoor()
        {
            //GameObject doorPrefab = (GameObject)Resources.Load("Prefabs/door");

            //小鬼门
            Vector3 doorPos = new Vector3();
            int doorType;
            NormalDoorGUID = new int[normalBlocks.Length];
            for (int i = 0; i < normalBlocks.Length; i++)
            {
                //Vector3 doorPos = new Vector3();
                doorPos.x = Random.Range(normalBlocks[i].topLeftPos.x, normalBlocks[i].bottomRightPos.x);
                doorPos.z = Random.Range(normalBlocks[i].topLeftPos.y, normalBlocks[i].bottomRightPos.y);
                doorPos.y = 0;// doorPrefab.transform.localScale.y / 2;
                doorType = (int)DOORTYPE.NORMALDOOR;//Random.Range((int)DOORTYPE.NORMALDOOR, (int)DOORTYPE.FINALDOOR);
                NormalDoorGUID[i] = i;
                DoorMgr.Instance.Add(InitOneDoor(doorPrefab, doorPos, doorType, i, "小鬼门", "小鬼门"));
            }

            //大鬼门
            int blockNo = Random.Range(1, finalBlocks.Length);
            doorPos.x = Random.Range(finalBlocks[blockNo].topLeftPos.x, finalBlocks[blockNo].bottomRightPos.x);
            doorPos.z = Random.Range(finalBlocks[blockNo].topLeftPos.y, finalBlocks[blockNo].bottomRightPos.y);
            doorPos.y = 0;// doorPrefab.transform.localScale.y / 2;

            doorType = (int)DOORTYPE.FINALDOOR;
            DoorMgr.Instance.Add(InitOneDoor(finaldoorPrefab, doorPos, doorType, normalBlocks.Length, "大鬼门", "大鬼门"));
        }

        public DoorBase InitOneDoor(GameObject prefab, Vector3 position, int doorType, int doorGUID, string DoorName = "door", string DoorDesc = "door")
        {
            //根据信息生成一个门的对象
            //prefab:门的预制体,position：门的位置, doorType门的种类，doorGUID：门的唯一标识
            //GameObject tmp = Instantiate(prefab);
            //tmp.transform.position = position;
            

            //wanghao
            GameObject tmp = PhotonNetwork.Instantiate(prefab.name, position, Quaternion.identity);
            //door.transform.Rotate(rotation);
            tmp.name = DoorName;
            //tmp.SetActive(false);
            //wanghao
            doorSize = tmp.transform.localScale;
            setIsShow(tmp, false);
            DoorBase doorScript;
            switch (doorType)
            {
                case (int)DOORTYPE.NORMALDOOR:
                    //tmp.AddComponent<NormalDoor>();
                    doorScript = tmp.GetComponent<NormalDoor>();
                    break;
                case (int)DOORTYPE.TRANSFERDOOR:
                    //tmp.AddComponent<TransferDoor>();
                    doorScript = tmp.GetComponent<NormalDoor>();
                    break;
                case (int)DOORTYPE.FINALDOOR:
                    //tmp.AddComponent<TransferDoor>();
                    doorScript = tmp.GetComponent<FinalDoor>();
                    break;
                default:
                    //tmp.AddComponent<NormalDoor>();
                    doorScript = tmp.GetComponent<NormalDoor>();
                    break;
            }
            doorScript.DoorGUID = doorGUID;
            doorScript.DoorType = doorType;
            doorScript.DoorName = DoorName;
            doorScript.DoorDesc = DoorDesc;
            Debug.Log(doorScript);
            return doorScript;
        }
        //wanghao
        private void setIsShow(GameObject gObj, bool isShow)
        {
            if (isShow)
            {
                gObj.transform.localScale = doorSize;
                gObj.GetComponent<BoxCollider>().isTrigger = true;
            }
            else
            {
                gObj.transform.localScale = new Vector3(0, 0, 0);
                gObj.GetComponent<BoxCollider>().isTrigger = false;
            }
        }

        void GeretateNormalDoorShowOrder()
        {
            int doorNum = DoorMgr.Instance.GetNormalDoorNum();
            NormalDoorOrder = GetIndexRandomNum(0, doorNum - 1);
            CurDoorOrder = 0;
        }

        public void ShowNextDoor()
        {
            //显示CurDoorOrder对应的门
            int doorNum = DoorMgr.Instance.GetNormalDoorNum();
            if (CurDoorOrder < doorNum)
            {
                Debug.Log("第" + (CurDoorOrder + 1).ToString() + "个小鬼门出现");
                //DoorMgr.Instance.GetDoorBase(NormalDoorGUID[NormalDoorOrder[CurDoorOrder]]).gameObject.SetActive(true);
                //wanghao
                setIsShow(DoorMgr.Instance.GetDoorBase(NormalDoorGUID[NormalDoorOrder[CurDoorOrder]]).gameObject, true);
                CurDoorOrder++;
            }
        }

        public void ShowFinalDoor()
        {
            //显示大鬼门
            if (DoorMgr.Instance.AllDoorPool.ContainsKey((int)DOORTYPE.FINALDOOR))
            {
                Debug.Log("最终的大鬼门出现");
                foreach (var d in DoorMgr.Instance.AllDoorPool[(int)DOORTYPE.FINALDOOR].Alldoor)
                {
                    //d.Value.gameObject.SetActive(true);
                    //wanghao
                    setIsShow(d.Value.gameObject, true);
                }
            }
            //finalDoor.SetActive(true);
        }

        public static int[] GetIndexRandomNum(int minValue, int maxValue)
        {
            //打乱从minValue到maxValue几个数的顺序
            System.Random random = new System.Random();
            int sum = Mathf.Abs(maxValue - minValue) + 1;
            int site = sum;
            int[] index = new int[sum];
            int[] result = new int[sum];
            int temp = 0;
            for (int i = minValue; i <= maxValue; i++, temp++)
            {
                index[temp] = i;
            }
            for (int i = 0; i < sum; i++, site--)
            {
                int id = random.Next(0, site - 1);
                result[i] = index[id];
                index[id] = index[site - 1];
            }
            return result;

        }

        void CreateWall2(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            //创建墙
            GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeObject.transform.position = position;
            cubeObject.transform.Rotate(rotation);
            cubeObject.transform.localScale = scale;
            cubeObject.name = "wall";
            Destroy(cubeObject.GetComponent<MeshRenderer>());
        }

        //该方法被废弃
        void CreateWall(Vector3 position, Vector3 rotation, Vector3 scale)
        {

            //创建墙
            GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeObject.transform.position = position;
            cubeObject.transform.Rotate(rotation);
            cubeObject.transform.localScale = scale;
            cubeObject.name = "wall";
            //cubeObject.AddComponent<Rigidbody>();
            //cubeObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            //Destroy(cubeObject.GetComponent<MeshRenderer>());


            //创建墙上的门
            GameObject prefab = (GameObject)Resources.Load("Prefabs/door");
            float lengthWall = scale.z;
            float lengthDoor = prefab.transform.localScale.z;
            int countDoor = (int)((lengthWall - BoundDistance * 2) / (minDoorInverval + lengthDoor));

            //Debug.Log("lengthWall"+ lengthWall.ToString());
            //Debug.Log("boundDistance" + boundDistance.ToString());
            //Debug.Log("minDoorInverval" + minDoorInverval.ToString());
            //Debug.Log("lengthDoor" + lengthDoor.ToString());

            //Debug.Log(countDoor);
            Quaternion doorRotation = Quaternion.Euler(rotation);
            for (int ic = 0; ic < countDoor; ic++)
            {
                float x = 0.5f;
                float y = 0;
                float z = (ic - countDoor / 2) * (lengthDoor / 2 + minDoorInverval) / lengthWall;
                //Debug.Log("x" + x.ToString());
                //Debug.Log("y" + y.ToString());
                //Debug.Log("z" + z.ToString());

                Vector3 doorPos = cubeObject.transform.TransformPoint(new Vector3(x, y, z));
                //Debug.Log("doorPos" + doorPos.ToString());
                doorPos.y = prefab.transform.localScale.y / 2;

                GameObject door = Instantiate(prefab);
                door.transform.position = doorPos;
                door.transform.Rotate(rotation);
                door.name = "door";
            }
        }

    }


    public struct Block
    {
        public Block(Vector2 tlPos, Vector2 brPos)
        {
            blockWidth = tlPos.x - brPos.x;
            blockLength = tlPos.y - brPos.y;
            topLeftPos = tlPos;
            bottomRightPos = brPos;
            //Debug.Log("tlPos" + tlPos.ToString());
            // Debug.Log("brPos" + brPos.ToString());
        }
        public float blockWidth;//块的宽度
        public float blockLength;//块的长度
        public Vector3 topLeftPos;//左上角顶点位置
        public Vector3 bottomRightPos;//右上角顶点位置

    }
}
