using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.MyGame
{
    public class GameController : MonoBehaviourPunCallbacks
    {
        //游戏控制器
        private GAMESTATE state = GAMESTATE.WAITSTAGE; // 当前游戏状态
        private float GameWaitStageTime { get; } = 10;//进入游戏倒计时
        private float GameFirstStageTime { get; } = 480;//游戏第一阶段持续时间
        private float GameSecondStageTime { get; } = 640;//游戏第二阶段持续时间
        private float NormalDoorShowTime { get; } = 60;//小鬼门的出现时间间隔

        static public GameController Instance;

        private bool GameResult;

        public GAMESTATE GetCurGameState()
        {
            //获得当前游戏状态
            return state;
        }

        public bool GetGameResult()
        {
            //获得当前游戏状态
            return GameResult;
        }

        private void Awake()
        {
        }
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            //进入游戏
            //wanghao：如果是主客户端，才进行游戏状态控制
            if (PhotonNetwork.IsMasterClient || PhotonNetwork.IsConnected == false)
                GoIntoGameScene();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void GoIntoGameScene()
        {
            //等待（倒计时）阶段
            TimeMgr.instance.AddTimer("WaitTime",
                new TimeCount(GameWaitStageTime, null, () =>
                {
                //进入游戏第一阶段
                GoIntoGameFirstStage();
                    TimeMgr.instance.RemoveTimer("WaitTime");
                }, () =>
                {
                    state = GAMESTATE.WAITSTAGE;
                //游戏准备阶段操作
                Debug.Log("倒计时阶段");
                }
            ));
        }

        public void GoIntoGameFirstStage()
        {
            //游戏第一阶段
            TimeMgr.instance.AddTimer("FirstStageTime",
                new TimeCount(GameFirstStageTime, null, () =>
                {
                    GoIntoGameSecondStage();
                    TimeMgr.instance.RemoveTimer("FirstStageTime");
                }, () =>
                {
                    state = GAMESTATE.FIRSTSTAGE;

                //游戏第一阶段操作
                Debug.Log("游戏第一阶段");
                    ShowNormalDoor();
                }


             ));
        }

        public void ShowNormalDoor()
        {
            //小鬼门显现
            Debug.Log("ShowNormalDoor");

            TimeMgr.instance.AddTimer("ShowNormalDoor",
                new TimeCount(NormalDoorShowTime, null, () =>
                {
                    TimeMgr.instance.RemoveTimer("ShowNormalDoor");
                //ShowNormalDoor();
            }, () =>
            {
                Scene.Instance.ShowNextDoor();
            }
                    , DoorMgr.Instance.GetNormalDoorNum()));
        }

        public void GoIntoGameSecondStage()
        {
            //游戏第二阶段
            TimeMgr.instance.AddTimer("SecondStageTime",
                new TimeCount(GameSecondStageTime, null, () =>
                {
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    foreach (var player in players)
                    {
                        if (player.GetComponent<Player>().iCharcaterCount == (int)Charactors_type.Ghost)
                        {
                            object[] content = new object[] { player.GetComponent<PhotonView>().ViewID }; // Array contains the target position and the IDs of the selected units
                            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                            SendOptions sendOptions = new SendOptions { Reliability = true };
                            PhotonNetwork.RaiseEvent((byte)Event_Code.GameEnd, content, raiseEventOptions, sendOptions);
                            break;
                        }
                    }
                    TimeMgr.instance.RemoveTimer("SecondStageTime");
                }, () =>
                {
                    state = GAMESTATE.SECONDSTAGE;
                //游戏第二阶段操作
                Scene.Instance.ShowFinalDoor();
                    Debug.Log("游戏第二阶段");
                }
             ));
        }

        public void GoIntoGameEnd(bool isWin)
        {
            
            //游戏结束
            state = GAMESTATE.ENDSTAGE;
            GameResult = isWin;
            Debug.Log("游戏结束:"+ isWin);
            PhotonNetwork.LoadLevel("GameEnd");
        }
    }
}

