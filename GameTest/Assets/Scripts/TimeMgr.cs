using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class TimeMgr : MonoBehaviour
    {
        //计时器管理器
        public static TimeMgr instance;
        private List<TimeCount> _timers;
        private Dictionary<string, TimeCount> _timerDict;


        private void Awake()
        {
            _timers = new List<TimeCount>();
            _timerDict = new Dictionary<string, TimeCount>();
            instance = this;
        }
        private void Update()
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].OnUpdate(Time.deltaTime);
            }
        }

        public void AddTimer(string str, TimeCount timer)
        {
            //添加计时器
            if (_timerDict.ContainsKey(str))
            {
                _timerDict[str].LeftTime += _timerDict[str].Duration;

            }
            else
            {
                _timerDict.Add(str, timer);
                _timers.Add(timer);
            }
        }
        public void RemoveTimer(string str)
        {
            //删除计时器
            var timer = _timerDict[str];
            if (timer != null)
            {
                _timers.Remove(timer);
                _timerDict.Remove(str);

            }
        }
        public bool IfExistTimer(string str)
        {
            //判断计时器是否存在
            var timer = _timerDict[str];
            if (timer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

        
