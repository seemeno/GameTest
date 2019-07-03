using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    public class GameEndScene : MonoBehaviour
    {

        public Text winText;
        public Text loseText;
        public float totalTime = 4;
        // Start is called before the first frame update
        void Start()
        {
            if (GameController.Instance.GetGameResult())
            {
                winText.GetComponent<CanvasGroup>().alpha = 1;
                winText.GetComponent<CanvasGroup>().interactable = true;
                winText.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                loseText.GetComponent<CanvasGroup>().alpha = 1;
                loseText.GetComponent<CanvasGroup>().interactable = true;
                loseText.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

    }

        // Update is called once per frame
        void Update()
        {
            totalTime -= Time.deltaTime;
            if(totalTime < 0)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
    }
}

