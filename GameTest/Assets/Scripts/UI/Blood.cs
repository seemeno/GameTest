using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.


namespace Com.MyCompany.MyGame
{
    public class Blood : MonoBehaviour
    {
        //血条
        Player mainPlayer;
        private Slider bloodSlider;
        // Start is called before the first frame update

        private void Awake()
        {
            mainPlayer = GameObject.Find("MainPlayer").GetComponent<Player>();
            //curBlood = originalBlood;
            bloodSlider = this.GetComponent<Slider>();
            bloodSlider.maxValue = mainPlayer.Initial_HP;//
            bloodSlider.value = mainPlayer.curr_Health_Point;
        }
        void Start()
        {
            bloodSlider.value = mainPlayer.curr_Health_Point;
        }

        // Update is called once per frame
        void Update()
        {
            bloodSlider.value = mainPlayer.curr_Health_Point;
        }

        public void Update(float hp)
        {
            //更新血条
            bloodSlider.value = hp;
        }
    }
}

