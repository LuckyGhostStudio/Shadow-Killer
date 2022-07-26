using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;      //物品图片
    public Text amount;     //物品数量

    public InventoryData Bag { get; set; }  //库存数据
    public int Index { get; set; } = -1;    //物品序号（在背包中的序号：在库存列表中的索引）

    /// <summary>
    /// 设置物品UI
    /// </summary>
    /// <param name="itemData">物品数据</param>
    /// <param name="itemAmount">数量</param>
    public void SetupItemUI(ItemData itemData, int itemAmount)
    {
        if (itemData != null)
        {
            icon.sprite = itemData.itemIcon;
            amount.text = itemAmount.ToString();

            icon.gameObject.SetActive(true);    //启用icon所在对象
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }
}
