using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Invertory", menuName = "Inventory/Inventory Data")]
public class InventoryData : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();   //库存物品列表

    /// <summary>
    /// 添加物品到库存
    /// </summary>
    /// <param name="newItemData">新物品</param>
    /// <param name="amount">数量</param>
    public void AddItem(ItemData newItemData, int amount)
    {
        bool found = false;

        if (newItemData.stackable)  //可堆叠
        {
            //查找库存是否有此物品
            foreach (var item in items)
            {
                if(item.itemData == newItemData)    //存在
                {
                    item.amount += amount;  //数量增加
                    found = true;
                    break;
                }
            }
        }

        if (!found) //库存没找到此物品
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemData == null)    //寻找一个空格
                {
                    items[i].itemData = newItemData;
                    items[i].amount = amount;
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;   //物品数据
    public int amount;          //数量
}
