using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

namespace Com.MyCompany.MyGame
{
    public class SetWin : MonoBehaviour
    {

        public Slider slider;
        public AudioSource music;
        public float musicVolume;
        public GameObject OjAudio;
        public bool IsFullScreen;
        public Dropdown dropdown;

        public Vector2Int[] ResolutionList = {
            new Vector2Int(1366, 768),
            new Vector2Int(1600, 1200),
            new Vector2Int(640, 480)
        };

        // Start is called before the first frame update
        void Start()
        {
            musicVolume = 0.5F;
            slider.value = musicVolume;
            music.volume = musicVolume;
            music.Play();
            DontDestroyOnLoad(OjAudio);
            dropdown.ClearOptions();
            OptionData tempData;
            for (int i = 0; i < ResolutionList.Length; i++)
            {
                tempData = new Dropdown.OptionData();
                tempData.text = ResolutionList[i].x.ToString()+'X'+ ResolutionList[i].y.ToString();
                dropdown.options.Add(tempData);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeVoice()
        {
            music.volume = slider.value;
        }

        public void CloseWin()
        {
            gameObject.SetActive(false);
        }

        public void SetFullScreen()
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }


        public void SetWinScreen()
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        }

        public void SetResolution()
        {
            Screen.SetResolution(ResolutionList[dropdown.value].y, ResolutionList[dropdown.value].x, IsFullScreen);
        }


    }
}
