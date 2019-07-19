using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class LobbyWin : MonoBehaviour
    {
        public RectTransform SetWin;
        public RectTransform IntroWin;
        // Start is called before the first frame update
        void Start()
        {
            SetWin.gameObject.SetActive(false);
            IntroWin.gameObject.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowSettingWin()
        {
            SetWin.gameObject.SetActive(true);
        }

        public void ShowIntroWin()
        {
            IntroWin.gameObject.SetActive(true);
        }
    }
}
