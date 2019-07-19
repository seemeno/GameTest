﻿using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject MatchWin;

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        bool isConnecting;
        private readonly byte EnterRoomCode = 100;

        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            //Connect();
            //progressLabel.SetActive(false);
            MatchWin.SetActive(false);
            controlPanel.SetActive(true);
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            isConnecting = true;
            //progressLabel.SetActive(true);
            MatchWin.SetActive(true);
            controlPanel.SetActive(false);
            
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }


        #endregion

        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            if (isConnecting)
                PhotonNetwork.JoinRandomRoom();
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            Debug.Log("******连接成功！******");
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            //progressLabel.SetActive(false);
            MatchWin.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            Debug.Log("******连接失败！******");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
            
            Debug.Log("******创建新房间！******");
        }

        public override void OnJoinedRoom()
        {
            progressLabel.GetComponent<Text>().text = string.Format("Waitting ({0} / 2 )......", PhotonNetwork.CurrentRoom.PlayerCount);
            //if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            //{
            //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            //    SendOptions sendOptions = new SendOptions { Reliability = true };
            //    PhotonNetwork.RaiseEvent(EnterRoomCode, null, raiseEventOptions, sendOptions);
            //}
            
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            Debug.Log("******加入房间！******");
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                PhotonNetwork.LoadLevel("SampleScene");
            }
        }
        //public void OnEvent(EventData photonEvent)
        //{
        //    byte eventCode = photonEvent.Code;
        //    Debug.Log("event code:" + eventCode);

        //    if (eventCode == EnterRoomCode)
        //    {
        //        Debug.Log("进入游戏>>>>>>>>>>>>>>>>>>");
        //    }
        //}

        //public new void OnEnable()
        //{
        //    PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        //}

        //public new void OnDisable()
        //{
        //    PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        //}


        #endregion


    }
}