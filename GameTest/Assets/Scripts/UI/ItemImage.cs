using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

public class ItemImage : MonoBehaviour
{

    public Text itemName;
    public Text itemCount;

    //本节以字体为例

    public void UpdateItem(string name,int count)
    {

        itemName.text = name;
        itemCount.text = count.ToString().PadLeft(1);


    }

    //实际多是图片，可以使用这个更新图片

    public Image itemImage;

    public void UpdateItemImage(Sprite icon)

    {

        itemImage.sprite = icon;

    }

}