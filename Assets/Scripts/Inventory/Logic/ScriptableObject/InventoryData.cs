using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Invertory", menuName = "Inventory/Inventory Data")]
public class InventoryData : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();   //�����Ʒ�б�

    /// <summary>
    /// �����Ʒ�����
    /// </summary>
    /// <param name="newItemData">����Ʒ</param>
    /// <param name="amount">����</param>
    public void AddItem(ItemData newItemData, int amount)
    {
        bool found = false;

        if (newItemData.stackable)  //�ɶѵ�
        {
            //���ҿ���Ƿ��д���Ʒ
            foreach (var item in items)
            {
                if(item.itemData == newItemData)    //����
                {
                    item.amount += amount;  //��������
                    found = true;
                    break;
                }
            }
        }

        if (!found) //���û�ҵ�����Ʒ
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemData == null)    //Ѱ��һ���ո�
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
    public ItemData itemData;   //��Ʒ����
    public int amount;          //����
}
