using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{

    public struct OneGameIntro
    {
        public string text;
        public GAMETROTYPE type;
        public string path;
        public OneGameIntro(string tmpText, GAMETROTYPE tmpType, string tmpPath) {
            this.text = tmpText;
            this.type = tmpType;
            this.path = tmpPath;
        }
    }

    public class IntroWin : MonoBehaviour
    {
        public RectTransform WinGroup;
        public List<RectTransform> WinList;
        List<OneGameIntro> IntroList;
        private int CurWinIdx;
        // Start is called before the first frame update
        private void Awake()
        {
            WinList = new List<RectTransform>();
            IntroList = new List<OneGameIntro>();
            InitIntro();
            InitGameIntroUI();
        }

        void Start()
        {
            ShowWinByIdx(0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void InitGameIntroUI()
        {
            GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/GameIntro");
            if (IntroList.Count > WinList.Count)
            {
                for (int i = WinList.Count; i < IntroList.Count; i++)
                {
                    RectTransform ob = Instantiate(itemPrefab).GetComponent<RectTransform>();
                    ob.parent = WinGroup;
                    WinList.Add(ob);
                }
            }

            for (int i = 0;  i < IntroList.Count; i++)
            {
                WinList[i].GetComponent<GameIntroInstance>().SetInstance(IntroList[i].text, IntroList[i].type, IntroList[i].path);
            }
        }

        void InitIntro()
        {
            IntroList.Add(new OneGameIntro(
                "游戏目标：是躲藏苟且，还是杀人还阳，这取决于你当前的身份。若陷入困境，无法完成当前的目标，就去寻找鬼门吧，穿过鬼门，身份互换，目标也会随之改变。", GAMETROTYPE.GAMEINTRO, ""));
            IntroList.Add(new OneGameIntro(
            "游戏操作：WASD控制人物上下左右，Z键攻击（仅限鬼魂身份），J键使用道具（仅限人类身份），Space跳跃。", GAMETROTYPE.GAMEINTRO, ""));
            IntroList.Add(new OneGameIntro(
                        "鬼门关：原是分隔阴阳的重镇，但如今镇将神荼不知所踪，鬼门关也再不能分隔阴阳，致使阴阳失衡，鬼物行于市。", GAMETROTYPE.SENCEINTRO, ""));
            IntroList.Add(new OneGameIntro(
                        "小鬼门：链接阴阳，门后守有判官，穿过它受审听判，可令阴阳倒转，人鬼互换。", GAMETROTYPE.TOOLINTRO, "Images/Scenes/NormalDoor"));
            IntroList.Add(new OneGameIntro(
                                    "大鬼门：直通阳间，原本重兵把守，现已无人看护，活人穿过它就能重返阳间。", GAMETROTYPE.TOOLINTRO, "Images/Scenes/FinalDoor"));
            IntroList.Add(new OneGameIntro(
                                    "糯米：可使用五次，使用后形成作用范围半径2米的圆形区域，有鬼祟之物碰到会令其产生反应。南方民间辟邪之物，由门后判官给予，大概是种嘲弄吧。", GAMETROTYPE.TOOLINTRO, "Images/Prop/nuomi"));
            IntroList.Add(new OneGameIntro(
                                    "如燕散：使用后提升角色移动速度50%。生死如渊，非燕雀而不得过。", GAMETROTYPE.TOOLINTRO, "Images/Prop/nuomi"));
            IntroList.Add(new OneGameIntro(
                                   "生死簿拓本残页：回复使用者生命值。阴阳失衡，就连本供神佛传阅的拓本，也面目全非。", GAMETROTYPE.TOOLINTRO, "Images/Prop/juanzhou"));
            IntroList.Add(new OneGameIntro(
                                               "忘川水：将使用者随机传送到场景中任一位置。原是界定生死之河，但如今已是随处可见，就连河水都清澈了不少。", GAMETROTYPE.TOOLINTRO, ""));
            IntroList.Add(new OneGameIntro(
                                               "孟婆汤：作用范围为半径10的圆形区域，孟婆取八苦熬制而成，虽是世间至苦，但鬼魂五感全无，只有这孟婆汤能尝出些许生味，因此对鬼魂诱惑极大，犹如毒品。", GAMETROTYPE.TOOLINTRO, ""));
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
            if (idx < 0 || idx >= WinList.Count)
                return;

            CurWinIdx = idx;
            for (int i = 0; i < WinList.Count; i++)
            {
                RectTransform child = WinList[i].Find("Text").GetComponent<RectTransform>();
                WinList[i].anchoredPosition = new Vector2(0, 0);
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
                WinList[i].SetSiblingIndex(WinList.Count - Mathf.Abs(i - CurWinIdx)-1);
            }
            
        }
    }



}

