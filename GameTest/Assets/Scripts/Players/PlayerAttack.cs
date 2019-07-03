using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class PlayerAttack : MonoBehaviourPun
    {
        public float timeBetweenAttacks = 0.5f;
        private float attackDamage = 3;
        public KeyCode attackKey = KeyCode.Z;


        public Animator anim;
        //GameObject player;
        public Player player;
        //EnemyHealth enemyHealth;
        float timer;
        float AttackCD = 10f;
        public float AttackRange = 2;
        public string EnemyLayer = "team1";


        void Awake()
        {
            //player = GameObject.FindGameObjectWithTag("Player");
            //player = transform.GetComponent<Player>();
            //enemyHealth = GetComponent<EnemyHealth>();
            //anim = GetComponent<Animator>();
            if (photonView.IsMine)
            {
                switch (gameObject.layer)
                {
                    case 9:
                        EnemyLayer = "team2";
                        break;
                    case 10:
                        EnemyLayer = "team1";
                        break;
                }
            }
        }

        private void Start()
        {
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                if (Input.GetKeyDown(attackKey) && player.iCharcaterCount == (int)Charactors_type.Ghost)
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRange, LayerMask.GetMask(EnemyLayer));
                    if (colliders.Length > 0)
                    {
                        int minIdx = FindCloset(colliders);
                        Attack(colliders[minIdx].transform.GetComponent<Player>());
                    }
                }
            }


            //player.TakeDamage(Time.deltaTime * attackDamage);

        }

        int FindCloset(Collider[] colliders)
        {
            int minIdx = 0;
            float minDistance = Vector3.Distance(transform.position, colliders[minIdx].transform.position);
            for (var i = 1; i < colliders.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                if (distance < minDistance)
                {
                    minIdx = i;
                    minDistance = Vector3.Distance(transform.position, colliders[i].transform.position);
                }
            }
            return minIdx;
        }

        void Attack(Player otherPlayer)
        {
            if (otherPlayer != null && otherPlayer.curr_Health_Point > 0)
            {
                //otherPlayer.TakeDamage(attackDamage);
                
                object[] content = new object[] { otherPlayer.gameObject.GetComponent<PhotonView>().ViewID, attackDamage }; // Array contains the target position and the IDs of the selected units
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent((byte)Event_Code.Attack, content, raiseEventOptions, sendOptions);
            }
        }


    }
}


