using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Com.MyCompany.MyGame
{
    public class TimeCount
    {
        public float Duration;//时长
        public float LeftTime;//剩余时长
        private Action _updateAction;//计时期间在update中调用函数
        private Action _callAction;//计时结束回调函数
        private Action _intiAction;//计时开始前调用函数
        private bool _isPause;//是否计时
        private int LeftNum;//剩余计时次数


        public TimeCount(float duration, Action updateAction = null,
            Action callAction = null, Action intiAction = null, int maxNum = 1)
        {
            LeftTime = duration;
            Duration = duration;
            if (intiAction != null) intiAction.Invoke();
            _intiAction = intiAction;
            _updateAction = updateAction;
            _callAction = callAction;
            _isPause = false;
            LeftNum = maxNum - 1;
        }
        public void OnUpdate(float deltaTime)
        {
            LeftTime -= Time.deltaTime;
            if (LeftTime <= 0)
            {
                if (LeftNum > 0)
                {
                    LeftNum--;
                    _intiAction.Invoke();
                    LeftTime = Duration;
                }
                else if (_callAction != null)
                {
                    _callAction.Invoke();
                }
            }
            else
            {
                if (_updateAction != null && !_isPause)
                    _updateAction.Invoke();
            }
        }

        public void SetTimerTrick(bool b)
        {
            _isPause = b;
        }
    }
}

