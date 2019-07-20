using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class GameIntroInstance : MonoBehaviour
    {
        public Text text;
        public Image ObImage;
        public Image TitleImage;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetInstance(string tmpText, GAMETROTYPE type, string path = "")
        {
            if (path == "")
            {
                text.GetComponent<RectTransform>().anchorMax = new Vector2(0.95f, 0.7f);
                ObImage.gameObject.SetActive(false);
            }
            else
            {
                ObImage.gameObject.SetActive(true);
                ObImage.sprite = Resources.Load<Sprite>(path);
                text.GetComponent<RectTransform>().anchorMax = new Vector2(0.95f, 0.5f);
            }
            text.text = tmpText;

            switch (type)
            {
                case GAMETROTYPE.GAMEINTRO:
                    TitleImage.sprite = Resources.Load<Sprite>("UI/IntroductionUI/GameWord");
                    TitleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
                    break;
                case GAMETROTYPE.SENCEINTRO:
                    TitleImage.sprite = Resources.Load<Sprite>("UI/IntroductionUI/SenceWord");
                    TitleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
                    break;
                case GAMETROTYPE.TOOLINTRO:
                    TitleImage.sprite = Resources.Load<Sprite>("UI/IntroductionUI/PropWord");
                    TitleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 30);
                    break;
                default:
                    break;
            }
        }
    }
}
