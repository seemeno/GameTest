using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class RangeImage : MonoBehaviour
    {
        private float Range;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("SetRange");
            RectTransform image = gameObject.GetComponent<RectTransform>();
            RectTransform map = MiniMap.Instance.GetComponent<RectTransform>();
            image.sizeDelta = new Vector2(
                Range / Scene.Instance.MapWidth * map.sizeDelta.x,
                Range / Scene.Instance.MapLength * map.sizeDelta.y);
            Debug.Log("长度" + Range / Scene.Instance.MapWidth * map.sizeDelta.x);
            Debug.Log("糯米" + image.sizeDelta);
        }

        // Update is called once per frame
        void Update()
        {
            //RectTransform image = gameObject.GetComponent<RectTransform>();
            //image.sizeDelta.Set(20, 20);
            //Debug.Log("糯米" + image.sizeDelta);
        }
        public void SetRange(float range)
        {
            Range = range;
        }
    }
}