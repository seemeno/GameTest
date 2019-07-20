using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class ShowSoulProp : PropBase
    {
        private float Range = 20.0f; //作用范围
        private float Duration = 20; //持续时间
        private GameObject icon;//地图上的标记
        private Color flashColour = new Color(1f, 0f, 0f, 0.1f);
        private Color originColour = new Color(0.5f, 1.0f, 0.1f, 0.2f);

        public override void UseMethod(Transform tmp)
        {
            Debug.Log("使用显示鬼魂道具");
            if (tmp.GetComponent<Player>().iCharcaterCount != (int)Charactors_type.Person)
                return;

            var entity = tmp.gameObject.GetComponent<Player>();
            string EnemyLayer = tmp.gameObject.GetComponent<PlayerAttack>().EnemyLayer;
            RectTransform rectTransform = MiniMap.Instance.ObjIconList["player"].GetComponent<RectTransform>().Find("RangeImage").GetComponent<RectTransform>();
            Image playerRangeImage = rectTransform.GetComponent<Image>();
            rectTransform.GetComponent<RangeImage>().SetRange(Range);
            TimeMgr.instance.AddTimer("ShowSoul", new TimeCount(Duration,
                ()=> {
                    if (entity != null && entity.iCharcaterCount == (int)Charactors_type.Person)
                    {
                        Collider[] colliders = Physics.OverlapSphere(tmp.position, Range, LayerMask.GetMask(EnemyLayer));
                        if (colliders.Length > 0)
                        {
                            //发现敌人，地图变色
                            playerRangeImage.color = flashColour;

                        }
                        else
                        {
                            playerRangeImage.color = originColour;
                        }
                    }
                    else
                    {
                        playerRangeImage.color = originColour;
                        rectTransform.gameObject.SetActive(false);
                    }
                }, 
                () =>{
                    //隐藏地图上Ghost
                    TimeMgr.instance.RemoveTimer("ShowSoul");
                    rectTransform.gameObject.SetActive(false);
                }, () => {
                    //在地图上显示Ghost
                    rectTransform.gameObject.SetActive(true);

                }

                ));
            
        }

        float GetDistance(Vector3 pos1, Vector3 pos2)
        {
            return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2.0f)+ Mathf.Pow(pos1.z - pos2.z, 2.0f));
        }

        public ShowSoulProp(int type, int GUID, string name, string Desc, string path, int count, int limit) : base(type, GUID, name, Desc,path,count,limit)
        {
        }
    }
}

