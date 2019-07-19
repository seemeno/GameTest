using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class IntroWin : MonoBehaviour
    {
        public RectTransform WinGroup;
        public RectTransform[] WinList;
        private int CurWinIdx;
        // Start is called before the first frame update
        void Start()
        {
            ShowWinByIdx(0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickLeftBtn()
        {
            Debug.Log("left");
            ShowWinByIdx(CurWinIdx + 1);
        }

        public void OnClickRightBtn()
        {
            Debug.Log("right");
            ShowWinByIdx(CurWinIdx - 1);
        }

        public void CloseWin()
        {
            gameObject.SetActive(false);
        }

        void ShowWinByIdx(int idx)
        {
            if (idx < 0 || idx >= WinList.Length)
                return;
            Debug.Log(idx);
            CurWinIdx = idx;
            for (int i = 0; i < WinList.Length; i++)
            {
                if (i < idx)
                {
                    WinList[i].anchorMax = new Vector2(0.4f, 0.5f);
                    WinList[i].anchorMin = new Vector2(0.0f, 0.5f);
                    WinList[i].sizeDelta = new Vector2(0, 180);
                }
                else if (i == idx)
                {
               
                    WinList[i].anchorMax = new Vector2(0.4f, 0.5f);
                    WinList[i].anchorMin = new Vector2(0.0f, 0.5f);
                    WinList[i].sizeDelta = new Vector2(0, 200);
                }
                else if (i == idx + 1)
                {
                    WinList[i].anchorMax = new Vector2(0.7f, 0.5f);
                    WinList[i].anchorMin = new Vector2(0.3f, 0.5f);
                    WinList[i].sizeDelta = new Vector2(0, 180);

                }
                else if (i >= idx + 2)
                {
                    WinList[i].anchorMax = new Vector2(1.0f, 0.5f);
                    WinList[i].anchorMin = new Vector2(0.6f, 0.5f);
                    WinList[i].sizeDelta = new Vector2(0, 180);

                }
                WinList[i].SetSiblingIndex(WinList.Length - Mathf.Abs(i - CurWinIdx)-1);
            }
            
        }
    }
}
