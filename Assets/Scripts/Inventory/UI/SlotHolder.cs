using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子类型
/// </summary>
public enum SlotType
{
    BAG,    //背包
    WEAPON, //装备
    ACTION  //活动栏
}

public class SlotHolder : MonoBehaviour
{
    public SlotType slotType;   //格子类型
    public ItemUI itemUI;       //物品UI

    /// <summary>
    /// 更新物品：将库存列表物品数据与UI关联
    /// </summary>
    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;   //将库存的数据关联到背包UI
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;   //将装备栏的数据关联到背包UI
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;      //将活动栏的数据关联到背包UI
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];          //获得对应库存列表的item
        itemUI.SetupItemUI(item.itemData, item.amount);     //设置背包物品UI
    }
}
