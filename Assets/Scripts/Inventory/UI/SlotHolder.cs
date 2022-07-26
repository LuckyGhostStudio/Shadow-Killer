using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public enum SlotType
{
    BAG,    //����
    WEAPON, //װ��
    ACTION  //���
}

public class SlotHolder : MonoBehaviour
{
    public SlotType slotType;   //��������
    public ItemUI itemUI;       //��ƷUI

    /// <summary>
    /// ������Ʒ��������б���Ʒ������UI����
    /// </summary>
    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;   //���������ݹ��������UI
                break;
            case SlotType.WEAPON:
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];          //��ÿ���б��item
        itemUI.SetupItemUI(item.itemData, item.amount);     //���ñ�����ƷUI
    }
}
