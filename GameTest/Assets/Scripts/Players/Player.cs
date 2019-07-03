using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;


namespace Com.MyCompany.MyGame
{
    public enum Charactors_type : int
    {
        //角色类型
        Ghost = 1,
        Person = 0,
    }
    public enum Charactors_State : int
    {
        //角色状态
        FORWARD = 0,//角色状态向前
        RIGHT = 1,//角色状态向右
        BACKWORD = 2,//角色状态向后
        LEFT = 3,//角色状态向左
    }

    public enum Event_Code : byte
    {
        Attack = 0,
        ChangeCharactor = 1,
        GameEnd = 2,
        GameEnter = 3,
    }

    public class Player : MonoBehaviourPun, IPunObservable
    {
        //坐标系转换
        Vector3 CFORWARD = Vector3.forward;
        Vector3 CBACK = Vector3.back;
        Vector3 CLEFT = Vector3.left;
        Vector3 CRIGHT = Vector3.right;


        private Transform m_Transform;
        private CharacterController controller;
        private Animator anim;

        public float Initial_Person_Speed = 5;//角色初始速度
        public float Initial_HP = 10;//角色初始血量

        private int Player_State;//角色转向状态
        private int Player_oldState = 0;//前一次角色的状态


        public float curr_Health_Point;//角色血量
        public float curr_Speed;//角色速度
        public float Jumpspeed = 8;//跳跃速度
        protected Vector3 movement = Vector3.zero;

        private int Buttons_Down = 0;//记录按下的键数量
        private bool Left_Down = false;//左是否按下
        private bool Right_Down = false;//右是否按下

        public MiniMap MinimapUI;


        public int iCharcaterCount;//
        bool isDead;
        bool damaged;
        public Image damageImage;
        public Transform healthSlider;
        public MessageUI message;
        private float flashSpeed = 5f;
        private Color flashColour = new Color(1f, 0f, 0f, 0.1f);


        void Awake()
        {
            m_Transform = gameObject.GetComponent<Transform>();
            controller = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            curr_Health_Point = Initial_HP;

            if (PhotonNetwork.IsMasterClient || PhotonNetwork.IsConnected == false)
            {
                gameObject.layer = 9;
                iCharcaterCount = 1;
            }
            else
            {
                gameObject.layer = 10;
                iCharcaterCount = 0;
            }

            //GameObject prefab = (GameObject)Resources.Load("Prefabs/ObjectIcon");
            //MiniMap.Instance.AddObjIcon(prefab, m_Transform);
        }

        void Start()
        {
            if (photonView.IsMine || PhotonNetwork.IsConnected == false)
            {
                //wanghao：相机同步
                CameraFollow _cameraWork = this.gameObject.GetComponent<CameraFollow>();


                if (_cameraWork != null)
                {
                    _cameraWork.OnStartFollowing();
                }
                else
                {
                    Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
                }

                transform.Find("ScreenUI").gameObject.SetActive(true);

                setCharactor(iCharcaterCount);
            }
            
        }

        #region PUN Callbacks
        //wanghao:数据信息同步
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(iCharcaterCount);
                stream.SendNext(curr_Health_Point);
                stream.SendNext(gameObject.layer);
            }
            else
            {
                // Network player, receive data
                this.iCharcaterCount = (int)stream.ReceiveNext();
                this.curr_Health_Point = (float)stream.ReceiveNext();
                this.gameObject.layer = (int)stream.ReceiveNext();
            }
        }
        #endregion

        public void OnEvent(EventData photonEvent)
        {
            if (photonView.IsMine)
            {
                byte eventCode = photonEvent.Code;
                Debug.Log("event code:" + eventCode);
                switch (eventCode)
                {
                    case (byte)Event_Code.Attack:
                        {
                            Debug.Log(">>>>>>>>>>>>>>>>>event acctck");
                            object[] data = (object[])photonEvent.CustomData;

                            int viewId = (int)data[0];
                            float attackDamage = (float)data[1];
                            if (gameObject.GetComponent<PhotonView>().ViewID == viewId)
                                TakeDamage(attackDamage);
                            break;
                        }
                    case (byte)Event_Code.ChangeCharactor:
                        {
                            Debug.Log(">>>>>>>>>>>>>>>>>event ChangeCharactor");
                            iCharcaterCount = 1 - iCharcaterCount;
                            setCharactor(iCharcaterCount);
                            break;
                        }
                    case (byte)Event_Code.GameEnd:
                        {
                            Debug.Log(">>>>>>>>>>>>>>>>>event GameEnd");
                            object[] data = (object[])photonEvent.CustomData;
                            int viewId = (int)data[0];
                            Debug.Log("target viewid:" + viewId + "my viewid:" + gameObject.GetComponent<PhotonView>().ViewID);
                            if (gameObject.GetComponent<PhotonView>().ViewID == viewId)
                                GameController.Instance.GoIntoGameEnd(true);
                            else
                                GameController.Instance.GoIntoGameEnd(false);
                            break;
                        }

                    default:
                        break;
                }
            }
                
            
        }

        public void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        public void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        #region MonoBehaviour CallBacks
        void Update()
        {
            LKATCMR();

            //wanghao
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            //初始化角色
            Init();
            //setCharactor(iCharcaterCount);
            BeatenAnim();
            //jump
            if (Input.GetButtonDown("Jump"))
                Jump();

            //chartest();

            //move
            Move();
            Animation();
        }
        #endregion

        public void Move()
        {
            //向前
            if (Input.GetKey(KeyCode.W))
                setState((int)Charactors_State.FORWARD);
            //向后
            if (Input.GetKey(KeyCode.S))
                setState((int)Charactors_State.BACKWORD);
            //向左
            if (Input.GetKey(KeyCode.A))
                setState((int)Charactors_State.LEFT);
            //向右
            if (Input.GetKey(KeyCode.D))
                setState((int)Charactors_State.RIGHT);
        }
        public void Animation()
        {
            //向前
            if (Input.GetKeyDown(KeyCode.W))
                PlayAnimation((int)Charactors_State.FORWARD);
            if (Input.GetKeyUp(KeyCode.W))
                StopAnimation((int)Charactors_State.FORWARD);

            //向后
            if (Input.GetKeyDown(KeyCode.S))
                PlayAnimation((int)Charactors_State.BACKWORD);
            else if (Input.GetKeyUp(KeyCode.S))
                StopAnimation((int)Charactors_State.BACKWORD);

            //向左
            if (Input.GetKeyDown(KeyCode.A))
                PlayAnimation((int)Charactors_State.LEFT);
            else if (Input.GetKeyUp(KeyCode.A))
                StopAnimation((int)Charactors_State.LEFT);
            //向右
            if (Input.GetKeyDown(KeyCode.D))
                PlayAnimation((int)Charactors_State.RIGHT);
            else if (Input.GetKeyUp(KeyCode.D))
                StopAnimation((int)Charactors_State.RIGHT);
        }

        public void Init()
        {//初始化步骤，把角色放在地上
            if (!controller.isGrounded)
            {
                movement.y += Physics.gravity.y * Time.deltaTime;
            }
            controller.Move(movement * Time.deltaTime * Jumpspeed);
        }

        public void Jump()
        {//跳
            if (controller.isGrounded == true)
            {
                movement.y = 4.0f;
            }
            controller.Move(movement * Time.deltaTime * Jumpspeed);
        }


        public void setState(int currState)
        {//位移
            Vector3 transformValue = new Vector3();//定义平移向量
            switch (currState)
            {
                case 0://角色状态向前时，角色不断向前缓慢移动
                    transformValue = CFORWARD * Time.deltaTime * curr_Speed;
                    break;
                case 1://角色状态向右时。角色不断向右缓慢移动
                    transformValue = CRIGHT * Time.deltaTime * curr_Speed;

                    break;
                case 2://角色状态向后时。角色不断向后缓慢移动
                    transformValue = CBACK * Time.deltaTime * curr_Speed;
                    break;
                case 3://角色状态向左时，角色不断向左缓慢移动
                    transformValue = CLEFT * Time.deltaTime * curr_Speed;
                    break;
            }
            transform.Translate(transformValue, Space.Self);//平移角色
            Player_oldState = Player_State;//赋值，方便下一次计算
            Player_State = currState;//赋值，方便下一次计算
        }

        public void LKATCMR()
        {//角色始终面向摄像机
            this.transform.rotation = Camera.main.transform.rotation;
            //this.transform.LookAt(Camera.main.transform.position);
            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - this.transform.position), 0);
        }

        public void setCharactor(int iCharcaterCount)
        {//初始化角色（0为人，1为鬼）
            if (iCharcaterCount == (int)Charactors_type.Person)
            {
                MinimapUI.gameObject.SetActive(true);
                curr_Speed = Initial_Person_Speed;
                anim.SetInteger("iCharcaterCount", 0);
            }
            else if (iCharcaterCount == (int)Charactors_type.Ghost)
            {
                MinimapUI.gameObject.SetActive(false);
                curr_Speed = Initial_Person_Speed * 2;
                anim.SetInteger("iCharcaterCount", 1);
            }

        }

        public void PlayAnimation(int currKeyDown)
        {//播放动画
            Buttons_Down++;
            switch (currKeyDown)
            {
                case 0://向前
                    if (Left_Down)
                    {
                        anim.SetInteger("Right", 1);
                    }
                    else
                    {
                        anim.SetInteger("Left", 1);
                    }
                    break;
                case 1://向右
                    Right_Down = true;
                    anim.SetInteger("Right", 0);
                    anim.SetInteger("Left", 1);
                    break;
                case 2://向后
                    if (Left_Down)
                    {
                        anim.SetInteger("Right", 1);
                    }
                    else
                    {
                        anim.SetInteger("Left", 1);
                    }
                    break;
                case 3://向左
                    Left_Down = true;
                    anim.SetInteger("Left", 0);
                    anim.SetInteger("Right", 1);
                    break;
            }
        }

        public void StopAnimation(int currKeyUp)
        {//动画播放结束返回
            Buttons_Down--;
            if (!(Buttons_Down > 0))
            {
                switch (currKeyUp)
                {
                    case 0://向前后返回
                        anim.SetInteger("Right", 0);
                        anim.SetInteger("Left", 0);
                        break;
                    case 1://向右后返回
                        Left_Down = false;
                        anim.SetInteger("Right", 0);
                        anim.SetInteger("Left", 0);
                        break;
                    case 2://向后后返回
                        anim.SetInteger("Right", 0);
                        anim.SetInteger("Left", 0);
                        break;
                    case 3://向左后返回
                        Right_Down = false;
                        anim.SetInteger("Right", 0);
                        anim.SetInteger("Left", 0);
                        break;
                }
            }
            else
            {
                if (currKeyUp == (int)Charactors_State.LEFT)
                    Left_Down = false;
                if (currKeyUp == (int)Charactors_State.RIGHT)
                    Right_Down = false;
            }

        }
        public void change_HP(int delta_Point)
        {
            curr_Health_Point -= delta_Point;
        }


        public void BeatenAnim()
        {
            if (damaged)
            {
                Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>BeatenAnim:" + damaged);
                damageImage.color = flashColour;
            }
            else
            {
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
            damaged = false;
        }


        public void TakeDamage(float amount)
        {
            damaged = true;
            curr_Health_Point -= amount;

            //healthSlider.value = currentHealth;

            //playerAudio.Play();

            if (curr_Health_Point <= 0 && !isDead)
            {
                Debug.Log("i am dead!");
                Is_Death();
            }
        }

        public void Is_Death()
        {//玩家死亡
            isDead = true;
            anim.SetTrigger("Die");

            //另一个玩家胜利
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                if (player.GetComponent<PhotonView>().ViewID != gameObject.GetComponent<PhotonView>().ViewID)
                {
                    object[] content = new object[] { player.GetComponent<PhotonView>().ViewID }; // Array contains the target position and the IDs of the selected units
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                    SendOptions sendOptions = new SendOptions { Reliability = true };
                    PhotonNetwork.RaiseEvent((byte)Event_Code.GameEnd, content, raiseEventOptions, sendOptions);
                    break;
                }
            }

            //playerAudio.clip = deathClip;
            //playerAudio.Play();

        }

        public void Change_Speed(int delta_Speed)
        {
            curr_Speed += delta_Speed;
        }

        public void MoveDirect(Vector3 targetPos)
        {//人物位置移动
            transform.position = targetPos;
            //controller.Move(targetPos);
        }

    }
}



