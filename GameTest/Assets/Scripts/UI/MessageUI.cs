using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
public class MessageUI : MonoBehaviourPun
{
    public Text MessageText;
    public static MessageUI instance;
    public ScrollRect scroll;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        MessageText.text = "游戏开始！！！\n";
    }
    public void AddMessage(string text)
    {
        MessageText.text += text + "\n";

        Canvas.ForceUpdateCanvases();       //主要关键代码
        scroll.verticalNormalizedPosition = 0f;  //主要关键代码

    }
}
