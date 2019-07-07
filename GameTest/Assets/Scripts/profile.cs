using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public enum DOORTYPE : int
    {
        NORMALDOOR = 1,//常规门
        TRANSFERDOOR = 2, //传送门
        FINALDOOR = 3,//大鬼门
    }

    public enum GAMESTATE : int
    {
        WAITSTAGE = 0,//刚进入游戏，即将开始阶段
        FIRSTSTAGE = 1, //游戏第一阶段
        SECONDSTAGE = 2,//游戏第二阶段
        ENDSTAGE = 3,//游戏结束
    };

    public enum PROPGUID : int
    {
        //道具的GUID唯一标识
        SPEEDUP = 1,//加速道具
        CHANGEPLAYERTYPE = 2,//角色转换
        RECOVER = 3,//回血
        RANDOMPOS = 4,//随机传送位置
        SHOWSOUL = 5,//鬼魂接近提示


    }
}

