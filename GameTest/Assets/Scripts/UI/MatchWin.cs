using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class MatchWin : MonoBehaviour
    {
        private bool IsStartCount;
        private float CurTime;
        private float StartTime;
        public Text text;
        // Start is called before the first frame update
        private void Awake()
        {
        }
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("IsStartCount" + IsStartCount);
            if (IsStartCount)
            {
                int duration = (int)Mathf.Floor(Time.time - StartTime);
                int second = duration % 60;
                int minite = duration / 60;
                text.text = string.Format("{0:D2}", minite) + ':'+ string.Format("{0:D2}", second);
            }
        }

        public void StartCount()
        {
            Debug.Log("StartCount");
            IsStartCount = true;
            Debug.Log("IsStartCount" + IsStartCount);
            StartTime = Time.time;
        }

        public void StopCount()
        {
            IsStartCount = false;
        }

        public void OnCloseWin()
        {
            gameObject.SetActive(false);
        }
    }
}
